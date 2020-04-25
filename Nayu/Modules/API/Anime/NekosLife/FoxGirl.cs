using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Preconditions;
using Newtonsoft.Json;

namespace Nayu.Modules.API.Anime.NekosLife
{
    public class FoxGirl : NayuModule
    {
        [Subject(Categories.Images)]
        [Command("foxgirl")]
        [Summary("Displays an random fox girl :3")]
        [Remarks("Ex: n!neko")]
        [Cooldown(5)]
        public async Task GetRandomFoxGirl()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/v2/img/fox_girl");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekoLink = dataObject.url.ToString();

            var embed = new EmbedBuilder();
            embed.WithTitle("Randomly generated fox girl just for you <3!");
            embed.WithImageUrl(nekoLink);
            await SendMessage(Context, embed.Build());
        }
    }
}