namespace BotCombat.Core
{
    public class Wall : MapObject
    {
        public Wall(int x, int y, MapImage mapImage, int strength) : base(x, y, mapImage)
        {
            Strength = strength;
        }

        public int Strength { get; }
    }
}
