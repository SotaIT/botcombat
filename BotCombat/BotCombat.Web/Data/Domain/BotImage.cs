namespace BotCombat.Web.Data.Domain
{
    public class BotImage: IEntity, IMapObject, IHasImage
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public int ImageId { get; set; }
    }
}