using System.Collections.Generic;
using System.Linq;
using BotCombat.Abstractions;
using BotCombat.Abstractions.BotModels;
using BotCombat.Core.Models;
using Log = BotCombat.Core.Models.Log;

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


        public static IList<Object> ToMapObjectModels(this IEnumerable<IMapObject> mapObjects)
        {
            return mapObjects.Select(ToMapObjectModel)
                .ToList();
        }

        public static Object ToMapObjectModel(this IMapObject mapObject)
        {
            return new Object(mapObject.X, mapObject.Y);
        }

        public static IDictionary<int, Bot> ToMapBotModels(this IEnumerable<BotContainer> botContainers)
        {
            return botContainers
                .Select(ToMapBotModel)
                .ToDictionary(b => b.Id, b => b);
        }

        public static Bot ToMapBotModel(this BotContainer botContainer)
        {
            return new Bot(botContainer.Id,
                botContainer.X,
                botContainer.Y,
                botContainer.Stamina,
                botContainer.Strength);
        }

        public static IList<Abstractions.BotModels.Log> ToLogModels(this IEnumerable<Log> logs)
        {
            return logs
                .Select(dl => new Abstractions.BotModels.Log(dl.X, dl.Y, dl.SourceId, dl.TargetId, dl.Value))
                .ToList();
        }
    }
}