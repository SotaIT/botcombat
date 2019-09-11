using System;
using BotCombat.Abstractions;
using BotCombat.Abstractions.BotModels;

namespace BotCombat.Core.Models
{
    internal class BotContainer : MapObject
    {
        private readonly IBot _bot;

        private int _power;

        private PowerStats _powerStats;

        public BotContainer(IBot bot, int x, int y, int power, Game game) : base(x, y)
        {
            _bot = bot;
            _power = power;
            PowerStats = _bot.DistributePower(_power, game);
        }

        public int Id => _bot.Id;

        /// <summary>
        /// Increases the health
        /// </summary>
        public int Stamina => PowerStats.Stamina;

        /// <summary>
        /// Increases the number of damage done
        /// </summary>
        public int Strength => PowerStats.Strength;

        private PowerStats PowerStats
        {
            get => _powerStats;
            set
            {
                _powerStats = value;
                CheckPowerDistribution();
            }
        }

        private void CheckPowerDistribution()
        {
            if (_power != PowerStats.Power)
                throw new ArgumentOutOfRangeException(nameof(PowerStats), "Incorrect power distribution!");
        }

        public MoveDirection ChooseDirection(Game game)
        {
            return _bot.ChooseDirection(game);
        }

        public void AddBonus(int power, Game game)
        {
            _power += power;
            PowerStats = _bot.DistributePower(_power, game);
        }
    }
}