using BotCombat.Abstractions;
using System;
using System.Collections.Generic;

namespace BotCombat.Block
{
    public class BlockBot : IBot
    {
        public MapImage BotImage => throw new NotImplementedException();

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
