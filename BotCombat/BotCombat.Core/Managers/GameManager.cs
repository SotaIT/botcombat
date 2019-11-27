using System;
using System.Collections.Generic;
using System.Linq;
using BotCombat.BotWorld;

namespace BotCombat.Core
{
    /// <summary>
    /// The game manager
    /// </summary>
    public class GameManager
    {
        private readonly List<Log> _logs = new List<Log>();

        private readonly MapManager _map;

        private readonly MapSettings _settings;

        private readonly Map _mapModel;

        private Game _game;

        private readonly List<Step> _steps = new List<Step>();

        private readonly Dictionary<int, int> _bonusSpawn = new Dictionary<int, int>();

        public GameManager(MapSettings mapSettings, IEnumerable<IBot> bots)
        {
            _settings = mapSettings;
            _mapModel = new Map(_settings.Id, _settings.Width, _settings.Height, _settings.Walls, _settings.Traps);
            _map = new MapManager(_settings);

            AddBots(bots);
            CreateStep();
        }

        private int LogNumber => _logs.Count;
        private int StepNumber => _steps.Count;

        /// <summary>
        /// Adds the bots to the map
        /// </summary>
        private void AddBots(IEnumerable<IBot> bots)
        {
            var availablePoints = _settings.StartPoints.Count > 0
                ? _settings.StartPoints.ToList()
                : _map.GetEmptyPoints();
            var random = new Random();

            foreach (var bot in bots)
            {
                if (availablePoints.Count == 0)
                    throw new Exception("Not enough place for bots!");

                var pointIndex = random.Next(availablePoints.Count);
                var point = availablePoints[pointIndex];
                availablePoints.RemoveAt(pointIndex);

                _map.Add(new BotManager(bot, point.X, point.Y, _settings, _game));
            }
        }

        private void CreateStep()
        {
            _steps.Add(new Step(
                StepNumber,
                _map.Bonuses.ToList(),
                _map.Bots.Select(b => b.ToBot()).ToDictionary(b => b.Id, b => b),
                _map.Bullets.Select(b => b.ToBullet()).ToList(),
                _logs.Where(l => l.Step == StepNumber).ToList()
            ));

            _game = new Game(_mapModel, _steps);
        }

        private bool MakeStep()
        {
            ReSpawnBonuses();
            PerformBotActions();
            ComputeCollisions();
            MoveBullets();
            CreateStep();

            var lastStep = _game.Steps[^1];
            return lastStep.Number <= _settings.MaxStepCount && lastStep.Bots.Count > 1;
        }

        private void ReSpawnBonuses()
        {
            _map.AddRange(_bonusSpawn
                .Where(bs => bs.Value == StepNumber)
                .Select(bs => _settings.Bonuses.FirstOrDefault(b => b.Id == bs.Key)), false);
        }

        public Game Play()
        {
            while (MakeStep()) { }

            return _game;
        }

        private void RemoveDeadBots()
        {
            var deadBots = _map.Bots.Where(b => b.IsDead).ToList();
            foreach (var bot in deadBots)
            {
                _map.Remove(bot);
                AddLog(LogType.Killed, bot.X, bot.Y, 0, bot.Id, 0);
            }
        }

        private void PerformBotActions()
        {
            RemoveDeadBots();
            foreach (var bot in _map.Bots.ToList())
                PerformBotAction(bot);
        }

        private void PerformBotAction(BotManager bot)
        {
            _map.Remove(bot);
            var bullet = bot.Perform(_game);
            _map.Add(bot);

            if (bullet != null)
                _map.Add(bullet);
        }

        private void ComputeCollisions()
        {
            // compute bonuses and damage taken
            for (var x = 0; x < _settings.Width; x++)
                for (var y = 0; y < _settings.Height; y++)
                    if (_map.IsCollision(x, y))
                    {
                        var bots = _map.GetObjects<BotManager>(x, y);
                        if (bots.Count == 0) continue;

                        ApplyBonus(x, y, bots);
                        ApplyTrap(x, y, bots);
                        ApplyIntersection(bots);
                    }

        }

        /// <summary>
        /// All bots in the current point gets a piece of bonus power
        /// The bonus is removed from the point
        /// </summary>
        private void ApplyBonus(int x, int y, IReadOnlyCollection<BotManager> bots)
        {
            var bonus = _map.GetObject<Bonus>(x, y);
            if (bonus == null) return;

            // the power is divided by the number of bots
            var power = bonus.Power / bots.Count;

            // add bonus to each bot
            foreach (var bot in bots)
            {
                bot.TakeBonus(_game, power);
                AddLog(LogType.Bonus, bonus.X, bonus.Y, bonus.Id, bot.Id, bonus.Power);
            }

            // remove bonus from the map
            _map.Remove(bonus);

            // ste respawn if the map allows that
            if (_settings.BonusSpawnInterval > 0)
                _bonusSpawn[bonus.Id] = StepNumber + _settings.BonusSpawnInterval;
        }

        private void AddLog(LogType type, int x, int y, int sourceId, int targetId, int value)
        {
            _logs.Add(new Log(LogNumber, type, StepNumber, x, y, sourceId, targetId, value));
        }

        /// <summary>
        /// All bots in the current point get damage
        /// </summary>
        private void ApplyTrap(int x, int y, IEnumerable<BotManager> bots)
        {
            var trap = _map.GetObject<Trap>(x, y);
            if (trap == null) return;

            foreach (var bot in bots)
                TrapDamage(trap, bot);
        }

        /// <summary>
        /// Damage intersecting bots: each bot damages to all other bots
        /// </summary>
        private void ApplyIntersection(IReadOnlyCollection<BotManager> bots)
        {
            if (bots.Count < 2) return;

            foreach (var bot in bots)
                foreach (var targetBot in bots.Where(targetBot => bot.Id != targetBot.Id))
                    BotDamage(bot, targetBot);
        }

        /// <summary>
        /// Bulletes move and make damage
        /// </summary>
        private void MoveBullets()
        {
            // remove exploaded bullets
            foreach (var bullet in _map.GetObjects<BulletManager>().Where(b => b.Exploded).ToList())
                _map.Remove(bullet);

            for (var i = 0; i < _settings.BulletSpeed; i++)
                foreach (var bullet in _map.GetObjects<BulletManager>().Where(b => !b.Exploded).ToList())
                {
                    _map.Remove(bullet);
                    bullet.Move(_game);
                    _map.Add(bullet);

                    if (!bullet.Exploded)
                        ApplyBullet(bullet);
                }
        }

        /// <summary>
        /// Make damage by bullet
        /// </summary>
        private void ApplyBullet(BulletManager bullet)
        {
            var bots = _map.GetObjects<BotManager>(bullet.X, bullet.Y);
            if (bots.Count == 0) return;

            foreach (var bot in bots)
                BulletDamage(bullet, bot);

            bullet.Explode();
        }

        private void TrapDamage(Trap trap, BotManager target)
        {
            Damage(LogType.Trap, trap, target);
        }

        private void BotDamage(BotManager source, BotManager target)
        {
            Damage(LogType.Damaged, source, target);
        }

        private void BulletDamage(BulletManager bullet, BotManager bot)
        {
            Damage(LogType.Ranged, bullet, bot);
        }

        private void Damage(LogType logType, IDamager source, BotManager target)
        {
            target.TakeDamage(_game, source.Damage);
            AddLog(logType, source.X, source.Y, source.Id, target.Id, source.Damage);
        }
    }
}