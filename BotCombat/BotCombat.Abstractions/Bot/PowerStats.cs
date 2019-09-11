using System;

namespace BotCombat.Abstractions
{
    public class PowerStats
    {
        private int _stamina;
        private int _strength;

        public int Stamina
        {
            get => _stamina;
            set
            {
                CheckValue(value);
                _stamina = value;
            }
        }

        public int Strength
        {
            get => _strength;
            set
            {
                CheckValue(value);
                _strength = value;
            }
        }

        public int Power => Stamina + Strength;

        private static void CheckValue(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}