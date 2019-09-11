namespace BotCombat.Abstractions
{
    public class PowerStats
    {
        public int Stamina { get; set; }

        public int Strength { get; set; }

        public int Power => Stamina + Strength;
    }
}