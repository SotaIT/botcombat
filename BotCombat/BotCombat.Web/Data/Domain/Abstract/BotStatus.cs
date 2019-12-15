namespace BotCombat.Web.Data.Domain
{
    public enum BotStatus
    {
        /// <summary>
        /// Can be run only from edit mode
        /// </summary>
        Draft = 0,

        /// <summary>
        /// Can run anywhere but only by the bot author
        /// </summary>
        Private = 1,

        /// <summary>
        /// Anyone can choose and run this bot
        /// </summary>
        Public = 2,

        /// <summary>
        /// Public + the source code can is open to anyone
        /// </summary>
        Open = 3,

        /// <summary>
        /// The bot is deleted and can't be accessed by anyone
        /// </summary>
        Deleted = 4
    }
}