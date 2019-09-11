namespace BotCombat.Abstractions.BotModels
{
    public class Bonus: MapObject
    {
        public int Id { get; }

        public int Power { get; }

        public Bonus(int id, int x, int y, int power) : base(x,y)
        {
            Id = id;
            Power = power;
        }
    }
}