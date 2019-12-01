using System;
using System.Collections.Generic;
using System.Linq;
using BotCombat.BotWorld;
using BotCombat.Core;
using BotCombat.Web.Data.Domain;
using BotCombat.Web.Models;
using Bonus = BotCombat.Web.Data.Domain.Bonus;
using Game = BotCombat.Web.Data.Domain.Game;
using Map = BotCombat.Web.Data.Domain.Map;
using Trap = BotCombat.Web.Data.Domain.Trap;
using Wall = BotCombat.Web.Data.Domain.Wall;
using StartPoint = BotCombat.Web.Data.Domain.StartPoint;

namespace BotCombat.Web.Services
{
    public class GameService
    {
        public BotDataService BotDataService { get; }
        public GameDataService GameDataService { get; }
        public ImageDataService ImageDataService { get; }
        public MapDataService MapDataService { get; }

        public GameService(MapDataService mapDataService,
            BotDataService botDataService,
            ImageDataService imageDataService,
            GameDataService gameDataService)
        {
            MapDataService = mapDataService;
            BotDataService = botDataService;
            ImageDataService = imageDataService;
            GameDataService = gameDataService;
        }

        /// <summary>
        /// Creates a game
        /// </summary>
        public Game CreateGame(int mapId, List<int> botIds)
        {
            var map = MapDataService.Get(mapId);

            if (botIds.Count > map.MaxBotCount)
                throw new ArgumentOutOfRangeException(nameof(botIds), $"Maximum bot count for this map is: {map.MaxBotCount}!");

            return GameDataService.AddGame(mapId, botIds);
        }

        public List<Game> GetNewGames()
        {
            return GameDataService.GetGames(GameStates.Created);
        }

        public void QueueGame(Game game)
        {
            game.State = (int)GameStates.Queued;
            GameDataService.UpdateGame(game);
        }

        public List<Game> GetQueuedGames()
        {
            return GameDataService.GetGames(GameStates.Queued);
        }

        public void PlayGame(Game game)
        {
            try
            {
                // get bots
                var gameBots = GameDataService.GetBots(game.Id);

                // get map data from DB
                var map = MapDataService.Get(game.MapId);
                var walls = MapDataService.GetMapObjects<Wall>(game.MapId);
                var bonuses = MapDataService.GetMapObjects<Bonus>(game.MapId);
                var traps = MapDataService.GetMapObjects<Trap>(game.MapId);
                var startPoints = MapDataService.GetMapObjects<StartPoint>(game.MapId);
                var allBotImages = MapDataService.GetMapObjects<BotImage>(game.MapId).ToList();

                // images
                var wallImages = walls.ToDictionary(o => o.Id, o => o.ImageId);
                var bonusImages = bonuses.ToDictionary(o => o.Id, o => o.ImageId);
                var trapImages = traps.ToDictionary(o => o.Id, o => o.ImageId);
                GetBotImages(gameBots, allBotImages, 
                    out var botImages, out var bulletImages, out var shotImages, out var explosionImages, out var botImageIds);

                var images = GetImagesForViewModel(map, wallImages, bonusImages, trapImages, botImageIds);

                // create mapsettings
                var mapSettings = GetMapSettings(map, walls, bonuses, traps, startPoints);

                // play the game
                var gameManager = new GameManager(mapSettings, GetIBots(gameBots));
                var gameModel = gameManager.Play();

                // create viewmodel
                var viewModel = new GameViewModel(
                    gameModel,
                    map.Scale,
                    map.BackgroundImageId,
                    map.BulletSpeed,
                    wallImages,
                    bonusImages,
                    trapImages,
                    botImages,
                    bulletImages,
                    shotImages,
                    explosionImages,
                    images);

                // convert to json
                var json = GameSerializer.ToJson(viewModel);

                // calculate bot stats
                CalculateStats(gameModel, gameBots);

                // save
                game.Played = DateTime.UtcNow;
                game.State = (int)GameStates.Played;
                game.Json = json;
                GameDataService.UpdateGame(game, gameBots);
            }
            catch (Exception)
            {
                game.State = (int)GameStates.Error;
                GameDataService.UpdateGame(game);
                throw;
            }
        }

        private static void CalculateStats(BotWorld.Game gameModel, IReadOnlyCollection<GameBot> gameBots)
        {
            foreach (var gameBot in gameBots)
            {
                gameBot.ResetStats();
            }

            var logs = gameModel.Steps.SelectMany(s => s.Logs).ToList();
            foreach (var log in logs)
            {
                var targetBot = gameBots.FirstOrDefault(b => b.BotId == log.TargetId);
                if (targetBot == null) continue;


                switch ((LogType)log.Type)
                {
                    case LogType.Damaged:
                        var sourceBot = gameBots.FirstOrDefault(b => b.BotId == log.SourceId);
                        targetBot.DamageTaken += log.Value;
                        if (sourceBot != null)
                            sourceBot.DamageDone += log.Value;
                        break;
                    case LogType.Bonus:
                        targetBot.BonusPower += log.Value;
                        targetBot.Bonuses++;
                        break;
                    case LogType.Trap:
                        targetBot.TrapDamageTaken += log.Value;
                        targetBot.Traps++;
                        break;
                    case LogType.Killed:
                        targetBot.DeathStep = log.Step - 1;
                        break;
                }
            }
        }

        private IEnumerable<IBot> GetIBots(IEnumerable<GameBot> gameBots)
        {
            var bots = BotDataService
                .Get(gameBots.Select(b => b.BotId).ToList())
                .Select(bot => BotFactory.CreateBot((BotTypes)bot.Type, bot.Id, bot.Code))
                .ToList();
            return bots;
        }

        private static MapSettings GetMapSettings(Map map, IEnumerable<Wall> walls, IEnumerable<Bonus> bonuses, IEnumerable<Trap> traps,
            IEnumerable<StartPoint> startPoints)
        {
            var mapSettings = new MapSettings(
                map.Id,
                map.Width,
                map.Height,
                map.InitialPower,
                map.StrengthWeight,
                map.StaminaWeight,
                map.RangedWeight,
                map.MaxStepCount ?? int.MaxValue,
                map.BonusSpawnInterval,
                map.BulletSpeed,
                map.ActionTimeout,
                map.MemoryLimit,
                walls.Select(w => new BotWorld.Wall(w.Id, w.X, w.Y)).ToList(),
                bonuses.Select(b => new BotWorld.Bonus(b.Id, b.X, b.Y, b.Power)).ToList(),
                traps.Select(t => new BotWorld.Trap(t.Id, t.X, t.Y, t.Damage)).ToList(),
                startPoints.Select(p => new Core.StartPoint(p.X, p.Y, p.BotId)).ToList());
            return mapSettings;
        }

        private static void GetBotImages(IEnumerable<GameBot> gameBots, IReadOnlyList<BotImage> allBotImages, 
            out Dictionary<int, int> botImages, 
            out Dictionary<int, int> bulletImages, 
            out Dictionary<int, int> shotImages, 
            out Dictionary<int, int> explosionImages, 
            out List<int> botImageIds)
        {
            botImages = new Dictionary<int, int>();
            bulletImages = new Dictionary<int, int>();
            shotImages = new Dictionary<int, int>();
            explosionImages = new Dictionary<int, int>();
            botImageIds = new List<int>();

            var botImageIndex = 0;
            var firsCycle = true;
            foreach (var gameBot in gameBots)
            {
                botImages[gameBot.BotId] = allBotImages[botImageIndex].ImageId;
                bulletImages[gameBot.BotId] = allBotImages[botImageIndex].BulletImageId;
                shotImages[gameBot.BotId] = allBotImages[botImageIndex].ShotImageId;
                explosionImages[gameBot.BotId] = allBotImages[botImageIndex].ExplosionImageId;

                if (firsCycle)
                {
                    botImageIds.Add(allBotImages[botImageIndex].ImageId);
                    botImageIds.Add(allBotImages[botImageIndex].BulletImageId);
                    botImageIds.Add(allBotImages[botImageIndex].ShotImageId);
                    botImageIds.Add(allBotImages[botImageIndex].ExplosionImageId);
                }

                botImageIndex++;
                if (botImageIndex < allBotImages.Count) continue;
                botImageIndex = 0;
                firsCycle = false;
            }
        }

        private Dictionary<int, string> GetImagesForViewModel(Map map, Dictionary<int, int> wallImages, Dictionary<int, int> bonusImages,
            Dictionary<int, int> trapImages, IEnumerable<int> botImageIds)
        {
            var imageIds = new List<int> { map.BackgroundImageId };
            imageIds.AddRange(wallImages.Values);
            imageIds.AddRange(bonusImages.Values);
            imageIds.AddRange(trapImages.Values);
            imageIds.AddRange(botImageIds);
            var images = ImageDataService.Get(imageIds.Distinct().ToList()).ToDictionary(i => i.Id, i => i.FileName);
            return images;
        }

        public List<Game> GetPlayedGames()
        {
            return GameDataService.GetGames(GameStates.Played);
        }


    }
}