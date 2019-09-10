namespace BotCombat.Core.Models
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