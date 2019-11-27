using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BotCombat.BotWorld
{
    /// <summary>
    /// The model of the current step
    /// </summary>
    public class Step
    {
        public Step(int number,
            IList<Bonus> bonuses,
            IDictionary<int, Bot> bots, 
            IList<Bullet> bullets,
            IList<Log> logs)
        {
            Number = number;
            Bonuses = new ReadOnlyCollection<Bonus>(bonuses);
            Bots = new ReadOnlyDictionary<int, Bot>(bots);
            Bullets = new ReadOnlyCollection<Bullet>(bullets);
            Logs = new ReadOnlyCollection<Log>(logs);
        }

        public int Number { get; }
        public IReadOnlyList<Bonus> Bonuses { get; }
        public IReadOnlyDictionary<int, Bot> Bots { get; }
        public IReadOnlyList<Bullet> Bullets { get; }
        public IReadOnlyList<Log> Logs { get; }
    }
}