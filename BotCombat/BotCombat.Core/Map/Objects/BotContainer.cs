﻿using System;
using System.Collections.Generic;
using System.Linq;
using BotCombat.Abstractions;

namespace BotCombat.Core
{
    public class BotContainer : MapObject
    {
        private readonly IBot _bot;

        private int _power;

        private Dictionary<PowerStats, int> _powerValues;

        public BotContainer(IBot bot, int x, int y, int power, Step step) : base(x, y, bot.BotImage)
        {
            _bot = bot;
            _power = power;
            PowerValues = _bot.InitPower(_power, step);
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