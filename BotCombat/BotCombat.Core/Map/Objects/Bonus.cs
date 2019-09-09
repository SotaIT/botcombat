using BotCombat.Abstractions;

namespace BotCombat.Core
{
    public class Bonus : MapObject
    {
        public Bonus(int x, int y, int power) : base(x, y)
        {
            Power = power;
        }

        public int Power { get; }
    }
}