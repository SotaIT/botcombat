namespace BotCombat.Abstractions.BotModels
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