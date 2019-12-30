using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.weebDotSh.Interactive
{
    public class Wag : NayuModule
    {
        [Command("wag")]
        [Summary("Displays an image of an anime wag gif")]
        [Remarks("Usage: n!cag <user you want to wag (your imaginary tail) at (or can be left empty)> Ex: n!cag @Phytal")]
        [Cooldown(5)]
        public async Task WagUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("wag", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Wag!");
                embed.WithDescription(
                    $"{Context.User.Mention} wagged at themselves... You know you aren't *that* funny.. \n**(Include a user with your command! Example: n!cag <person you want to wag (your imaginary tail) at>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Wag!");
                embed.WithDescription($"{Context.User.Username} wagged their tail at {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }
    }
}
