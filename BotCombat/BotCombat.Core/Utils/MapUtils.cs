using System.Collections.Generic;
using System.Linq;
using BotCombat.Abstractions;

namespace BotCombat.Core
{
    internal static class MapUtils
    {
        public static Coordinates GetDestination(int x, int y, MoveDirection direction)
        {
            var point = new Coordinates { X = x, Y = y };

            switch (direction)
            {
                case MoveDirection.Up:
                    point.Y--;
                    break;
                case MoveDirection.Right:
                    point.X++;
                    break;
                case MoveDirection.Down:
                    point.Y++;
                    break;
                case MoveDirection.Left:
                    point.X--;
                    break;
            }

            return point;
        }


        public static IList<Abstractions.Models.Object> ToMapObjectModels(this IEnumerable<IMapObject> mapObjects)
        {
            return mapObjects.Select(ToMapObjectModel)
                .ToList();
        }

        public static Abstractions.Models.Object ToMapObjectModel(this IMapObject mapObject)
        {
            return new Abstractions.Models.Object(mapObject.X, mapObject.Y);
        }

        public static IDictionary<int, Abstractions.Models.Bot> ToMapBotModels(this IEnumerable<BotContainer> botContainers)
        {
            return botContainers
                .Select(ToMapBotModel)
                .ToDictionary(b => b.Id, b => b);
        }

        public static Abstractions.Models.Bot ToMapBotModel(this BotContainer botContainer)
        {
            return new Abstractions.Models.Bot(botContainer.Id,
                botContainer.X,
                botContainer.Y,
                botContainer.Stamina,
                botContainer.Strength);
        }

        public static IList<Abstractions.Models.Log> ToLogModels(this IEnumerable<Log> logs)
        {
            return logs
                .Select(dl => new Abstractions.Models.Log(dl.X, dl.Y, dl.SourceId, dl.TargetId, dl.Value))
                .ToList();
        }
    }
}