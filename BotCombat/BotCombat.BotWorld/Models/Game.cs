using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BotCombat.BotWorld
{
    /// <summary>
    /// The Game is the master object, containing the map and all of it contents, and the steps done
    /// </summary>
    public class Game
    {
        public Game(Map map, IList<Step> steps)
        {
            Map = map;
            Steps = new ReadOnlyCollection<Step>(steps);
        }

        /// <summary>
        /// Map
        /// </summary>
        public Map Map { get; }

        /// <summary>
        /// Steps done by this time, in correct order
        /// </summary>
        public IReadOnlyList<Step> Steps { get; }
    }
}