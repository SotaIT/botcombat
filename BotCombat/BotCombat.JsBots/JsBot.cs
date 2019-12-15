
using System.Reflection;
using BotCombat.BotUtils;
using BotCombat.BotWorld;
using Jint;

namespace BotCombat.JsBots
{
    public sealed class JsBot : IBot
    {
        private readonly Engine _engine;

        public JsBot(int id, string code = null)
        {
            Id = id;
            _engine = new Engine();

            _engine.Execute(
                $@"{code ?? DefaultSourceCode}
    var bot = new iBot();
    function distributePower(game, power, result) {{ bot.distributePower(power, game, result); }}
    function chooseAction(game, result) {{ bot.chooseAction(game, result); }}
");
        }

        public int Id { get; }

        public ActionResult ChooseAction(Game game)
        {
            var result = new ActionResult();
            _engine.Invoke("chooseAction", game, result);
            return result;
        }

        public StatsResult DistributePower(Game game, int power)
        {
            var result = new StatsResult();
            _engine.Invoke("distributePower", game, power, result);
            return result;
        }

        public static string DefaultSourceCode =>
            Assembly
                .GetCallingAssembly()
                .GetEmbeddedResource("Resources.DefaultJsBot.js");
    }
}