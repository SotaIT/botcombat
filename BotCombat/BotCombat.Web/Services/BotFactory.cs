using System;
using BotCombat.Abstractions;
using BotCombat.Cs;
using BotCombat.Js;

namespace BotCombat.Web.Services
{
    public static class BotFactory
    {
        public enum BotType
        {
            CSharp = 0,
            Javascript = 1
        }

        public static IBot CreateBot(BotType type, int id, string code = null)
        {
            switch (type)
            {
                case BotType.CSharp:
                    return new CsBot(id, code);
                case BotType.Javascript:
                    return new JsBot(id, code);
            }

            throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
