namespace BotCombat.Web.Data.Domain
{
    public class MapBot: IEntity, IMapObject
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public int BotId { get; set; }
        public int BotImageId { get; set; }
        public int? StartPointId { get; set; }
    }
}