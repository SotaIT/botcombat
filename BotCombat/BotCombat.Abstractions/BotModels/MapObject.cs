namespace BotCombat.Abstractions.BotModels
{
    public abstract class MapObject
    {
        protected MapObject(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }
    }
}