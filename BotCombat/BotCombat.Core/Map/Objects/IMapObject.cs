using BotCombat.Abstractions;

namespace BotCombat.Core
{
    public interface IMapObject
    {
        MapImage MapImage { get; }
        int X { get; }
        int Y { get; }

        void Move(MoveDirection direction);
    }
}