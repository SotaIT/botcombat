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


        public static Models.Map ToMapModel(this MapSettings mapSettings)
        {
            return new Models.Map(mapSettings.Id, mapSettings.Width, mapSettings.Height);
        }

        public static IReadOnlyList<Models.Object> ToMapObjectModels(this IEnumerable<IMapObject> mapObjects)
        {
            return new ReadOnlyCollection<Models.Object>(
                mapObjects.Select(ToMapObjectModel)
                .ToList());
        }

        public static Models.Object ToMapObjectModel(this IMapObject mapObject)
        {
            return new Models.Object(mapObject.X, mapObject.Y);
        }

        public static IReadOnlyDictionary<int, Models.Bot> ToMapBotModels(this IEnumerable<BotContainer> botContainers)
        {
            return new ReadOnlyDictionary<int, Models.Bot>(
                botContainers
                .Select(ToMapBotModel)
                .ToDictionary(b => b.Id, b => b));
        }

        public static Models.Bot ToMapBotModel(this BotContainer botContainer)
        {
            return new Models.Bot(botContainer.Id,
                botContainer.X,
                botContainer.Y,
                botContainer.Stamina,
                botContainer.Strength);
        }

        public static IReadOnlyList<DamageLog> ToDamageLogModels(this IEnumerable<DamageLog> damageLogs)
        {
            return new ReadOnlyCollection<DamageLog>(damageLogs.ToList());
        }
    }
}
