namespace BotCombat.Core.Models
{
    public class Map
    {
        public Map(int id, int width, int height)
        {
            Id = id;
            Width = width;
            Height = height;
        }

        public int Id { get; }

        public int Width { get; }

        public int Height { get; }
    }
}
