using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.weebDotSh.Interactive
{
    public class Cry : NayuModule
    {
        [Command("cry")]
        [Summary("Displays an image of an anime cry gif")]
        [Remarks("Usage: n!cry <user you want to cry at (or can be left empty)> Ex: n!cry @Phytal")]
        [Cooldown(5)]
        public async Task CryUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("cry", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Waaahhh!");
                embed.WithDescription(
                    $"{Context.User.Mention} cried for no reason... awww, don't worry I'll always be here for you! \n**(Include a user with your command! Example: n!bite <person you want to bite>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Waaahhh!");
                embed.WithDescription($"{Context.User.Username} cried at {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed);
            }
        }
    }
}
