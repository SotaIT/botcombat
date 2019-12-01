namespace BotCombat.BotWorld
{
    public class Coordinates
    {
        public Coordinates(int x, int y, bool isWall = false, bool isOut = false)
        {
            X = x;
            Y = y;
            IsWall = isWall;
            IsOut = isOut;
        }

        public int X { get; }
        public int Y { get; }
        public bool IsWall { get; }
        public bool IsOut { get; }
        public bool IsRestricted => IsOut || IsWall;
    }
}