using BotCombat.Abstractions.BotModels;

namespace BotCombat.Abstractions
{
    public abstract class BaseBot : IBot
    {
        protected BaseBot(int id)
        {
            Id = id;
        }

        public virtual int Id { get; }

        public abstract MoveDirection ChooseDirection(Game game);
        public abstract PowerStats DistributePower(int power, Game game);
    }
}