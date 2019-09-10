using BotCombat.Abstractions;
using BotCombat.Cs;
using System.Collections.Generic;
using BotCombat.Abstractions.BotModels;

namespace BotCombat.TestApp
{
    public class TestBot : DefaultCsBot
    {
        public TestBot() : base(1)
        {
        }
    }

    public class TestBot2 : TestBot
    {
        public override int Id => 2;

        public override Dictionary<PowerStats, int> DistributePower(int power, Game game)
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
