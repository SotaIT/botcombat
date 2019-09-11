using BotCombat.Abstractions.BotModels;

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
        PowerStats DistributePower(int power, Game game);

        /// <summary>
        /// Choose the best direction to move
        /// </summary>
        MoveDirection ChooseDirection(Game game);
    }
}