namespace BotCombat.Abstractions.Models
{
    public class Object
    {
        public Object(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }
    }
}
