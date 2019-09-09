using System;
using System.Collections.Generic;
using System.Linq;
using BotCombat.Abstractions;
using BotCombat.Abstractions.Models;
using Jint;

namespace BotCombat.Js
{
    public class JsBot : IBot
    {
        public const string DefaultInitPowerScript =
            "result.Strength = power / 2; result.Stamina = power - result.Strength;";

        public const string DefaultDistributePowerScript = DefaultInitPowerScript;
        public const string DefaultChooseDirectionScript = "result.Direction = Math.floor(Math.random() * 5);";

        private readonly Engine _engine;

        public JsBot(int id, int timeOut, string initPowerScript, string distributePowerScript,
            string chooseDirectionScript)
        {
            Id = id;
            TimeOut = timeOut;
            _engine = new Engine();

            _engine.Execute(
                $"function initPower(power, game, result) {{ {initPowerScript} }}" +
                $"function distributePower(power, game, result) {{ {distributePowerScript} }}" +
                $"function chooseDirection(game, result) {{ {MoveDirectionToJs()} {chooseDirectionScript} }}"
            );
        }

        public int TimeOut { get; }

        public int Id { get; }

        public MoveDirection ChooseDirection(Game game)
        {
            var result = new DirectionResult();
            _engine.Invoke("chooseDirection", game, result);
            return result.Direction;
        }

        public Dictionary<PowerStats, int> DistributePower(int power, Game game)
        {
            return DistributePowerJs("distributePower", power, game);
        }

        public Dictionary<PowerStats, int> InitPower(int power, Game game)
        {
            return DistributePowerJs("initPower", power, game);
        }

        private static string MoveDirectionToJs()
        {
            var members = Enum.GetValues(typeof(MoveDirection)).Cast<MoveDirection>().Select(m => $"{m}: {(int)m}");
            return $"let MoveDirection = {{{string.Join(", ", members)}}};";
        }

        private Dictionary<PowerStats, int> DistributePowerJs(string funcName, int power, Game game)
        {
            var result = new PowerResult();
            _engine.Invoke(funcName, power, game, result);
            return new Dictionary<PowerStats, int>
            {
                [PowerStats.Strength] = result.Strength,
                [PowerStats.Stamina] = result.Stamina
            };
        }

        private class PowerResult
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int Strength { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int Stamina { get; set; }
        }

        private class DirectionResult
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public MoveDirection Direction { get; set; }
        }
    }
}