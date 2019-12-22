using System.Collections.Generic;

namespace BotCombat.BotWorld
{
    public abstract class BaseResult
    {
        private readonly List<string> _debugMessages = new List<string>();

        public void Debug(string message)
        {
            _debugMessages.Add(message);
        }

        public string[] GetDebugMessages()
        {
            return _debugMessages.ToArray();
        }
    }
}