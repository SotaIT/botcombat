namespace BotCombat.Abstractions.BotModels
{
    public class Bot : Object
    {
        public Bot(int id, int x, int y, int stamina, int strength) : base(x, y)
        {
            Id = id;
            Stamina = stamina;
            Strength = strength;
        }

        public int Id { get; }

        public int Stamina { get; }

        public int Strength { get; }
    }
}