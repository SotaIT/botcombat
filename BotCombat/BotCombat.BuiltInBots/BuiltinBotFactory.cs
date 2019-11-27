using System;
using BotCombat.BotWorld;

namespace BotCombat.BuiltInBots
{
    public static class BuiltinBotFactory
    {
        public static IBot GetBot(int id, string name)
        {
            switch ((BuiltinBots)Enum.Parse(typeof(BuiltinBots), name))
            {
                case BuiltinBots.RandomBot:
                    return new RandomBot(id);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}