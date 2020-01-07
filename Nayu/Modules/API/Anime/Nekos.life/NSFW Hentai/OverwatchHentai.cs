using System;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Preconditions;
using Newtonsoft.Json;

namespace Nayu.Modules.API.Anime.Nekos.life.NSFW_Hentai
{
    public class OverwatchHentai : NayuModule
    {
        [Command("overwatchnsfw")]
        [Summary("Generates a picture of NSFW Overwatch from r/OverwatchNSFW")]
        [Alias("ownsfw")]
        [Remarks("Ex: n!ownsfw")]
        [Cooldown(5)]
        public async Task GetRandomOWHentai()
        {
            var channel = Context.Channel as ITextChannel;
            if (channel.IsNsfw)
            {
                string json;
                using (var client = new WebClient())
                {
                    json = client.DownloadString("https://www.reddit.com/r/OverwatchNSFW/random/.json");
                }

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);
                string image = dataObject[0].data.children[0].data.url.ToString();
                string posttitle = dataObject[0].data.children[0].data.title.ToString();
                string link = dataObject[0].data.children[0].data.permalink.ToString();
                string ups = dataObject[0].data.children[0].data.ups.ToString();
                string comments = dataObject[0].data.children[0].data.num_comments.ToString();

                var embed = new EmbedBuilder()
                    .WithTitle(posttitle)
                    .WithImageUrl(image)
                    .WithFooter($"👍 {ups} | 💬 {comments} (If image is not shown you can click on the link)")
                    .WithUrl($"https://www.reddit.com{link}")
                    .WithColor(37, 152, 255);
                await SendMessage(Context, embed);
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
