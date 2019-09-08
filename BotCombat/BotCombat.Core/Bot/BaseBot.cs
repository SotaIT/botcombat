using System;
using System.Collections.Generic;
using System.Linq;

namespace BotCombat.Core
{
    public abstract class BaseBot : IBot
    {
        private readonly PowerStats[] stats = Enum.GetValues(typeof(PowerStats)).Cast<PowerStats>().ToArray();

        public abstract MapImage BotImage { get; }

        public abstract int Id { get; }

        /// <inheritdoc />
        public abstract MoveDirection ChooseDirection(Step step);

        /// <inheritdoc />
        public virtual Dictionary<PowerStats, int> InitPower(int power, Step step)
        {
            return DistributePower(power, step);
        }

        /// <inheritdoc />
        public virtual Dictionary<PowerStats, int> DistributePower(int power, Step step)
        {
            var delta = power % stats.Length;
            var part = power / stats.Length;

            // equal amount of power to each stat
            var dict = stats.ToDictionary(s => s, s => part);

            // the rest of the power we give to Stamina
            dict[PowerStats.Stamina] += delta;

            return dict;
        }
    }
}
