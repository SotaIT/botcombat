using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BotCombat.Abstractions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace BotCombat.Cs
{
    public sealed partial class CsBot : IBot
    {
        public CsBot(int id, int timeOut, string sourceCode)
        {
            Id = id;
            TimeOut = timeOut;

            Bot = CreateBot(sourceCode);
        }

        private IBot Bot { get; }

        public int TimeOut { get; }

        public int Id { get; }

        public MoveDirection ChooseDirection(Step step)
        {
            return Bot.ChooseDirection(step);
        }

        public Dictionary<PowerStats, int> DistributePower(int power, Step step)
        {
            return Bot.DistributePower(power, step);
        }

        public Dictionary<PowerStats, int> InitPower(int power, Step step)
        {
            return Bot.InitPower(power, step);
        }

        private IBot CreateBot(string sourceCode)
        {
            var fileName = $"BotCombat.Cs.Bot{Id}.dll";

            if (!File.Exists(fileName))
            {
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
                locations.Add(typeof(IBot).GetTypeInfo().Assembly.Location);

                var compilation = CSharpCompilation.Create(fileName)
                    .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                    .AddReferences(locations.Select(l => MetadataReference.CreateFromFile(l)))
                    .AddSyntaxTrees(CSharpSyntaxTree.ParseText(sourceCode));
                var emitResult = compilation.Emit(fileName);
                if (!emitResult.Success)
                    throw new Exception("Compile error: " + GetErrorMessage(emitResult));
            }

            var @class = Assembly.LoadFrom(fileName).GetTypes().FirstOrDefault(t => typeof(IBot).IsAssignableFrom(t));
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