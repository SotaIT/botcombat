using System;
using System.Linq;
using System.Reflection;
using BotCombat.Abstractions;
using BotCombat.Abstractions.BotModels;
using BotCombat.BotUtils.Extensions;
using Jint;

namespace BotCombat.Js
{
    public sealed partial class JsBot : IBot
    {
        private readonly Engine _engine;

        public JsBot(int id, string code = null)
        {
            Id = id;
            _engine = new Engine();

            _engine.Execute(
                $@"{MoveDirectionToJs()}
                {code ?? DefaultSourceCode}
    var bot = new iBot();
    function distributePower(power, game, result) {{ bot.distributePower(power, game, result); }}
    function chooseDirection(game, result) {{ bot.chooseDirection(game, result); }}
");
        }

        public int Id { get; }

        public MoveDirection ChooseDirection(Game game)
        {
            var result = new DirectionResult();
            _engine.Invoke("chooseDirection", game, result);
            return result.Direction;
        }

        public PowerStats DistributePower(int power, Game game)
        {
            return DistributePowerJs("distributePower", power, game);
        }

        private static string MoveDirectionToJs()
        {
            var members = Enum.GetValues(typeof(MoveDirection)).Cast<MoveDirection>().Select(m => $"{m}: {(int)m}");
            return $"let MoveDirection = {{{string.Join(", ", members)}}};";
        }

        private PowerStats DistributePowerJs(string funcName, int power, Game game)
        {
            var result = new PowerStats();
            _engine.Invoke(funcName, power, game, result);
            return result;
        }

        private class DirectionResult
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public MoveDirection Direction { get; set; }
        }

        private static string DefaultSourceCode =>
            Assembly
                .GetCallingAssembly()
                .GetEmbeddedResource("Resources.DefaultJsBot.js");
    }
}