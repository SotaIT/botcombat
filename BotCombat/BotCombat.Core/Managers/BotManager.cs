using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotCombat.BotWorld;

namespace BotCombat.Core
{
    internal class BotManager : IDamager
    {
        private readonly IBot _iBot;
        private readonly MapSettings _settings;
        private PowerStats _powerStats;
        private int _damageTaken;
        private int _bulletCount;

        public BotManager(IBot bot,
            int x,
            int y,
            MapSettings mapSettings,
            Game game)
        {
            _iBot = bot;
            _settings = mapSettings;
            X = x;
            Y = y;
            Power = _settings.InitialPower;
            DistributePower(game);
        }

        /// <summary>
        /// Current health of the bot
        /// </summary>
        public int Health => PowerStats.Stamina * _settings.StaminaWeight - DamageTaken;

        /// <summary>
        /// Current damage of the bot
        /// </summary>
        public int Damage => PowerStats.Strength * _settings.StrengthWeight;

        /// <summary>
        /// Current damage of the bot
        /// </summary>
        public int Ranged => PowerStats.Ranged * _settings.RangedWeight;

        /// <summary>
        /// Is the bot dead
        /// </summary>
        public bool IsDead => Health < 1;

        private PowerStats PowerStats
        {
            get => _powerStats;
            set
            {
                _powerStats = value;
                CheckPowerDistribution();
            }
        }

        private int DamageTaken
        {
            get => _damageTaken;
            set
            {
                IsDamaged = true;
                _damageTaken = value;
            }
        }

        private bool IsDamaged { get; set; }

        private int Power { get; set; }

        public int Id => _iBot.Id;

        public int X { get; private set; }

        public int Y { get; private set; }

        public Direction Direction { get; private set; } = Direction.Up;

        public bool IsStunned { get; private set; }

        private readonly List<DebugMessage> _debugMessages = new List<DebugMessage>();

        /// <summary>
        /// Performs the bot action
        /// </summary>
        public BulletManager Perform(Game game)
        {
            _debugMessages.Clear();

            // if the tank was hit by a bullet
            // it can't do anything for one step
            if (IsStunned)
            {
                IsStunned = false;
                return null;
            }

            var botAction = ChooseAction(game);
            // bot does nothing
            if (botAction == null || botAction.Value == BotAction.Stop)
                return null;

            // Shoot
            if (botAction.Value == BotAction.Shoot)
                return Shoot();

            // move
            Move(game, botAction.Value);

            return null;
        }

        private void Move(Game game, BotAction botAction)
        {
            // reset damaged
            IsDamaged = false;

            // save last direction to be able to show to which direction is the bot turn on
            Direction = botAction.ToDirection();

            // calculate the destination point
            var point = game.Map.GetMoveDestination(botAction, this);

            // can't move to specified point
            if (point.IsRestricted)
                return;

            X = point.X;
            Y = point.Y;
        }

        private BulletManager Shoot()
        {
            return new BulletManager(Id, _bulletCount++, X, Y, Ranged, Direction);
        }

        public Bot ToBot()
        {
            return new Bot(Id, X, Y, Health, Damage, (int)Direction, IsDamaged, IsStunned);
        }

        private void CheckPowerDistribution()
        {
            if (Power != PowerStats.Power)
                Error("Incorrect power distribution!");
        }

        private BotAction? ChooseAction(Game game)
        {
            return CallBotMethod(() => _iBot.ChooseAction(game))?.BotAction;
        }

        private void DistributePower(Game game)
        {
            var stats = CallBotMethod(() => _iBot.DistributePower(game, Power))?.Stats;
            if (stats != null)
                PowerStats = stats;
        }

        public void TakeBonus(Game game, int power)
        {
            Power += power;
            DistributePower(game);
        }

        public void TakeDamage(Game game, int damage)
        {
            DamageTaken += damage;
            DistributePower(game);
        }

        private void Error(string message)
        {
            AddDebugMessage(message, true);
            DamageTaken += Health + 1;
        }

        private void AddDebugMessage(string message, bool error = false)
        {
            _debugMessages.Add(new DebugMessage { BotId = Id, Message = message, Error = error });
        }

        private TResult CallBotMethod<TResult>(Func<TResult> func) where TResult : BaseResult
        {
            try
            {
                var task = Task.Run(func);
                if (!task.Wait(TimeSpan.FromSeconds(_settings.ActionTimeout)))
                    Error("Timeout expired!");
                var result = task.Result;

                var messages = result.GetDebugMessages();
                if (messages != null && messages.Length > 0)
                    foreach (var m in messages)
                        AddDebugMessage(m);

                return result;
            }
            catch (Exception ex)
            {
                Error(ex.Message);
                return null;
            }
        }

        public void Stun()
        {
            IsStunned = true;
        }

        public DebugMessage[] GetDebugMessages()
        {
            return _debugMessages.ToArray();
        }
    }
}