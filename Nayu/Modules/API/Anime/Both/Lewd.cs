using System.Net;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Nayu.Preconditions;
using Nayu.Core.Modules;
using System;
using Weeb.net;
using Weeb.net.Data;

namespace Nayu.Modules.API.Anime
{
    public class Lewd : NayuModule
    {
        [Command("lewd")]
        [Summary("Displays a lewd image (nsfw)")]
        [Remarks("Ex: n!lewd")]
        [Cooldown(10)]
        public async Task GetRandomNekoLewd()
        {
            var channel = Context.Channel as ITextChannel;
            if (channel.IsNsfw)
            {
                int rand = Global.Rng.Next(1, 3);
                if (rand == 1)
                {
                    string json = "";
                    using (WebClient client = new WebClient())
                    {
                        json = client.DownloadString("https://nekos.life/api/v2/img/lewd");
                    }

                    var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                    string nekolink = dataObject.url.ToString();

                    var embed = new EmbedBuilder();
                    embed.WithTitle("Randomly generated lewd neko just for you <3!");
                    embed.WithImageUrl(nekolink);
                    embed.WithFooter($"Powered by nekos.life");
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }

                if (rand == 2)
                {
                    string[] tags = new[] { "" };
                    weebDotSh.Helpers.WebRequest webReq = new weebDotSh.Helpers.WebRequest();
                    RandomData result = await webReq.GetTypesAsync("neko", tags, FileType.Gif, NsfwSearch.Only, false);
                    string url = result.Url;
                    string id = result.Id;
                    var embed = new EmbedBuilder();

                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Lewd!");
                    embed.WithDescription(
                        $"{Context.User.Mention} here's some lewd anime girls at your disposal :3");
                    embed.WithImageUrl(url);
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need to use this command in a NSFW channel, {Context.User.Username}!";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}