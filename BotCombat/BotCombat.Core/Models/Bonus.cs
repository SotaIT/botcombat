namespace BotCombat.Core.Models
{
    public class Bonus : MapObject
    {
        public Bonus(int id, int x, int y, int power) : base(x, y)
        {
            Id = id;
            Power = power;
        }

        public int Id { get; }

        public int Power { get; }
    }
}