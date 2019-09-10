using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BotCombat.Abstractions;
using BotCombat.Abstractions.BotModels;
using BotCombat.Core.Models;
using Log = BotCombat.Core.Models.Log;
using Map = BotCombat.Core.Models.Map;

namespace BotCombat.Core
{
    public class MapManager
    {
        private readonly List<Bonus> _bonuses = new List<Bonus>();

        private readonly List<BotContainer> _bots = new List<BotContainer>();

        private readonly List<Log> _logs = new List<Log>();

        private readonly Dictionary<int, int> _damageTaken = new Dictionary<int, int>();

        private readonly Dictionary<int, List<BotContainer>> _deadBots = new Dictionary<int, List<BotContainer>>();

        private readonly List<IMapObject>[,] _mapPoints;

        private readonly Map _map;

        private readonly Abstractions.BotModels.Map _mapModel;

        private Game _game;

        private readonly List<Step> _steps = new List<Step>();

        private readonly List<Wall> _walls = new List<Wall>();

        public MapManager(Map map, List<IBot> bots)
        {
            _map = map;
            _mapPoints = new List<IMapObject>[_map.Width, _map.Height];
            _mapModel = new Abstractions.BotModels.Map(_map.Id, _map.Width, map.Height, _walls.ToMapObjectModels());

            InitMapPoints();

            AddWalls();

            AddBonuses();

            AddBots(bots);
        }

        private List<BotContainer> CurrentStepDeadBots
        {
            get
            {
                var currentStepIndex = _steps.Count;
                if(!_deadBots.ContainsKey(currentStepIndex))
                    _deadBots[currentStepIndex] = new List<BotContainer>();
                return _deadBots[currentStepIndex];
            }
        }

        private void CreateStep()
        {
            _steps.Add(new Step(
                _steps.Count,
                _bonuses.ToMapObjectModels(),
                _bots.ToMapBotModels(),
                _logs.Where(l => l.Step == _steps.Count).ToLogModels(),
                CurrentStepDeadBots.Select(b => b.Id).ToList()
            ));

            _game = new Game(_mapModel, _steps);
        }

        public Game MakeStep()
        {
            MoveBots();
            ComputeCollisions();
            CreateStep();
            return _game;
        }

        private void MoveBots()
        {
            foreach (var bot in _bots)
                MoveBot(bot, _game);
        }

        private void MoveBot(BotContainer bot, Game game)
        {
            var direction = bot.ChooseDirection(game);
            // bot doesn't want to move - stop
            if (direction == MoveDirection.None)
                return;

            // calculate the destination point
            var point = MapUtils.GetDestination(bot.X, bot.Y, direction);

            // bot is going to move out of the map - stop
            if (point.X >= _map.Width || point.X < 0 || point.Y >= _map.Height || point.Y < 0)
                return;

            // if there is a wall at the destination point - stop
            if (IsWall(point.X, point.Y))
                return;

            RemoveFromPoint(bot);
            bot.Move(direction);
            AddToPoint(bot);
        }

        private void ComputeCollisions()
        {
            // compute bonuses and damage taken
            for (var x = 0; x < _map.Width; x++)
                for (var y = 0; y < _map.Height; y++)
                    if (_mapPoints[x, y].Count > 1)
                    {
                        var bonus = FindBonus(x, y);
                        var bots = GetBots(x, y);

                        AddBonuses(bonus, bots);
                        DamageIntersectingBots(bots);
                    }

            // remove dead bots
            var deadBots = _bots.Where(b => _damageTaken[b.Id] >= b.Stamina * _map.StaminaWeight).ToList();
            foreach (var bot in deadBots)
            {
                CurrentStepDeadBots.Add(bot);
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
            {
                bot.AddBonus(bonus.Power, _game);
                _logs.Add(new Log(_steps.Count, bonus.X, bonus.Y, 0, bot.Id, bonus.Power));
            }

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

            foreach (var bot in bots)
                foreach (var targetBot in bots.Where(targetBot => bot.Id != targetBot.Id))
                    Damage(bot, targetBot);
        }

        private void Damage(BotContainer source, BotContainer target)
        {
            var damage = source.Strength * _map.StrengthWeight;
            _damageTaken[target.Id] += damage;
            _logs.Add(new Log(_steps.Count, source.X, source.Y, source.Id, target.Id, damage));
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
            for (var x = 0; x < _map.Width; x++)
                for (var y = 0; y < _map.Height; y++)
                    _mapPoints[x, y] = new List<IMapObject>();
        }

        private void AddWalls()
        {
            foreach (var wall in _map.Walls)
                if (_mapPoints[wall.X, wall.Y].Count == 0)
                {
                    _mapPoints[wall.X, wall.Y].Add(wall);
                    _walls.Add(wall);
                }

            CreateStep();
        }

        private void AddBonuses()
        {
            foreach (var bonus in _map.Bonuses)
                if (_mapPoints[bonus.X, bonus.Y].Count == 0)
                {
                    _mapPoints[bonus.X, bonus.Y].Add(bonus);
                    _bonuses.Add(bonus);
                }

            CreateStep();
        }

        private void AddBots(IEnumerable<IBot> bots)
        {
            var emptyPoints = GetEmptyPoints();
            var random = new Random();

            foreach (var bot in bots)
            {
                var pointIndex = random.Next(emptyPoints.Count);
                var point = emptyPoints[pointIndex];
                emptyPoints.RemoveAt(pointIndex);

                var botContainer = new BotContainer(bot, point.X, point.Y, _map.InitialPower, _game);
                _bots.Add(botContainer);
                _mapPoints[point.X, point.Y].Add(botContainer);
                _damageTaken[botContainer.Id] = 0;
            }

            CreateStep();
        }

        private List<Coordinates> GetEmptyPoints()
        {
            var list = new List<Coordinates>();

            for (var x = 0; x < _map.Width; x++)
                for (var y = 0; y < _map.Height; y++)
                    if (_mapPoints[x, y].Count == 0)
                        list.Add(new Coordinates { X = x, Y = y });

            return list;
        }

        #endregion
    }
}