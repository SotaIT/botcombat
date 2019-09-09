using BotCombat.Abstractions;

namespace BotCombat.Core
{
    public abstract class MapObject : IMapObject
    {
        protected MapObject(int x, int y, MapImage mapImage)
        {
            X = x;
            Y = y;
            MapImage = mapImage;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public MapImage MapImage { get; }

        public void Move(MoveDirection direction)
        {
            var point = MapUtils.GetDestination(X, Y, direction);
            X = point.X;
            Y = point.Y;
        }
    }
}