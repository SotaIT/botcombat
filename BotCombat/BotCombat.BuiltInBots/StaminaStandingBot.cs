using BotCombat.BotWorld;

namespace BotCombat.BuiltInBots
{
    /// <summary>
    /// This bot just stays at its starting point and stacks stamina
    /// </summary>
    public class TowerBot : BaseBot
    {
        public TowerBot(int id) : base(id)
        {
        }

        public override ActionResult ChooseAction(Game game)
        {
            return ActionResult(BotAction.Stop);
        }

        public override StatsResult DistributePower(Game game, int power)
        {
            return StatsResult(1, 1, power - 2);
        }
    }
}