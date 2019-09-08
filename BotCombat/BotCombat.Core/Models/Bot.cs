namespace BotCombat.Core.Models
{
    public class Bot : Object
    {
        public Bot(int id, int x, int y, int health, int damage) : base(x, y)
        {
            Id = id;
            Health = health;
            Damage = damage;
        }

        public int Id { get; }

        public int Health { get; }

        public int Damage { get; }
    }
}
