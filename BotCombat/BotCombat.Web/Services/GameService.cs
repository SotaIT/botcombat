using System.Collections.Generic;
using BotCombat.Core;
using Game = BotCombat.Abstractions.BotModels.Game;

namespace BotCombat.Web.Services
{
    public class GameService
    {
        private readonly MapDataService _mapService;
        private readonly BotDataService _botService;

        public GameService(MapDataService mapService, BotDataService botService)
        {
            _mapService = mapService;
            _botService = botService;
        }

        public Game Play(int mapId, List<int> botIds)
        {
            // get map and bots from DB
            var map = _mapService.GetCoreMap(mapId);
            var bots = _botService.GetBots(botIds);

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