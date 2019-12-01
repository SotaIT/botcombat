namespace BotCombat.Web.Data.Domain
{
    public class BotImage: IEntity, IMapObject, IHasImage
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public int ImageId { get; set; }
        public int BulletImageId { get; set; }
        public int ShotImageId { get; set; }
        public int ExplosionImageId { get; set; }
    }
}