using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.weebDotSh
{
    public class Rem : NayuModule
    {
        [Command("rem")]
        [Summary("Displays an image of an anime rem gif")]
        [Remarks("Ex: n!rem")]
        [Cooldown(5)]
        public async Task RemIMG()
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("rem", tags, FileType.Any, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            var embed = new EmbedBuilder();

            embed.WithColor(37, 152, 255);
            embed.WithTitle("Rem!");
            embed.WithDescription(
                $"{Context.User.Mention} here's some rem pics at your disposal :3");
            embed.WithImageUrl(url);
            embed.WithFooter($"Powered by weeb.sh | ID: {id}");

            await Context.Channel.SendMessageAsync("", embed: embed.Build());

        }
    }
}
