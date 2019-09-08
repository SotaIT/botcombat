namespace BotCombat.Core
{
    public class DamageLog
    {
        public DamageLog(int step, int x, int y, int sourceId, int targetId, int damage)
        {
            Step = step;
            X = x;
            Y = y;
            SourceId = sourceId;
            TargetId = targetId;
            Damage = damage;
        }

        public int Step { get; }
        public int X { get; }
        public int Y { get; }
        public int SourceId { get; }
        public int TargetId { get; }
        public int Damage { get; }
    }
}
