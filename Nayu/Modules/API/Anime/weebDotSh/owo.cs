using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Weeb.net;
using Weeb.net.Data;
using Nayu.Core.Modules;
using Nayu.Preconditions;

/*

     oooo  ww      ww  oooo  
    oo  oo ww      ww oo  oo 
    oo  oo  ww ww ww  oo  oo 
     oooo    ww  ww    oooo      
 
 */

namespace Nayu.Modules.API.Anime.weebDotSh.NSFW
{
    public class owo : NayuModule
    {
        [Command("owo")]
        [Summary("Displays an image of an anime owo gif")]
        [Remarks("Ex: n!owo")]
        [Cooldown(5)]
        public async Task owoIMG()
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("owo", tags, FileType.Any, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            var embed = new EmbedBuilder();

            embed.WithColor(37, 152, 255);
            embed.WithTitle("owo");
            embed.WithDescription(
                $"owo");
            embed.WithImageUrl(url);
            embed.WithFooter($"Powered by weeb.sh | ID: {id} | owo");

            await Context.Channel.SendMessageAsync("", embed: embed.Build());

        }
    }
}
