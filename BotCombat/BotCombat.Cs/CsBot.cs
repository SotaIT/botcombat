using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BotCombat.Abstractions;
using BotCombat.Abstractions.BotModels;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace BotCombat.Cs
{
    public sealed partial class CsBot : IBot
    {
        public CsBot(int id): this(id, DefaultSourceCode)
        {
            
        }

        public CsBot(int id, string code)
        {
            Id = id;
            Bot = CreateBot(code);
        }

        private IBot Bot { get; }

        public int Id { get; }

        public MoveDirection ChooseDirection(Game game)
        {
            return Bot.ChooseDirection(game);
        }

        public Dictionary<PowerStats, int> DistributePower(int power, Game game)
        {
            return Bot.DistributePower(power, game);
        }

        public Dictionary<PowerStats, int> InitPower(int power, Game game)
        {
            return Bot.InitPower(power, game);
        }

        private IBot CreateBot(string sourceCode)
        {
            var fileName = $"BotCombat.Cs.Bot{Id}.dll";
            var filePath = $"Bots{Path.DirectorySeparatorChar}{fileName}";

            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory("Bots");

                var coreDir = Directory.GetParent(typeof(object).GetTypeInfo().Assembly.Location);

                var coreLibs = new[]
                {
                    "mscorlib.dll",
                    "netstandard.dll",
                    "System.Runtime.dll",
                    "System.Private.CoreLib.dll",
                    "System.Linq.dll",
                    "System.Collections.dll"
                };

                var locations = coreLibs.Select(cl => $"{coreDir.FullName}{Path.DirectorySeparatorChar}{cl}").ToList();
                locations.Add(typeof(BaseBot).GetTypeInfo().Assembly.Location);

                var compilation = CSharpCompilation.Create(fileName)
                    .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                    .AddReferences(locations.Select(l => MetadataReference.CreateFromFile(l)))
                    .AddSyntaxTrees(CSharpSyntaxTree.ParseText(sourceCode));
                var emitResult = compilation.Emit(filePath);
                if (!emitResult.Success)
                    throw new Exception("Compile error: " + GetErrorMessage(emitResult));
            }

            var @class = Assembly.LoadFrom(filePath).GetTypes().FirstOrDefault(t => typeof(BaseBot).IsAssignableFrom(t));
            if (@class == null)
                throw new Exception("Bot class not found!");

            return Activator.CreateInstance(@class, Id) as IBot;
        }

        private static string GetErrorMessage(EmitResult emitResult)
        {
            return string.Join(" \n", emitResult.Diagnostics);
        }
    }
}