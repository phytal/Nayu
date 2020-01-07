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
    public class Feed : NayuModule
    {
        [Command("feed")]
        [Summary("Feed someone!")]
        [Remarks("n!feed <user you want to feed (if left empty you will feed yourself)> Ex: n!feed @Phytal")]
        [Cooldown(10)]
        public async Task GetRandomNekoHug(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string json = "";
                using (WebClient client = new WebClient())
                {
                    json = client.DownloadString("https://nekos.life/api/v2/img/feed");
                }

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string nekolink = dataObject.url.ToString();

                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Munch!");
                    embed.WithDescription(
                        $"{Context.User.Mention} fed themselves... Let's hope they don't get fat... \n **(Include a user with your command! Example: n!feed <person you want to feed>)**");
                    embed.WithImageUrl(nekolink);
                    embed.WithFooter($"Powered by nekos.life");

                    await SendMessage(Context, embed);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithImageUrl(nekolink);
                    embed.WithTitle("Munch!");
                    embed.WithDescription($"{Context.User.Username} fed {user.Mention}!");
                    embed.WithFooter($"Powered by nekos.life");

                    await SendMessage(Context, embed);
                }
            }

            if (rand == 2)
            {
                string[] tags = new[] { "" };
                weebDotSh.Helpers.WebRequest webReq = new weebDotSh.Helpers.WebRequest();
                RandomData result = await webReq.GetTypesAsync("feed", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                string id = result.Id;
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Yum!");
                    embed.WithDescription(
                        $"{Context.User.Mention} fed themselves, I think that's a bit too normal. \n **(Include a user with your command! Example: n!feed <person you want to feed>)**");
                    embed.WithImageUrl(url);
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await SendMessage(Context, embed);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithImageUrl(url);
                    embed.WithTitle("Yum!");
                    embed.WithDescription($"{Context.User.Username} fed {user.Mention}!");
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await SendMessage(Context, embed);
                }
            }
        }
    }
}
