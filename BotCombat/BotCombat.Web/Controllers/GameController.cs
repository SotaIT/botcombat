using System.Collections.Generic;
using BotCombat.Web.Converters;
using BotCombat.Web.JsonModels;
using BotCombat.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BotCombat.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("{id}", Name = "Play")]
        public Game Play(int id, [FromQuery(Name = "b")] List<int> bots)
        {
            return _gameService.Play(id, bots).ToJsonModel();
        }



    }
}