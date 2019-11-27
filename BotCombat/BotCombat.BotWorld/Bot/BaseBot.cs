namespace BotCombat.BotWorld
{
    /// <summary>
    /// A base class to create a bot
    /// </summary>
    public abstract class BaseBot : IBot
    {
        protected BaseBot(int id)
        {
            Id = id;
        }

        protected ActionResult ActionResult(BotAction botAction)
        {
            return new ActionResult { BotAction = botAction };
        }

        protected ActionResult ActionResult(int botAction)
        {
            return ActionResult((BotAction)botAction);
        }

        protected StatsResult StatsResult(int ranged, int strength, int stamina)
        {
            return new StatsResult
            {
                Ranged = ranged,
                Strength = strength,
                Stamina = stamina
            };
        }

        /// <inheritdoc />
        public virtual int Id { get; }

        /// <inheritdoc />
        public abstract ActionResult ChooseAction(Game game);

        /// <inheritdoc />
        public abstract StatsResult DistributePower(Game game, int power);
    }
}