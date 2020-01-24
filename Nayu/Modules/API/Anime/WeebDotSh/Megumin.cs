using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.WeebDotSh
{
    public class Megumin : NayuModule
    {        
        [Subject(Categories.Images)]
        [Command("megumin")]
        [Summary("Displays a Megumin image/gif")]
        [Remarks("Ex: n!megumin")]
        [Cooldown(5)]
        public async Task LewdIMG()
        {
            string[] tags = { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("megumin", tags, FileType.Any, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            var embed = new EmbedBuilder();

            embed.WithColor(Global.NayuColor);
            embed.WithTitle("Explosion!");
            embed.WithDescription(
                $"{Context.User.Mention} here's some megumin pics at your disposal :3");
            embed.WithImageUrl(url);
            embed.WithFooter($"Powered by weeb.sh | ID: {id}");

            await SendMessage(Context, embed.Build());

        }
    }
}
