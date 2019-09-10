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

        [HttpGet]
        public Game Play(int mapId, List<int> bots)
        {
            return ServiceFactory.GameService.Play(mapId, bots).ToJsonModel();
        }
    }
}