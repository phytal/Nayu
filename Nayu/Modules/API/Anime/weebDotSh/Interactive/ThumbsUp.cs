using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.weebDotSh.Interactive
{
    public class ThumbsUp : NayuModule
    {
        [Command("thumbsUp")]
        [Summary("Displays an image of an anime thumbs-up gif")]
        [Remarks("Usage: n!thumbsUp <user you want to thumbs-up at (or can be left empty)> Ex: n!thumbsUp @Phytal")]
        [Cooldown(5)]
        public async Task ThumbsUpUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("thumbsup", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Yoku yatta!");
                embed.WithDescription(
                    $"{Context.User.Mention} gave themselves a thumbs up, high self-esteem is good you know. \n**(Include a user with your command! Example: n!thumbsUp <person you want to give a thumbs-up>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Yoku yatta!");
                embed.WithDescription($"{Context.User.Username} gave {user.Mention} a thumbs up!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
        }
    }
}
