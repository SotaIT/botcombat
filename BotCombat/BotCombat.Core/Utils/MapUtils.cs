using BotCombat.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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


        public static IList<BotCombat.Abstractions.Models.Object> ToMapObjectModels(this IEnumerable<IMapObject> mapObjects)
        {
            return mapObjects.Select(ToMapObjectModel)
                .ToList();
        }

        public static BotCombat.Abstractions.Models.Object ToMapObjectModel(this IMapObject mapObject)
        {
            return new BotCombat.Abstractions.Models.Object(mapObject.X, mapObject.Y);
        }

        public static IDictionary<int, BotCombat.Abstractions.Models.Bot> ToMapBotModels(this IEnumerable<BotContainer> botContainers)
        {
            return botContainers
                .Select(ToMapBotModel)
                .ToDictionary(b => b.Id, b => b);
        }

        public static BotCombat.Abstractions.Models.Bot ToMapBotModel(this BotContainer botContainer)
        {
            return new BotCombat.Abstractions.Models.Bot(botContainer.Id,
                botContainer.X,
                botContainer.Y,
                botContainer.Stamina,
                botContainer.Strength);
        }

        public static IList<BotCombat.Abstractions.Models.DamageLog> ToDamageLogModels(this IEnumerable<DamageLog> damageLogs)
        {
            return damageLogs
                .Select(dl => new BotCombat.Abstractions.Models.DamageLog(dl.X, dl.Y, dl.SourceId, dl.TargetId, dl.Damage))
                .ToList();
        }
    }
}
