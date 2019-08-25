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
    public class Hug : NayuModule
    {
        [Command("Hug")]
        [Summary("Hug someone!")]
        [Remarks("n!hug <user you want to hug (if left empty you will hug yourself)> Ex: n!hug @Phytal")]
        [Cooldown(10)]
        public async Task GetRandomNekoHug(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string json = "";
                using (WebClient client = new WebClient())
                {
                    json = client.DownloadString("https://nekos.life/api/v2/img/hug");
                }

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string nekolink = dataObject.url.ToString();

                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Hug!");
                    embed.WithDescription(
                        $"{Context.User.Mention} hugged themselves... Aw, don't be sad, you can hug me! \n **(Include a user with your command! Example: n!hug <person you want to hug>)**");
                    embed.WithImageUrl(nekolink);
                    embed.WithFooter($"Powered by nekos.life");

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithImageUrl(nekolink);
                    embed.WithTitle("Hug!");
                    embed.WithDescription($":heart:  |  {Context.User.Username} hugged {user.Mention}!");
                    embed.WithFooter($"Powered by nekos.life");

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
            }

            if (rand == 2)
            {
                string[] tags = new[] { "" };
                weebDotSh.Helpers.WebRequest webReq = new weebDotSh.Helpers.WebRequest();
                RandomData result = await webReq.GetTypesAsync("hug", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                string id = result.Id;
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Hug!");
                    embed.WithDescription(
                        $"{Context.User.Mention} hugged themselves, how is that even physically possible? \n **(Include a user with your command! Example: n!hug <person you want to hug>)**");
                    embed.WithImageUrl(url);
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithImageUrl(url);
                    embed.WithTitle("Hug!");
                    embed.WithDescription($"{Context.User.Username} hugged {user.Mention}!");
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
            }
        }
    }
}
