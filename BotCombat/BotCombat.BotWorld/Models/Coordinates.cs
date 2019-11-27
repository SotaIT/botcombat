namespace BotCombat.BotWorld
{
    public class Coordinates
    {
        public Coordinates(int x, int y, bool isWall = false)
        {
            X = x;
            Y = y;
            IsWall = isWall;
        }

        public int X { get; }
        public int Y { get; }
        public bool IsWall { get; }
    }
}