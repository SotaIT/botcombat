using BotCombat.Core;
using System;
using System.Collections.Generic;

namespace BotCombat.TestApp
{
    public class TestBot : BaseBot
    {
        static Random random = new Random();
        public override MapImage BotImage => new MapImage();

        public override int Id => 1;

        public override MoveDirection ChooseDirection(Step step)
        {
            return (MoveDirection)(random.Next(4) + 1);
        }
    }

    public class TestBot2 : TestBot
    {
        public override int Id => 2;

        public override Dictionary<PowerStats, int> DistributePower(int power, Step step)
        {
            var strength = power / 3;

            return new Dictionary<PowerStats, int>
            {
                [PowerStats.Strength] = strength,
                [PowerStats.Stamina] = power - strength
            };
        }
    }
}
