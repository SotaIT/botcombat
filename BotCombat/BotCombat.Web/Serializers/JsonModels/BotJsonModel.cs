namespace BotCombat.Web
{
    internal class BotJsonModel : MapObjectJsonModel
    {
        /// <summary>
        /// Health
        /// </summary>
        public int H { get; set; }

        /// <summary>
        /// Damage
        /// </summary>
        public int D { get; set; }

        /// <summary>
        /// Direction
        /// </summary>
        public int Dr { get; set; }

        /// <summary>
        /// Error
        /// </summary>
        public string E { get; set; }

        /// <summary>
        /// Damaged
        /// </summary>
        public int? Dd { get; set; }
    }
}