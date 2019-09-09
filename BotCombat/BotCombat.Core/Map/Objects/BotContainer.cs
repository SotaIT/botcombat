using BotCombat.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BotCombat.Core
{
    public class BotContainer : MapObject
    {
        public int Id { get => _bot.Id; }

        /// <summary>
        /// Increases the number of hitpoints
        /// </summary>
        public int Stamina
        {
            get => PowerValues[PowerStats.Stamina];
        }

        /// <summary>
        /// Increases the number of damage done
        /// </summary>
        public int Strength
        {
            get => PowerValues[PowerStats.Strength];
        }

        private int _power;

        private Dictionary<PowerStats, int> _powerValues;

        private Dictionary<PowerStats, int> PowerValues
        {
            get => _powerValues;
            set
            {
                _powerValues = value;
                CheckPowerDistribution();
            }
        }

        private readonly IBot _bot;


        public BotContainer(IBot bot, int x, int y, int power, Step step) : base(x, y, bot.BotImage)
        {
            _bot = bot;
            _power = power;
            PowerValues = _bot.InitPower(_power, step);
        }

        private void CheckPowerDistribution()
        {
            if (_power != PowerValues.Values.Sum())
                throw new Exception("Incorrect power distribution!");
        }

        public MoveDirection ChooseDirection(Step step)
        {
            return _bot.ChooseDirection(step);
        }

        public void AddBonus(int power, Step step)
        {
            _power += power;
            PowerValues = _bot.DistributePower(_power, step);
        }
    }
}
