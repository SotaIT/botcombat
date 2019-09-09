using System;
using System.Collections.Generic;
using BotCombat.Abstractions;

namespace BotCombat.Cs
{
    public class DefaultCsBot : BaseBot
    {
        public DefaultCsBot(int id, MapImage botImage) : base(id, botImage)
        {
        }

        public override MoveDirection ChooseDirection(Step step)
        {
            return (MoveDirection)(new Random().Next(4) + 1);
        }

        public override Dictionary<PowerStats, int> DistributePower(int power, Step step)
        {
            var strength = power / 2;

            return new Dictionary<PowerStats, int>
            {
                [PowerStats.Strength] = strength,
                [PowerStats.Stamina] = power - strength
            };
        }

        public override Dictionary<PowerStats, int> InitPower(int power, Step step)
        {
            return DistributePower(power, step);
        }
    }
}