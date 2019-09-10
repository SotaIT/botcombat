using BotCombat.Abstractions;
using System;
using System.Collections.Generic;
using BotCombat.Abstractions.BotModels;

namespace BotCombat.Block
{
    public class BlockBot : IBot
    {
        public int Id => throw new NotImplementedException();

        public MoveDirection ChooseDirection(Game game)
        {
            throw new NotImplementedException();
        }

        public Dictionary<PowerStats, int> DistributePower(int power, Game game)
        {
            throw new NotImplementedException();
        }

        public Dictionary<PowerStats, int> InitPower(int power, Game game)
        {
            throw new NotImplementedException();
        }
    }
}
