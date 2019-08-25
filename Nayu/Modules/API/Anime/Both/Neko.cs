using System.Threading.Tasks;
using Discord.Commands;
using System.Net;
using Newtonsoft.Json;
using Discord;
using Weeb.net;
using Weeb.net.Data;
using Nayu.Preconditions;
using Nayu.Core.Modules;

namespace Nayu.Modules.API.Anime
{
    public class Neko : NayuModule
    {
        [Command("neko")]
        [Summary("Displays an random neko :3")]
        [Remarks("Ex: n!neko")]
        [Cooldown(10)]
        public async Task GetRandomNeko()
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string json = "";
                using (WebClient client = new WebClient())
                {
                    json = client.DownloadString("https://nekos.life/api/v2/img/neko");
                }

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string nekolink = dataObject.url.ToString();

                var embed = new EmbedBuilder();
                embed.WithTitle("Randomly generated neko just for you <3!");
                embed.WithImageUrl(nekolink);
                embed.WithFooter($"Powered by nekos.life");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }

            if (rand == 2)
            {
                string[] tags = new[] { "" };
                weebDotSh.Helpers.WebRequest webReq = new weebDotSh.Helpers.WebRequest();
                RandomData result = await webReq.GetTypesAsync("neko", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                string id = result.Id;
                var embed = new EmbedBuilder();

                embed.WithColor(37, 152, 255);
                embed.WithTitle("Neko!");
                embed.WithDescription(
                    $"{Context.User.Mention} here's some nekos at your disposal :3");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }
    }
}
