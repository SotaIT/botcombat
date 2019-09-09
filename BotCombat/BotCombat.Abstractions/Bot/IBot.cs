using System.Collections.Generic;

namespace BotCombat.Abstractions
{
    public interface IBot
    {
        /// <summary>
        /// Bot image
        /// </summary>
        MapImage BotImage { get; }

        /// <summary>
        /// Bot Id
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Distribute power points among stats
        /// </summary>
        Dictionary<PowerStats, int> DistributePower(int power, Step step);

        /// <summary>
        /// Initial distribution of the power points among stats
        /// </summary>
        Dictionary<PowerStats, int> InitPower(int power, Step step);

        /// <summary>
        /// Choose the best direction to move
        /// </summary>
        MoveDirection ChooseDirection(Step step);
    }
}