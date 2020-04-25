using System.Net;
using System.Threading.Tasks;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Preconditions;
using Newtonsoft.Json;

namespace Nayu.Modules.API.Anime.NekosLife
{
    public class CatEmoticon : NayuModule
    {
        [Subject(Categories.Fun)]
        [Command("catemoticon")]
        [Summary("Displays an random cat emoticon :3")]
        [Alias("cate")]
        [Remarks("Ex: n!cate")]
        [Cooldown(5)]
        public async Task GetRandomNeko()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/v2/cat");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekoLink = dataObject.cat.ToString();

            await SendMessage(Context, null, nekoLink);
        }
    }
}