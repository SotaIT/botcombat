using BotCombat.Abstractions;

namespace BotCombat.Core.Models
{
    public abstract class MapObject : IMapObject
    {
        protected MapObject(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public void Move(MoveDirection direction)
        {
            var point = MapUtils.GetDestination(X, Y, direction);
            X = point.X;
            Y = point.Y;
        }
    }
}