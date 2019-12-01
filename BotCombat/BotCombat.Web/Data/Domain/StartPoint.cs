namespace BotCombat.Web.Data.Domain
{
    public class StartPoint: IEntity, IMapObject, ICoordinates
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int? BotId { get; set; }
    }
}