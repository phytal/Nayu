using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;
using Newtonsoft.Json;

namespace Nayu.Modules.API.Anime.Both
{
    public class Tickle : NayuModule
    {
        [Command("Tickle")]
        [Summary("Tickle someone! :3")]
        [Remarks("n!tickle <user you want to tickle (if left empty you will tickle yourself)> Ex: n!tickle @Phytal")]
        [Cooldown(10)]
        public async Task GetRandomTickle(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string json = "";
                using (WebClient client = new WebClient())
                {
                    json = client.DownloadString("https://nekos.life/api/v2/img/tickle");
                }

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string nekolink = dataObject.url.ToString();

                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Tickle!");
                    embed.WithDescription(
                        $"{Context.User.Mention} tickled themselves... I'll stay out of this for now... \n **(Include a user with your command! Example: n!tickle <person you want to tickle>)**");
                    embed.WithImageUrl(nekolink);
                    embed.WithFooter($"Powered by nekos.life");

                    await SendMessage(Context, embed);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithImageUrl(nekolink);
                    embed.WithTitle("Tickle!");
                    embed.WithDescription($"{Context.User.Username} tickled {user.Mention}!");
                    embed.WithFooter($"Powered by nekos.life");

                    await SendMessage(Context, embed);
                }
            }

            if (rand == 2)
            {
                string[] tags = new[] { "" };
                weebDotSh.Helpers.WebRequest webReq = new weebDotSh.Helpers.WebRequest();
                RandomData result = await webReq.GetTypesAsync("tickle", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                string id = result.Id;
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Tickle!");
                    embed.WithDescription(
                        $"{Context.User.Mention} tickled themselves, apparently you can get this lonely? \n **(Include a user with your command! Example: n!tickle <person you want to tickle>)**");
                    embed.WithImageUrl(url);
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await SendMessage(Context, embed);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithImageUrl(url);
                    embed.WithTitle("Hug!");
                    embed.WithDescription($"{Context.User.Username} tickled {user.Mention}!");
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await SendMessage(Context, embed);
                }
            }
        }
    }
}
