namespace BotCombat.Web.Data.Domain
{
    public class Map
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BackgroundImageId { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Scale { get; set; }

        public int InitialPower { get; set; }

        public int StrengthWeight { get; set; }

        public int StaminaWeight { get; set; }
    }
}
