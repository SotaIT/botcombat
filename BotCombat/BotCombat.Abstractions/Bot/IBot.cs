using System.Collections.Generic;
using BotCombat.Abstractions.Models;

namespace BotCombat.Abstractions
{
    public interface IBot
    {
        /// <summary>
        /// Bot Id
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Distribute power points among stats
        /// </summary>
        Dictionary<PowerStats, int> DistributePower(int power, Game game);

        /// <summary>
        /// Initial distribution of the power points among stats
        /// </summary>
        Dictionary<PowerStats, int> InitPower(int power, Game game);

        /// <summary>
        /// Choose the best direction to move
        /// </summary>
        MoveDirection ChooseDirection(Game game);
    }
}