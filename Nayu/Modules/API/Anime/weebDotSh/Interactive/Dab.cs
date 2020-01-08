using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.weebDotSh.Interactive
{
    public class Dab : NayuModule
    {
        [Command("dab")]
        [Summary("Displays an image of an anime dab gif")]
        [Remarks("Usage: n!dab <user you want to dab on (or can be left empty)> Ex: n!dab @Phytal")]
        [Cooldown(5)]
        public async Task DabUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("dab", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Dab!");
                embed.WithDescription(
                    $"{Context.User.Mention} dabbed on themselves! Woah where are your friends? \n**(Include a user with your command! Example: n!dab <person you want to dab on>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Dab!");
                embed.WithDescription($"{Context.User.Username} dabbed {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
        }
    }
}
