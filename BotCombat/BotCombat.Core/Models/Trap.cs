namespace BotCombat.Core.Models
{
    public class Trap : MapObject
    {
        public Trap(int x, int y, int damage) : base(x, y)
        {
            Damage = damage;
        }

        public int Damage { get; }
    }
}
