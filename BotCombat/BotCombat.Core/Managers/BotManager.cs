using System;
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

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; private set; }

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
                Damaged = true;
                _damageTaken = value;
            }
        }

        private bool Damaged { get; set; }

        private int Power { get; set; }

        public int Id => _iBot.Id;

        public int X { get; private set; }

        public int Y { get; private set; }

        public BotAction Direction { get; private set; } = BotAction.MoveRight;

        /// <summary>
        /// Performs the bot action
        /// </summary>
        public BulletManager Perform(Game game)
        {
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
            Damaged = false;

            // save last direction to be able to show to which direction is the bot turn on
            Direction = botAction;

            // calculate the destination point
            var point = game.Map.GetDestination(botAction, this);

            // can't move to specified point
            if (point.IsWall)
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
            return new Bot(Id, X, Y, Health, Damage, ErrorMessage, (int)Direction, Damaged);
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
            var stats= CallBotMethod(() => _iBot.DistributePower(game, Power))?.Stats;
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
            ErrorMessage = message;
            DamageTaken += Health + 1;
        }

        private TResult CallBotMethod<TResult>(Func<TResult> func) where TResult : class
        {
            try
            {
                var task = Task.Run(func);
                if (!task.Wait(TimeSpan.FromSeconds(2)))
                    Error("Timeout expired!");
                return task.Result;
            }
            catch (Exception ex)
            {
                Error(ex.Message);
                return null;
            }
        }
    }
}