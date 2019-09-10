using System.Collections.Generic;
using BotCombat.Core;
using Game = BotCombat.Abstractions.BotModels.Game;

namespace BotCombat.Web.Services
{
    public class GameService
    {
        public Game Play(int mapId, List<int> botIds)
        {
            // get map and bots from DB
            var map = ServiceFactory.MapService.GetMap(mapId);
            var bots = ServiceFactory.BotService.GetBots(botIds);

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