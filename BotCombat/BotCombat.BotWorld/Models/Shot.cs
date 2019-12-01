namespace BotCombat.BotWorld
{
    public class Shot: MapObject, IDirection
    {
        public Shot(int id, int x, int y, int direction) : base(id, x, y)
        {
            Direction = direction;
        }

        public int Direction { get; }
    }
}