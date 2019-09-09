namespace BotCombat.Core
{
    public class Log
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
        public int SourceId { get; }
        public int TargetId { get; }
        public int Value { get; }
    }
}