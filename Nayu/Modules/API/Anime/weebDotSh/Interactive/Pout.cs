using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.weebDotSh.Interactive
{
    public class Pout : NayuModule
    {
        [Command("pout")]
        [Summary("Displays an image of an anime pouting gif")]
        [Remarks("Usage: n!pout <user you want to pout at (or can be left empty)> Ex: n!pout @Phytal")]
        [Cooldown(5)]
        public async Task BiteUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("pout", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
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
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Pout!");
                embed.WithDescription($"{Context.User.Username} is pouting at {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
        }
    }
}
