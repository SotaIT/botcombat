using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BotCombat.Abstractions.BotModels
{
    public class Map
    {
        public Map(int id, int width, int height, IList<Object> walls, IList<Object> traps)
        {
            Id = id;
            Width = width;
            Height = height;
            Walls = new ReadOnlyCollection<Object>(walls);
            Traps = new ReadOnlyCollection<Object>(traps);
        }

        public int Id { get; }

        public int Width { get; }

        public int Height { get; }

        public IReadOnlyList<Object> Walls { get; }

        public IReadOnlyList<Object> Traps { get; }
    }
}