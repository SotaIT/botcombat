using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BotCombat.Abstractions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace BotCombat.Cs
{
    public sealed partial class CsBot : IBot
    {
        private IBot _bot { get; }

        public CsBot(int id, int timeOut, MapImage botImage, string sourceCode)
        {
            Id = id;
            TimeOut = timeOut;
            BotImage = botImage;

            _bot = CreateBot(sourceCode);
        }

        private IBot CreateBot(string sourceCode)
        {
            var fileName = $"BotCombat.Cs.Bot{Id}.dll";

            var coreDir = Directory.GetParent(typeof(object).GetTypeInfo().Assembly.Location);

            var coreLibs = new[] 
            {
                "mscorlib.dll" ,
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
                throw new Exception("Compile error!");

            var @class = Assembly.LoadFrom(fileName).GetTypes().FirstOrDefault(t => typeof(IBot).IsAssignableFrom(t));
            return Activator.CreateInstance(@class, Id, BotImage) as IBot;
        }

        public MapImage BotImage { get; }

        public int Id { get; }

        public int TimeOut { get; }

        public MoveDirection ChooseDirection(Step step)
        {
            return _bot.ChooseDirection(step);
        }

        public Dictionary<PowerStats, int> DistributePower(int power, Step step)
        {
            return _bot.DistributePower(power, step);
        }

        public Dictionary<PowerStats, int> InitPower(int power, Step step)
        {
            return _bot.InitPower(power, step);
        }
    }
}
