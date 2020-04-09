using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.WeebDotSh.Interactive
{
    public class Pout : NayuModule
    {
        [Subject(Categories.Interaction)]
        [Command("pout")]
        [Summary("Displays an image of an anime pouting gif")]
        [Remarks("Usage: n!pout <user you want to pout at (or can be left empty)> Ex: n!pout @Phytal")]
        [Cooldown(5)]
        public async Task PoutUser(IGuildUser user = null)
        {
            string[] tags = {""};
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("pout", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithTitle("Pout!");
                embed.WithDescription(
                    $"{Context.User.Mention} is pouting at themselves, you know everyone makes mistakes, right? \n**(Include a user with your command! Example: n!pout <person you want to pout at>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithImageUrl(url);
                embed.WithTitle("Pout!");
                embed.WithDescription($"{Context.User.Username} is pouting at {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
        }
    }
}