namespace BotCombat.Web.Data.Domain
{
    public class Author: IEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }
    }
}