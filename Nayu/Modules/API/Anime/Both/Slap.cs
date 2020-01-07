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
    public class Slap : NayuModule
    {
        [Command("slap")]
        [Summary("Slap someone!")]
        [Remarks("n!slap <user you want to slap (if left empty you will slap yourself)> Ex: n!slap @Phytal")]
        [Cooldown(10)]
        public async Task GetRandomNekoHug(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string json = "";
                using (WebClient client = new WebClient())
                {
                    json = client.DownloadString("https://nekos.life/api/v2/img/slap");
                }

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string nekolink = dataObject.url.ToString();

                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Slap!");
                    embed.WithDescription(
                        $"{Context.User.Mention} slapped themselves... Don't do this to yourself! \n **(Include a user with your command! Example: n!slap <person you want to slap>)**");
                    embed.WithImageUrl(nekolink);
                    embed.WithFooter($"Powered by nekos.life");

                    await SendMessage(Context, embed);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithImageUrl(nekolink);
                    embed.WithTitle("Slap!");
                    embed.WithDescription($"{Context.User.Username} slapped {user.Mention}!");
                    embed.WithFooter($"Powered by nekos.life");

                    await SendMessage(Context, embed);
                }
            }

            if (rand == 2)
            {
                string[] tags = new[] { "" };
                weebDotSh.Helpers.WebRequest webReq = new weebDotSh.Helpers.WebRequest();
                RandomData result = await webReq.GetTypesAsync("slap", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                string id = result.Id;
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Smack!");
                    embed.WithDescription(
                        $"{Context.User.Mention} slapped themselves, masochism much? \n **(Include a user with your command! Example: n!slap <person you want to slap>)**");
                    embed.WithImageUrl(url);
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await SendMessage(Context, embed);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithImageUrl(url);
                    embed.WithTitle("Smack!");
                    embed.WithDescription($"{Context.User.Username} slapped {user.Mention}!");
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await SendMessage(Context, embed);
                }
            }
        }
    }
}
