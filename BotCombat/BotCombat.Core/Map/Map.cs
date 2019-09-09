using BotCombat.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BotCombat.Core
{
    public class Map
    {
        private readonly List<IMapObject>[,] _mapPoints;

        private readonly MapSettings _mapSettings;

        private readonly List<Wall> _walls = new List<Wall>();

        private readonly List<Bonus> _bonuses = new List<Bonus>();

        private readonly List<BotContainer> _bots = new List<BotContainer>();

        private readonly List<BotContainer> _deadBots = new List<BotContainer>();

        private readonly Dictionary<int, int> _damageTaken = new Dictionary<int, int>();

        private readonly List<Step> _steps = new List<Step>();

        private readonly List<DamageLog> _damageLogs = new List<DamageLog>();

        public Step CurrentStep { get => _steps[_steps.Count - 1]; }

        public Map(MapSettings mapSettings, List<IBot> bots)
        {
            _mapSettings = mapSettings;
            _mapPoints = new List<IMapObject>[_mapSettings.Width, _mapSettings.Height];

            InitMapPoints();

            AddWalls();

            AddBonuses();

            AddBots(bots);
        }

        #region MapPoints

        private void AddToPoint(IMapObject mapObject)
        {
            _mapPoints[mapObject.X, mapObject.Y].Add(mapObject);
        }

        private void RemoveFromPoint(IMapObject mapObject)
        {
            _mapPoints[mapObject.X, mapObject.Y].Remove(mapObject);
        }

        private bool IsWall(int x, int y)
        {
            return _mapPoints[x, y].Any(o => o is Wall);
        }
        private Bonus FindBonus(int x, int y)
        {
            return _mapPoints[x, y].FirstOrDefault(o => o is Bonus) as Bonus;
        }

        private List<BotContainer> GetBots(int x, int y)
        {
            return _mapPoints[x, y].Where(o => o is BotContainer).Cast<BotContainer>().ToList();
        }

        #endregion

        #region init

        private void InitMapPoints()
        {
            for (var x = 0; x < _mapSettings.Width; x++)
                for (var y = 0; y < _mapSettings.Height; y++)
                    _mapPoints[x, y] = new List<IMapObject>();
        }

        private void AddWalls()
        {
            foreach (var wall in _mapSettings.Walls)
                if (_mapPoints[wall.X, wall.Y].Count == 0)
                {
                    _mapPoints[wall.X, wall.Y].Add(wall);
                    _walls.Add(wall);
                }
            CreateStep();
        }

        private void AddBonuses()
        {
            foreach (var bonus in _mapSettings.Bonuses)
                if (_mapPoints[bonus.X, bonus.Y].Count == 0)
                {
                    _mapPoints[bonus.X, bonus.Y].Add(bonus);
                    _bonuses.Add(bonus);
                }
            CreateStep();
        }

        private void AddBots(List<IBot> bots)
        {
            var emptyPoints = GetEmptyPoints();
            var random = new Random();

            foreach (var bot in bots)
            {
                var pointIndex = random.Next(emptyPoints.Count);
                var point = emptyPoints[pointIndex];
                emptyPoints.RemoveAt(pointIndex);

                var botContainer = new BotContainer(bot, point.X, point.Y, _mapSettings.InitialPower, CurrentStep);
                _bots.Add(botContainer);
                _mapPoints[point.X, point.Y].Add(botContainer);
                _damageTaken[botContainer.Id] = 0;
            }
            CreateStep();
        }

        private List<Coordinates> GetEmptyPoints()
        {
            var list = new List<Coordinates>();

            for (var x = 0; x < _mapSettings.Width; x++)
                for (var y = 0; y < _mapSettings.Height; y++)
                    if (_mapPoints[x, y].Count == 0)
                        list.Add(new Coordinates { X = x, Y = y });

            return list;
        }

        #endregion

        private void CreateStep()
        {
            _steps.Add(new Step(
                _steps.Count,
                _mapSettings.Id,
                _mapSettings.Width,
                _mapSettings.Height,
                _walls.ToMapObjectModels(),
                _bonuses.ToMapObjectModels(),
                _bots.ToMapBotModels(),
                _damageLogs.Where(l => l.Step == _steps.Count).ToDamageLogModels()
                ));
        }

        public Step Step()
        {
            MoveBots();
            ComputeCollisions();
            CreateStep();
            return CurrentStep;
        }

        private void MoveBots()
        {
            var currentStep = CurrentStep;
            foreach (var bot in _bots)
                MoveBot(bot, currentStep);
        }

        public BotContainer MoveBot(BotContainer bot, Step step)
        {
            var direction = bot.ChooseDirection(step);
            // bot doesn't want to move - stop
            if (direction == MoveDirection.None)
                return bot;

            // calculate the destination point
            var point = MapUtils.GetDestination(bot.X, bot.Y, direction);

            // bot is going to move out of the map - stop
            if (point.X >= _mapSettings.Width || point.X < 0 || point.Y >= _mapSettings.Height || point.Y < 0)
                return bot;

            // if there is a wall at the destination point - stop
            if (IsWall(point.X, point.Y))
                return bot;

            RemoveFromPoint(bot);
            bot.Move(direction);
            AddToPoint(bot);
            return bot;
        }

        private void ComputeCollisions()
        {
            // compute bonuses and damage taken
            for (var x = 0; x < _mapSettings.Width; x++)
                for (var y = 0; y < _mapSettings.Height; y++)
                    if (_mapPoints[x, y].Count > 1)
                    {
                        var bonus = FindBonus(x, y);
                        var bots = GetBots(x, y);

                        AddBonuses(bonus, bots);
                        DamageIntersectingBots(bots);
                    }

            // remove dead bots
            var deadBots = _bots.Where(b => _damageTaken[b.Id] >= b.Stamina * _mapSettings.StaminaWeight).ToList();
            foreach (var bot in deadBots)
            {
                _deadBots.Add(bot);
                _bots.Remove(bot);
                RemoveFromPoint(bot);
            }
        }

        /// <summary>
        /// All bots in the current point get bonus power
        /// The bonus is removed from the point
        /// </summary>
        private void AddBonuses(Bonus bonus, List<BotContainer> bots)
        {
            if (bonus == null) return;

            // add bonus to each bot
            foreach (var bot in bots)
                bot.AddBonus(bonus.Power, CurrentStep);

            // remove bonus from the map
            RemoveFromPoint(bonus);
            _bonuses.Remove(bonus);
        }

        /// <summary>
        /// Damage intersecting bots: each bot damages to all other bots
        /// </summary>
        private void DamageIntersectingBots(List<BotContainer> bots)
        {
            if (bots.Count < 2) return;

            foreach (var damagerBot in bots)
                foreach (var damagedBot in bots)
                    if (damagerBot.Id != damagedBot.Id) // todo: implement Equals
                        Damage(damagerBot, damagedBot);
        }

        private void Damage(BotContainer source, BotContainer target)
        {
            var damage = source.Strength * _mapSettings.StrengthWeight;
            _damageTaken[target.Id] += damage;
            _damageLogs.Add(new DamageLog(_steps.Count, source.X, source.Y, source.Id, target.Id, damage));
        }
    }
}
