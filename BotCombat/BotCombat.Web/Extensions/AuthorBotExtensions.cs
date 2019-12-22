using BotCombat.Web.Data.Domain;

namespace BotCombat.Web
{
    public static class AuthorBotExtensions
    {
        public static int GetRootId(this AuthorBot authorBot)
        {
            return authorBot.RootId ?? authorBot.Id;
        }
    }
}