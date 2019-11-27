namespace BotCombat.BotWorld
{
    public class Trap : MapObject, IDamager
    {
        public int Damage { get; }

        public Trap(int id, int x, int y, int damage) : base(id, x, y)
        {
            Damage = damage;
        }
    }
}