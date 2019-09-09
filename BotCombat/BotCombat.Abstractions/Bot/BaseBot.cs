using System.Collections.Generic;

namespace BotCombat.Abstractions
{
    public abstract class BaseBot : IBot
    {
        protected BaseBot(int id)
        {
            Id = id;
        }

        public virtual int Id { get; }

        public abstract MoveDirection ChooseDirection(Step step);
        public abstract Dictionary<PowerStats, int> DistributePower(int power, Step step);
        public abstract Dictionary<PowerStats, int> InitPower(int power, Step step);
    }
}