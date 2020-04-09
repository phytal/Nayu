using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.WeebDotSh
{
    public class Rem : NayuModule
    {
        [Subject(Categories.Images)]
        [Command("rem")]
        [Summary("Displays an image of an anime rem gif")]
        [Remarks("Ex: n!rem")]
        [Cooldown(5)]
        public async Task RemIMG()
        {
            string[] tags = {""};
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("rem", tags, FileType.Any, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            var embed = new EmbedBuilder();

            embed.WithColor(Global.NayuColor);
            embed.WithTitle("Rem!");
            embed.WithDescription(
                $"{Context.User.Mention} here's some rem pics at your disposal :3");
            embed.WithImageUrl(url);
            embed.WithFooter($"Powered by weeb.sh | ID: {id}");

            await SendMessage(Context, embed.Build());
        }
    }
}