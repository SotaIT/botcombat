namespace BotCombat.Core.Models
{
    internal class Log
    {
        public Log(int step, int x, int y, int sourceId, int targetId, int value)
        {
            Step = step;
            X = x;
            Y = y;
            SourceId = sourceId;
            TargetId = targetId;
            Value = value;
        }

        public int Step { get; }

        public int X { get; }

        public int Y { get; }

        /// <summary>
        /// If = 0 the damage/power is by trap/bonus
        /// </summary>
        public int SourceId { get; }

        public int TargetId { get; }

        /// <summary>
        /// If less than 0 it is damage else - power
        /// </summary>
        public int Value { get; }
    }
}