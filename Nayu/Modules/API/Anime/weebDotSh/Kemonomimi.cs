using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.weebDotSh
{
    public class Kemonomimi : NayuModule
    {
        [Command("kemonomimi")]
        [Summary("Displays an image of an anime kemonomimi gif")]
        [Remarks("Ex: n!kemonomimi")]
        [Cooldown(5)]
        public async Task KemonomimiIMG()
        {
            string[] tags = new[] {""};
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("kemonomimi", tags, FileType.Any, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            var embed = new EmbedBuilder();
        
            embed.WithColor(37, 152, 255);
            embed.WithTitle("Kemonomimi!");
            embed.WithDescription(
                $"{Context.User.Mention} here's some kemonomimi pics at your disposal :3");
            embed.WithImageUrl(url);
            embed.WithFooter($"Powered by weeb.sh | ID: {id}");

            await SendMessage(Context, embed.Build());

        }
    }
}
