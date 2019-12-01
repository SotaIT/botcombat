using System.Collections.Generic;

namespace BotCombat.Web
{
    internal class StepJsonModel
    {
        /// <summary>
        /// Number
        /// </summary>
        public int N { get; set; }

        /// <summary>
        /// Bonuses
        /// </summary>
        public IEnumerable<MapObjectJsonModel> Bs { get; set; }

        /// <summary>
        /// Bots
        /// </summary>
        public IEnumerable<BotJsonModel> Bt { get; set; }

        /// <summary>
        /// Bullets
        /// </summary>
        public IEnumerable<BulletJsonModel> Bl { get; set; }

        /// <summary>
        /// Shots
        /// </summary>
        public IEnumerable<MapObjectJsonModel> Ss { get; set; }

        /// <summary>
        /// Explosions
        /// </summary>
        public IEnumerable<MapObjectJsonModel> Es { get; set; }

        /// <summary>
        /// Logs
        /// </summary>
        public IEnumerable<LogJsonModel> L { get; set; }
    }
}