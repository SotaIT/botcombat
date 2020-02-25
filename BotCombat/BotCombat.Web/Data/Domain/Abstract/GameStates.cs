namespace BotCombat.Web.Data.Domain
{
    public enum GameStates
    {
        /// <summary>
        /// The game is created and waiting to be added to queue
        /// </summary>
        Created = 0,

        /// <summary>
        /// The game is queued and waiting to be played
        /// </summary>
        Queued = 1,

        /// <summary>
        /// The game has been played
        /// </summary>
        Played = 2,

        /// <summary>
        /// THere was an error when playing the game
        /// </summary>
        Error = 3,

        /// <summary>
        /// The game is played and is available in the catalog
        /// </summary>
        Published = 4
    }
}