using BotCombat.BotWorld;

namespace BotCombat.Core
{
    public class BulletManager : IDamager
    {
        public BulletManager(int id, int number, int x, int y, int damage, BotAction direction)
        {
            Damage = damage;
            Direction = direction;
            X = x;
            Y = y;
            Id = id;
            Number = number;
        }

        public int Id { get; }
        public int Number { get; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Damage { get; }
        public BotAction Direction { get; }
        public bool Exploded { get; private set; }

        public void Move(Game game)
        {
            var point = game.Map.GetDestination(Direction, this);

            if (point.IsRestricted)
            {
                Explode();
                return;
            }

            X = point.X;
            Y = point.Y;
        }

        public void Explode()
        {
            Exploded = true;
        }

        public Bullet ToBullet()
        {
            return new Bullet(Id, Number, X, Y, Damage, (int)Direction, Exploded);
        }

        public IMapObject CreateShot()
        {
            return new Shot(Id, X, Y, (int)Direction);
        }

        public IMapObject CreateExplosion()
        {
            return new Explosion(Id, X, Y);
        }
    }
}