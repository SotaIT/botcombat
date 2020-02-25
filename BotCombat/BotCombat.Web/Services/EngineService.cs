using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BotCombat.Core;
using BotCombat.Engine.Contracts;
using Newtonsoft.Json;

namespace BotCombat.Web.Services
{
    public class EngineService
    {
        public static async Task<GameResult> Play(MapSettings mapSettings, IEnumerable<BotSettings> bots, bool debugMode)
        {
            var requestObject = new GameSettings { MapSettings = mapSettings, Bots = bots, DebugMode = debugMode };
            var json = JsonConvert.SerializeObject(requestObject);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            var response = await client.PostAsync("http://localhost:43113/game/play/", new StringContent(json, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<GameResult>(responseContent);
        }
    }
}