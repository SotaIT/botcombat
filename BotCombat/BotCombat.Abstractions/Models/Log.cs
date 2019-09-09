namespace BotCombat.Abstractions.Models
{
    public class Log
    {
        public Log(int x, int y, int sourceId, int targetId, int damage)
        {
            X = x;
            Y = y;
            SourceId = sourceId;
            TargetId = targetId;
            Damage = damage;
        }

        public int X { get; }

        public int Y { get; }

        public int SourceId { get; }

        public int TargetId { get; }

        public int Damage { get; }
    }
}