namespace BotCombat.BotWorld
{
    /// <summary>
    /// Contains information about what have happened to bots
    /// </summary>
    public class Log
    {
        public Log(int id, LogType type, int step, int x, int y, int sourceId, int targetId, int value, string message = null)
        {
            Id = id;
            Step = step;
            X = x;
            Y = y;
            Type = (int)type;
            SourceId = sourceId;
            TargetId = targetId;
            Value = value;
            Message = message;
        }

        public int Id { get; }

        /// <summary>
        /// The Step number of event
        /// </summary>
        public int Step { get; }

        public int X { get; }
        public int Y { get; }

        /// <summary>
        /// The type of event
        /// </summary>
        public int Type { get; }

        /// <summary>
        /// The source of event
        /// </summary>
        public int SourceId { get; }

        /// <summary>
        /// The target of event
        /// </summary>
        public int TargetId { get; }

        /// <summary>
        /// The Power got / Damage taken
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; }
    }
}