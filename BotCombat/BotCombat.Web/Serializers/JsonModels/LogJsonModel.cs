namespace BotCombat.Web
{
    internal class LogJsonModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Step
        /// </summary>
        public int S { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public int T { get; set; }

        /// <summary>
        /// SourceId
        /// </summary>
        public int Si { get; set; }

        /// <summary>
        /// TargetId
        /// </summary>
        public int Ti { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public int V { get; set; }
    }
}