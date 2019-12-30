using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.weebDotSh.Interactive
{
    public class Bang : NayuModule
    {
        [Command("bang")]
        [Summary("Displays an image of an anime bang gif")]
        [Remarks("Usage: n!bang <user you want to shoot (or can be left empty)> Ex: n!bang @Phytal")]
        [Cooldown(5)]
        public async Task ShootUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("bang", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Bang!");
                embed.WithDescription(
                    $"{Context.User.Mention} shot themselves! Oh noes! Here, take a max revive! \n**(Include a user with your command! Example: n!bite <person you want to bite>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Bang!");
                embed.WithDescription($"{Context.User.Username} shot {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }
    }
}
