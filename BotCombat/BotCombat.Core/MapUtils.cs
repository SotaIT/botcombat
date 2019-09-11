using System.Collections.Generic;
using System.Linq;
using BotCombat.Abstractions;
using BotCombat.Abstractions.BotModels;
using BotCombat.Core.Models;
using Bonus = BotCombat.Abstractions.BotModels.Bonus;
using Log = BotCombat.Abstractions.BotModels.Log;
using Trap = BotCombat.Abstractions.BotModels.Trap;
using Wall = BotCombat.Abstractions.BotModels.Wall;

namespace BotCombat.Core
{
    internal static class MapUtils
    {
        public static Coordinates GetDestination(int x, int y, MoveDirection direction)
        {
            var point = new Coordinates {X = x, Y = y};

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


        public static IList<Bonus> ToBotModel(this IEnumerable<Models.Bonus> mapObjects)
        {
            return mapObjects.Select(ToBotModel)
                .ToList();
        }

        public static Bonus ToBotModel(this Models.Bonus mapObject)
        {
            return new Bonus(mapObject.Id, mapObject.X, mapObject.Y, mapObject.Power);
        }

        public static IList<Wall> ToBotModel(this IEnumerable<Models.Wall> mapObjects)
        {
            return mapObjects.Select(ToBotModel)
                .ToList();
        }

        public static Wall ToBotModel(this Models.Wall mapObject)
        {
            return new Wall(mapObject.X, mapObject.Y);
        }

        public static IList<Trap> ToBotModel(this IEnumerable<Models.Trap> mapObjects)
        {
            return mapObjects.Select(ToBotModel).ToList();
        }

        public static Trap ToBotModel(this Models.Trap mapObject)
        {
            return new Trap(mapObject.X, mapObject.Y, mapObject.Damage);
        }

        public static IDictionary<int, Bot> ToBotModel(this IEnumerable<BotContainer> botContainers)
        {
            return botContainers
                .Select(ToBotModel)
                .ToDictionary(b => b.Id, b => b);
        }

        public static Bot ToBotModel(this BotContainer botContainer)
        {
            return new Bot(botContainer.Id,
                botContainer.X,
                botContainer.Y,
                botContainer.Stamina,
                botContainer.Strength);
        }

        public static IList<Log> ToBotModel(this IEnumerable<Models.Log> logs)
        {
            return logs
                .Select(dl => new Log(dl.X, dl.Y, dl.SourceId, dl.TargetId, dl.Value))
                .ToList();
        }
    }
}