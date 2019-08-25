using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Weeb.net;
using Weeb.net.Data;
using Nayu.Core.Modules;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.weebDotSh
{
    public class Baka : NayuModule
    {
        [Command("baka")]
        [Summary("Displays an image of an anime baka gif")]
        [Remarks("Usage: n!baka <user you want to call a baka (or can be left empty)> Ex: n!baka @Phytal")]
        [Cooldown(5)]
        public async Task bakaUser(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string[] tags = new[] {""};
                Helpers.WebRequest webReq = new Helpers.WebRequest();
                RandomData result = await webReq.GetTypesAsync("baka", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                string id = result.Id;
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Baka!");
                    embed.WithDescription(
                        $"{Context.User.Mention} called themselves a baka! I kind of agree with that.. \n**(Include a user with your command! Example: n!baka <person you want to call a baka>)**");
                    embed.WithImageUrl(url);
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithImageUrl(url);
                    embed.WithTitle("Baka!");
                    embed.WithDescription($"{Context.User.Username} called {user.Mention} a baka!");
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
            }

            if (rand == 2)
            {
                {
                    string json = "";
                    using (WebClient client = new WebClient())
                    {
                        json = client.DownloadString("https://nekos.life/api/v2/img/baka");
                    }

                    var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                    string nekolink = dataObject.url.ToString();

                    var embed = new EmbedBuilder();
                    embed.WithTitle("Randomly generated neko just for you <3!");
                    embed.WithImageUrl(nekolink);
                    embed.WithFooter($"Powered by nekos.life");

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
            }
        }
    }
}
