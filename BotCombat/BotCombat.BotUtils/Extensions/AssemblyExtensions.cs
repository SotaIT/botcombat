using System.IO;
using System.Reflection;
using System.Text;

namespace BotCombat.BotUtils.Extensions
{
    public static class AssemblyExtensions
    {
        public static string GetEmbeddedResource(this Assembly assembly, string name)
        {
            using (var resourceStream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{name}"))
                if (resourceStream != null)
                    using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }

            return null;
        }
    }
}
