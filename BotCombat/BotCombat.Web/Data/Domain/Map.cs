namespace BotCombat.Web.Data.Domain
{
    public class Map : IEntity, IHasImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ImageId { get; set; }
        public int BackgroundImageId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Scale { get; set; }
        public int InitialPower { get; set; }
        public int StrengthWeight { get; set; }
        public int StaminaWeight { get; set; }
        public int? MaxStepCount { get; set; }
        public int BonusSpawnInterval { get; set; }
        public int BulletSpeed { get; set; }
        public int MaxBotCount { get; set; }
        public int RangedWeight { get; set; }
    }
}
