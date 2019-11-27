namespace BotCombat.BotWorld
{
    /// <summary>
    /// The interface for all bots
    /// </summary>
    public interface IBot
    {
        /// <summary>
        /// Bot Id
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Distribute power points among stats
        /// </summary>
        StatsResult DistributePower(Game game, int power);

        /// <summary>
        /// Choose the action to do
        /// </summary>
        ActionResult ChooseAction(Game game);
    }
}