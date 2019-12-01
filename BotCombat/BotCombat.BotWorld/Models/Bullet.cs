namespace BotCombat.BotWorld
{
    public class Bullet: MapObject, IDamager, IDirection
    {
        public Bullet(int id, int number, int x, int y, int damage, int direction, bool exploded) : base(id, x, y)
        {
            Number = number;
            Damage = damage;
            Direction = direction;
            Exploded = exploded;
        }

        public int Number { get; }
        public int Damage { get; }
        public int Direction { get; }
        public bool Exploded { get; }
    }
}