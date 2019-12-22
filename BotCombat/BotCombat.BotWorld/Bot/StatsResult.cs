namespace BotCombat.BotWorld
{
    public class StatsResult: BaseResult
    {
        public PowerStats Stats { get; } = new PowerStats();

        public int Stamina
        {
            get => Stats.Stamina;
            set => Stats.Stamina = value;
        }

        public int Strength
        {
            get => Stats.Strength;
            set => Stats.Strength = value;
        }

        public int Ranged
        {
            get => Stats.Ranged;
            set => Stats.Ranged = value;
        }

        public void SetValues(int ranged, int strength, int stamina)
        {
            Ranged = ranged;
            Strength = strength;
            Stamina = stamina;
        }
    }
}