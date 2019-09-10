namespace BotCombat.Web.Services
{
    public static class ServiceFactory
    {
        public static BotService BotService { get; } = new BotService();

        public static GameService GameService { get; } = new GameService();

        public static ImageService ImageService { get; } = new ImageService();

        public static MapService MapService { get; } = new MapService();
    }
}
