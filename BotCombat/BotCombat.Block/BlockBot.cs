using BotCombat.Abstractions;
using System;
using System.Collections.Generic;

namespace BotCombat.Block
{
    public class BlockBot : IBot
    {
        public int Id => throw new NotImplementedException();

        public MoveDirection ChooseDirection(Step step)
        {
            throw new NotImplementedException();
        }

        public Dictionary<PowerStats, int> DistributePower(int power, Step step)
        {
            throw new NotImplementedException();
        }

        public Dictionary<PowerStats, int> InitPower(int power, Step step)
        {
            throw new NotImplementedException();
        }
    }
}
