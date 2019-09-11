using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BotCombat.Abstractions.BotModels
{
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
    }
}