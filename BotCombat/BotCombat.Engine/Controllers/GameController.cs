using System;
using System.Linq;
using System.Threading.Tasks;
using BotCombat.Core;
using BotCombat.Engine.Contracts;
using BotCombat.Engine.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BotCombat.Engine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
        }

        [HttpGet("test")]
        public string Test()
        {
            return "Test";
        }

        [HttpPost]
        [Route("play")]
        public string Play(object request)
        {
            try
            {
                var gameSettings = JsonConvert.DeserializeObject<GameSettings>(request.ToString());
                var gameManager = new GameManager(gameSettings.MapSettings,
                    gameSettings.Bots.Select(bot => BotFactory.CreateBot((BotTypes) bot.Type, bot.Id, bot.Code)));
                var game = gameSettings.DebugMode ? gameManager.DebugPlay() : gameManager.Play();

                return JsonConvert.SerializeObject(new GameResult
                {
                    Game = game,
                    IsError = false
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return JsonConvert.SerializeObject(new GameResult
                {
                   IsError = true, 
                   Message = ex.Message
                });
            }
        }
    }
}
