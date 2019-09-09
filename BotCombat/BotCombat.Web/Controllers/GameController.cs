using System.Collections.Generic;
using BotCombat.Web.Converters;
using BotCombat.Web.Models;
using BotCombat.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BotCombat.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService = new GameService();

        [HttpGet]
        public IEnumerable<StepModel> Play(int mapId, List<int> bots)
        {
            return _gameService.Play(mapId, bots).ToModel();
        }

    }
}