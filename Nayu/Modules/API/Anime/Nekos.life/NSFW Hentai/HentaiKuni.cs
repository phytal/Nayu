using System;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Preconditions;
using Newtonsoft.Json;

namespace Nayu.Modules.API.Anime.Nekos.life.NSFW_Hentai
{
    public class HentaiKuni : NayuModule
    {
        [Command("kuni")]
        [Summary("Displays a hentai kuni")]
        [Remarks("Ex: n!kuni")]
        [Cooldown(5)]
        public async Task GetRandomNekoKuni()
        {
            var channel = Context.Channel as ITextChannel;
            if (channel.IsNsfw)
            {
                string json = "";
                using (WebClient client = new WebClient())
                {
                    json = client.DownloadString("https://nekos.life/api/v2/img/kuni");
                }

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string nekolink = dataObject.url.ToString();

                var embed = new EmbedBuilder();
                embed.WithTitle("Randomly generated hentai kunai just for you <3!");
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
