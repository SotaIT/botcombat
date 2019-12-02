using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BotCombat.BotWorld
{
    /// <summary>
    /// The Map
    /// </summary>
    public class Map
    {
        public Map(int id, int width, int height, IList<Wall> walls, IList<Trap> traps)
        {
            Id = id;
            Width = width;
            Height = height;
            Walls = new ReadOnlyCollection<Wall>(walls);
            Traps = new ReadOnlyCollection<Trap>(traps);
        }

        public int Id { get; }

        public int Width { get; }

        public int Height { get; }

        public IReadOnlyList<Wall> Walls { get; }

        public IReadOnlyList<Trap> Traps { get; }


        public Coordinates GetMoveDst(int botAction, int x, int y)
        {
            return GetMoveDestination((BotAction)botAction, x, y);
        }

        public Coordinates GetDst(int direction, int x, int y)
        {
            return GetDestination((Direction)direction, x, y);
        }

        public Coordinates GetMoveDestination(BotAction direction, IMapObject mapObject)
        {
            return GetMoveDestination(direction, mapObject.X, mapObject.Y);
        }

        public Coordinates GetMoveDestination(BotAction botAction, int x, int y)
        {
            switch (botAction)
            {
                case BotAction.MoveUp:
                case BotAction.MoveRight:
                case BotAction.MoveDown:
                case BotAction.MoveLeft:
                    break;
                default:
                    return new Coordinates(x, y);
            }

            return GetDestination(botAction.ToDirection(), x, y);
        }

        public Coordinates GetDestination(Direction direction, IMapObject mapObject)
        {
            return GetDestination(direction, mapObject.X, mapObject.Y);
        }

        public Coordinates GetDestination(Direction direction, int x, int y)
        {
            switch (direction)
            {
                case Direction.Up:
                    y--;
                    break;
                case Direction.Right:
                    x++;
                    break;
                case Direction.Down:
                    y++;
                    break;
                case Direction.Left:
                    x--;
                    break;
                default:
                    return new Coordinates(x, y);
            }

            // destination is wall
            var isWall = Walls.Any(w => w.X == x && w.Y == y);

            // destination out of map
            var isOut = x >= Width
                        || x < 0
                        || y >= Height
                        || y < 0;

            return new Coordinates(x, y, isWall, isOut);
        }

    }
}