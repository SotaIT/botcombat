using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BotCombat.Abstractions.BotModels
{
    /// <summary>
    /// The model of the current step
    /// </summary>
    public class Step
    {
        public Step(int number,
            IList<Object> bonuses,
            IDictionary<int, Bot> bots,
            IList<Log> logs,
            IList<int> deadBots)
        {
            Number = number;
            Bonuses = new ReadOnlyCollection<Object>(bonuses);
            Bots = new ReadOnlyDictionary<int, Bot>(bots);
            Logs = new ReadOnlyCollection<Log>(logs);
            DeadBots = new ReadOnlyCollection<int>(deadBots);
        }

        public int Number { get; }

        public IReadOnlyList<Object> Bonuses { get; }

        public IReadOnlyDictionary<int, Bot> Bots { get; }

        public IReadOnlyList<Log> Logs { get; }

        public IReadOnlyList<int> DeadBots { get; }
    }
}