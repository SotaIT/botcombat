using BotCombat.Abstractions;

namespace BotCombat.Core
{
    public class Bonus : MapObject
    {
        public Bonus(int x, int y, int power, MapImage mapImage) : base(x, y, mapImage)
        {
            Power = power;
        }

        public int Power { get; }
    }
}