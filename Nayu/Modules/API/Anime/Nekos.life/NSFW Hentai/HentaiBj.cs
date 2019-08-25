using System.Net;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Nayu.Preconditions;
using Nayu.Core.Modules;
using System;

namespace Nayu.Modules.API.Nekos.life.NSFW_Hentai
{
    public class HentaiBj : NayuModule
    {
        [Command("blowjob")]
        [Alias("bj")]
        [Summary("Displays hentai blowjob")]
        [Remarks("Ex: n!blowjob")]
        [Cooldown(5)]
        public async Task GetRandomNekoBj()
        {
            var channel = Context.Channel as ITextChannel;
            if (channel.IsNsfw)
            {
                string json = "";
                using (WebClient client = new WebClient())
                {
                    json = client.DownloadString("https://nekos.life/api/v2/img/bj");
                }

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string nekolink = dataObject.url.ToString();

                var embed = new EmbedBuilder();
                embed.WithTitle("Randomly generated hentai blowjob just for you <3!");
                embed.WithImageUrl(nekolink);
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
