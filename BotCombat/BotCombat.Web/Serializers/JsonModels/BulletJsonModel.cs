namespace BotCombat.Web
{
    internal class BulletJsonModel : MapObjectJsonModel
    {
        /// <summary>
        /// Number
        /// </summary>
        public int N { get; set; }

        /// <summary>
        /// Damage
        /// </summary>
        public int D { get; set; }

        /// <summary>
        /// Direction
        /// </summary>
        public int Dr { get; set; }

        /// <summary>
        /// Exploded
        /// </summary>
        public int? E { get; set; }
    }
}