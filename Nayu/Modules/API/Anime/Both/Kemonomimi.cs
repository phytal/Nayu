using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Modules.API.Anime.NekosLife;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.WeebDotSh
{
    public class Kemonomimi : NayuModule
    {
        [Command("kemonomimi")]
        [Summary("Displays an image of an anime kemonomimi gif")]
        [Remarks("Ex: n!kemonomimi")]
        [Cooldown(5)]
        public async Task KemonomimiIMG()
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string nekolink = NekosLifeHelper.GetNekoLink("kemonomimi");
                string description = "Randomly generated kemonomimi just for you <3!";

                var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, description);
                await SendMessage(Context, embed);
            }

            if (rand == 2)
            {
                string[] tags = new[] {""};
                Helpers.WebRequest webReq = new Helpers.WebRequest();
                RandomData result =
                    await webReq.GetTypesAsync("kemonomimi", tags, FileType.Any, NsfwSearch.False, false);
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
}
