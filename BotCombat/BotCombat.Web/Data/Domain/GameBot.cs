namespace BotCombat.Web.Data.Domain
{
    public class GameBot : IEntity
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int BotId { get; set; }

        /// <summary>
        /// Total damage done during the game
        /// </summary>
        public int? DamageDone { get; set; }

        /// <summary>
        /// Total damage taken from bots during the game
        /// </summary>
        public int? DamageTaken { get; set; }

        /// <summary>
        /// Total damage taken from traps during the game
        /// </summary>
        public int? TrapDamageTaken { get; set; }

        /// <summary>
        /// Total bonus power gain during the game
        /// </summary>
        public int? BonusPower { get; set; }

        /// <summary>
        /// Total bonuses captured during the game
        /// </summary>
        public int? Bonuses { get; set; }

        /// <summary>
        /// Total traps caught during the game
        /// </summary>
        public int? Traps { get; set; }

        /// <summary>
        /// The number of the step when the bot was killed
        /// </summary>
        public int? DeathStep { get; set; }

        public void ResetStats()
        {
            DamageDone = 0;
            DamageTaken = 0;
            TrapDamageTaken = 0;
            BonusPower = 0;
            Bonuses = 0;
            Traps = 0;
        }
    }
}