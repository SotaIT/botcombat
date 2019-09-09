﻿namespace BotCombat.Cs
{
    public partial class CsBot
    {
        public static string DefaultSourceCode =>
@"
using System;
using System.Collections.Generic;
using BotCombat.Abstractions;
using BotCombat.Abstractions.Models;

namespace BotCombat.Cs
{
    public class ExampleCsBot : BaseBot
    {
        public ExampleCsBot(int id) : base(id)
        {
        }

        public override MoveDirection ChooseDirection(Game game)
        {
            return (MoveDirection)(new Random().Next(4) + 1);
        }

        public override Dictionary<PowerStats, int> DistributePower(int power, Game game)
        {
            var strength = power / 2;

            return new Dictionary<PowerStats, int>
            {
                [PowerStats.Strength] = strength,
                [PowerStats.Stamina] = power - strength
            };
        }

        public override Dictionary<PowerStats, int> InitPower(int power, Game game)
        {
            return DistributePower(power, game);
        }
    }
}
";
    }
}
