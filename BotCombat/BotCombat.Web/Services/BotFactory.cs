using System;
using BotCombat.BotWorld;
using BotCombat.BuiltInBots;
using BotCombat.JsBots;
using BotCombat.Web.Data.Domain;

namespace BotCombat.Web.Services
{
    public static class BotFactory
    {
        public static IBot CreateBot(BotTypes type, int id, string code = null)
        {
            switch (type)
            {
                case BotTypes.Builtin:
                    return BuiltinBotFactory.GetBot(id, code);
                case BotTypes.Javascript:
                    return new JsBot(id, code);
            }

            throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
