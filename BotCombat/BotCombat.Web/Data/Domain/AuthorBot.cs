using System;

namespace BotCombat.Web.Data.Domain
{
    public class AuthorBot: IEntity
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int BotId { get; set; }
        public int? RootId { get; set; }
        public int? ParentId { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }
        public int? Status { get; set; }
    }
}