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


        public Coordinates GetDestination(int direction, int x, int y)
        {
            return GetDestination((BotAction)direction, x, y);
        }

        public Coordinates GetDestination(BotAction direction, IMapObject mapObject)
        {
            return GetDestination(direction, mapObject.X, mapObject.Y);
        }

        public Coordinates GetDestination(BotAction direction, int x, int y)
        {
            switch (direction)
            {
                case BotAction.MoveUp:
                    y--;
                    break;
                case BotAction.MoveRight:
                    x++;
                    break;
                case BotAction.MoveDown:
                    y++;
                    break;
                case BotAction.MoveLeft:
                    x--;
                    break;
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