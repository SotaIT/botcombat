using System;
using BotCombat.BotWorld;

namespace BotCombat.BuiltInBots
{
    /// <summary>
    /// Random moving bot
    /// </summary>
    public class RandomBot : BaseBot
    {
        public RandomBot(int id) : base(id)
        {
        }

        /// <summary>
        /// Chooses random direction
        /// </summary>
        public override ActionResult ChooseAction(Game game)
        {
            return ActionResult(new Random().Next(5) + 1);
        }

        public override StatsResult DistributePower(Game game, int power)
        {
            var ranged = power / 3;
            var strength = (power - ranged) / 2;
            return StatsResult(ranged, strength, power - strength - ranged);
        }
    }
}
