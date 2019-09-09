using System.Collections.Generic;
using BotCombat.Abstractions.Models;

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
        public abstract Dictionary<PowerStats, int> DistributePower(int power, Game game);
        public abstract Dictionary<PowerStats, int> InitPower(int power, Game game);
    }
}