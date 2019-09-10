using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BotCombat.Abstractions.BotModels
{
    public class Game
    {
        public Game(Map map, IList<Step> steps)
        {
            Map = map;
            Steps = new ReadOnlyCollection<Step>(steps);
        }

        public Map Map { get; }

        public IReadOnlyList<Step> Steps { get; }

        public Step LastStep => Steps[Steps.Count - 1];
    }
}