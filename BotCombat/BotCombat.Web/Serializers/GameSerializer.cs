using System.Linq;
using BotCombat.BotWorld;
using BotCombat.Web.Models;
using Newtonsoft.Json;

namespace BotCombat.Web
{
    internal static class GameSerializer
    {
        internal static string ToJson(GameViewModel viewModel)
        {
            var json = JsonConvert.SerializeObject(viewModel.ToJsonModel(), new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore
            });

            return json;
        }

        private static GameRootJsonModel ToJsonModel(this GameViewModel viewModel)
        {
            return new GameRootJsonModel
            {
                Game = viewModel.Game.ToJsonModel(),
                Background = viewModel.BackgroundImageId,
                Scale = viewModel.Scale,
                BulletSpeed = viewModel.BulletSpeed,
                WallImages = viewModel.WallImages,
                TrapImages = viewModel.TrapImages,
                BonusImages = viewModel.BonusImages,
                BulletImages = viewModel.BulletImages,
                ShotImages = viewModel.ShotImages,
                ExplosionImages = viewModel.ExplosionImages,
                BotImages = viewModel.BotImages,
                Images = viewModel.Images,
                BotNames = viewModel.BotNames
            };
        }

        private static GameJsonModel ToJsonModel(this Game game)
        {
            return new GameJsonModel
            {
                Map = game.Map.ToJsonModel(),
                Steps = game.Steps.Select(ToJsonModel)
            };
        }

        private static MapJsonModel ToJsonModel(this Map map)
        {
            return new MapJsonModel
            {
                Id = map.Id,
                Width = map.Width,
                Height = map.Height,
                Walls = map.Walls.Select(ToJsonModel),
                Traps = map.Traps.Select(ToJsonModel)
            };
        }

        private static StepJsonModel ToJsonModel(this Step step)
        {
            return new StepJsonModel
            {
                N = step.Number,
                Bs = step.Bonuses.Select(ToJsonModel),
                Bt = step.Bots.Values.Select(ToJsonModel),
                Bl = step.Bullets.Select(ToJsonModel),
                Ss = step.Shots.Select(ToJsonModel),
                Es = step.Explosions.Select(ToJsonModel),
                L = step.Logs.Select(ToJsonModel)
            };
        }

        private static MapObjectJsonModel ToJsonModel(this MapObject mapObject)
        {
            return new MapObjectJsonModel
            {
                Id = mapObject.Id,
                X = mapObject.X,
                Y = mapObject.Y
            };
        }

        private static ShotJsonModel ToJsonModel(this Shot shot)
        {
            return new ShotJsonModel
            {
                Id = shot.Id,
                Dr = shot.Direction,
                X = shot.X,
                Y = shot.Y
            };
        }

        private static BotJsonModel ToJsonModel(this Bot bot)
        {
            return new BotJsonModel
            {
                Id = bot.Id,
                X = bot.X,
                Y = bot.Y,
                H = bot.Health,
                D = bot.Damage,
                Dr = bot.Direction,
                IsD = bot.IsDamaged.ToInt(),
                IsS = bot.IsStunned.ToInt()
            };
        }

        private static BulletJsonModel ToJsonModel(this Bullet bullet)
        {
            return new BulletJsonModel
            {
                Id = bullet.Id,
                N = bullet.Number,
                X = bullet.X,
                Y = bullet.Y,
                D = bullet.Damage,
                Dr = bullet.Direction,
                E = bullet.Exploded.ToInt()
            };
        }

        private static LogJsonModel ToJsonModel(this Log log)
        {
            return new LogJsonModel
            {
                Id = log.Id,
                S = log.Step,
                Si = log.SourceId,
                T = log.Type,
                Ti = log.TargetId,
                V = log.Value,
                X = log.X,
                Y = log.Y,
                M = log.Message
            };
        }

        private static int? ToInt(this bool value)
        {
            return value ? 1 : (int?)null;
        }
    }
}