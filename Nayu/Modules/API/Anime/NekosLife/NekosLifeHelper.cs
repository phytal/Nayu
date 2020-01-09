using System.Net;
using Newtonsoft.Json;

namespace Nayu.Modules.API.Anime.NekosLife
{
    public class NekosLifeHelper
    {
        public static string GetNekoLink(string key)
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString($"https://nekos.life/api/v2/img/{key}");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekolink = dataObject.url.ToString();
            return nekolink;
        }
    }
}