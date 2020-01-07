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
    public class Poke : NayuModule
    {
        [Command("poke")]
        [Summary("Poke someone! :3")]
        [Remarks("n!poke <user you want to poke (if left empty you will poke yourself)> Ex: n!poke @Phytal")]
        [Cooldown(10)]
        public async Task GetRandomNeko(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string json = "";
                using (WebClient client = new WebClient())
                {
                    json = client.DownloadString("https://nekos.life/api/v2/img/poke");
                }

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string nekolink = dataObject.url.ToString();

                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Poke!");
                    embed.WithDescription(
                        $"{Context.User.Mention} poked themselves... I guess you can poke yourself if you're lonely... \n **(Include a user with your command! Example: n!poke <person you want to poke>)**");
                    embed.WithImageUrl(nekolink);
                    embed.WithFooter($"Powered by nekos.life");

                    await SendMessage(Context, embed);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithImageUrl(nekolink);
                    embed.WithTitle("Poke!");
                    embed.WithDescription($":point_right:  |  {Context.User.Username} poked {user.Mention}!");
                    embed.WithFooter($"Powered by nekos.life");

                    await SendMessage(Context, embed);
                }
            }

            if (rand == 2)
            {
                string[] tags = new[] { "" };
                weebDotSh.Helpers.WebRequest webReq = new weebDotSh.Helpers.WebRequest();
                RandomData result = await webReq.GetTypesAsync("poke", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                string id = result.Id;
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Poke!");
                    embed.WithDescription(
                        $"{Context.User.Mention} poked themselves, yes, I know the human body is  *i n t e r e s t i n g*  :3. \n **(Include a user with your command! Example: n!poke <person you want to poke>)**");
                    embed.WithImageUrl(url);
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await SendMessage(Context, embed);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithImageUrl(url);
                    embed.WithTitle("Poke!");
                    embed.WithDescription($"{Context.User.Username} poked {user.Mention}!");
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await SendMessage(Context, embed);
                }
            }
        }
    }
}
