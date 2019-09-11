using System.Collections.Generic;
using System.Linq;
using BotCombat.Core;
using BotCombat.Web.Models;
using Game = BotCombat.Abstractions.BotModels.Game;

namespace BotCombat.Web.Services
{
    public class GameService
    {
        private readonly MapDataService _mapDataService;
        private readonly BotDataService _botDataService;
        private readonly ImageDataService _imageDataService;

        public GameService(MapDataService mapDataService,
            BotDataService botDataService,
            ImageDataService imageDataService)
        {
            _mapDataService = mapDataService;
            _botDataService = botDataService;
            _imageDataService = imageDataService;
        }

        public GameViewModel GetGame(int mapId, List<int> botIds)
        {
            var game = new GameViewModel
            {
                Map = _mapDataService.GetMap(mapId),
                Walls = _mapDataService.GetMapWalls(mapId).ToList(),
                Bonuses = _mapDataService.GetMapBonuses(mapId).ToList(),
                Traps = _mapDataService.GetMapTraps(mapId).ToList(),
                Bots = _botDataService.GetBots(botIds).ToList()
            };

            var imageIds = new List<int> { game.Map.BackgroundImageId };
            imageIds.AddRange(game.Bonuses.Select(b => b.ImageId));
            imageIds.AddRange(game.Bots.Select(b => b.ImageId));
            game.Images = _imageDataService.GetImages(imageIds.Distinct().ToList()).ToList();

            return game;
        }


        public Game Play(int mapId, List<int> botIds)
        {
            // get map and bots from DB
            var map = _mapDataService.GetCoreMap(mapId);
            var bots = _botDataService.GetFullBots(botIds);

            // create MapManager
            var mapManager = new MapManager(map, bots);

            // loop MakeStep
            Game game;
            while (true)
            {
                game = mapManager.MakeStep();
                var step = game.LastStep;

                if (step.Bots.Count < 2) break;
            }

            // return the result
            return game;
        }
    }
}