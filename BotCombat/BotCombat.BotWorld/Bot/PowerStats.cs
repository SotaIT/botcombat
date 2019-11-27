using System;

namespace BotCombat.BotWorld
{
    /// <summary>
    /// Bot stats
    /// </summary>
    public class PowerStats
    {
        private int _stamina;
        private int _strength;
        private int _ranged;

        /// <summary>
        /// Increases the bot health
        /// </summary>
        public int Stamina
        {
            get => _stamina;
            set
            {
                CheckValue(value);
                _stamina = value;
            }
        }

        /// <summary>
        /// Increases the bot damage done
        /// </summary>
        public int Strength
        {
            get => _strength;
            set
            {
                CheckValue(value);
                _strength = value;
            }
        }

        /// <summary>
        /// Increases the bot ranged damage done
        /// </summary>
        public int Ranged
        {
            get => _ranged;
            set
            {
                CheckValue(value);
                _ranged = value;
            }
        }

        /// <summary>
        /// The sum of all stats
        /// </summary>
        public int Power => Stamina + Strength + Ranged;

        /// <summary>
        /// Validates stat value
        /// </summary>
        private static void CheckValue(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}