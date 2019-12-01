using BotCombat.BotWorld;

namespace BotCombat.Core
{
    public class StartPoint: Coordinates
    {
        public StartPoint(int x, int y, int? botId = null) : base(x, y)
        {
            BotId = botId;
        }

        public int? BotId { get; }
    }
}