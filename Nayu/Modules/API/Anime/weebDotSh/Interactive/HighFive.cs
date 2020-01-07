using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.weebDotSh.Interactive
{
    public class HighFive : NayuModule
    {
        [Command("highfive")]
        [Summary("Displays an image of an anime highfive gif")]
        [Remarks("Usage: n!highfive <user you want to highfive (or can be left empty)> Ex: n!highfive @Phytal")]
        [Cooldown(5)]
        public async Task HighFiveUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("highfive", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("High Five!");
                embed.WithDescription(
                    $"{Context.User.Mention} highfived themselves.. that was a pretty nice clap, {Context.User.Username}. \n**(Include a user with your command! Example: n!highfive <person you want to highfive>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("High Five!");
                embed.WithDescription($"{Context.User.Username} high fived {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed);
            }
        }
    }
}
