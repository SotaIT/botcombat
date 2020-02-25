using BotCombat.BotWorld;

namespace BotCombat.Engine.Contracts
{
    public class GameResult
    {
        public Game Game { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }

    }
}