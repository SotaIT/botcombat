namespace BotCombat.Web.Data.Domain
{
    public class Image : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
    }
}
