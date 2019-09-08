using BotCombat.Core;
using Jint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BotCombat.Js
{
    public class JsBot : IBot
    {

        public const string DefaultInitPowerScript = "result.Strength = power / 2; result.Stamina = power - result.Strength;";
        public const string DefaultDistributePowerScript = DefaultInitPowerScript;
        public const string DefaultChooseDirectionScript = "result.Direction = Math.floor(Math.random() * 5);";

        private readonly Engine engine;

        private class PowerResult
        {
            public int Strength { get; set; }

            public int Stamina { get; set; }

        }

        private class DirectionResult
        {
            public MoveDirection Direction { get; set; }
        }

        public JsBot(int id, int timeOut,  MapImage botImage, string initPowerScript, string distributePowerScript, string chooseDirectionScript)
        {
            Id = id;
            TimeOut = timeOut;
            BotImage = botImage;

            engine = new Engine(options =>
                options.TimeoutInterval(TimeSpan.FromMilliseconds(timeOut))
            );

            engine.Execute(
                $"function initPower(power, step, result) {{ {initPowerScript} }}" +
                $"function distributePower(power, step, result) {{ {distributePowerScript} }}" +
                $"function chooseDirection(step, result) {{ {MoveDirectionToJs()} {chooseDirectionScript} }}"
                );

        }

        public MapImage BotImage { get; }

        public int Id { get; }
        public int TimeOut { get; }

        public MoveDirection ChooseDirection(Step step)
        {
            var result = new DirectionResult();
            engine.Invoke("chooseDirection", step, result);
            return result.Direction;
        }

        private string MoveDirectionToJs()
        {
            var members = Enum.GetValues(typeof(MoveDirection)).Cast<MoveDirection>().Select(m => $"{m}: {(int)m}");
            return $"let MoveDirection = {{{(string.Join(", ", members))}}};";
        }

        public Dictionary<PowerStats, int> DistributePower(int power, Step step)
        {
            return DistributePowerJs("distributePower", power, step);
        }

        public Dictionary<PowerStats, int> InitPower(int power, Step step)
        {
            return DistributePowerJs("initPower", power, step);
        }

        private Dictionary<PowerStats, int> DistributePowerJs(string funcName, int power, Step step)
        {
            var result = new PowerResult();
            engine.Invoke(funcName, power, step, result);
            return new Dictionary<PowerStats, int>
            {
                [PowerStats.Strength] = result.Strength,
                [PowerStats.Stamina] = result.Stamina
            };
        }
    }
}
