using System;
using System.Collections.Generic;
using System.Linq;
using BotCombat.Abstractions;
using BotCombat.Abstractions.Models;

namespace BotCombat.Core
{
    public class BotContainer : MapObject
    {
        private readonly IBot _bot;

        private int _power;

        private Dictionary<PowerStats, int> _powerValues;

        public BotContainer(IBot bot, int x, int y, int power, Game game) : base(x, y)
        {
            _bot = bot;
            _power = power;
            PowerValues = _bot.InitPower(_power, game);
        }

        public int Id => _bot.Id;

        /// <summary>
        /// Increases the health
        /// </summary>
        public int Stamina => PowerValues[PowerStats.Stamina];

        /// <summary>
        /// Increases the number of damage done
        /// </summary>
        public int Strength => PowerValues[PowerStats.Strength];

        private Dictionary<PowerStats, int> PowerValues
        {
            get => _powerValues;
            set
            {
                _powerValues = value;
                CheckPowerDistribution();
            }
        }

        private void CheckPowerDistribution()
        {
            if (_power != PowerValues.Values.Sum())
                throw new Exception("Incorrect power distribution!");
        }

        public MoveDirection ChooseDirection(Game game)
        {
            return _bot.ChooseDirection(game);
        }

        public void AddBonus(int power, Game game)
        {
            _power += power;
            PowerValues = _bot.DistributePower(_power, game);
        }
    }
}