using System.Collections.Generic;
using BotCombat.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BotCombat.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;
        private readonly GameDataService _gameDataService;

        public GameController(GameService gameService, GameDataService gameDataService)
        {
            _gameService = gameService;
            _gameDataService = gameDataService;
        }

        [HttpGet("create/{id}")]
        public int? Create(int id, [FromQuery(Name = "b")] List<int> bots)
        {
            return _gameService.CreateGame(id, bots).Id;
        }

        [HttpGet("get/{id}")]
        public string Get(int id)
        {
            return _gameDataService.Get(id).Json;
        }
    }
}