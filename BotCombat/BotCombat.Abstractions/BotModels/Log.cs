namespace BotCombat.Abstractions.BotModels
{
    public class Log : MapObject
    {
        public Log(int x, int y, int sourceId, int targetId, int value) : base(x, y)
        {
            SourceId = sourceId;
            TargetId = targetId;
            Value = value;
        }

        public int SourceId { get; }

        public int TargetId { get; }

        public int Value { get; }
    }
}