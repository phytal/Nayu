using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.WeebDotSh.Interactive
{
    public class Smug : NayuModule
    {
        [Command("smug")]
        [Summary("Displays an image of an anime smug gif")]
        [Remarks("Usage: n!smug <user you want to be smug at (or can be left empty)> Ex: n!smug @Phytal")]
        [Cooldown(5)]
        public async Task SmugUser(IGuildUser user = null)
        {
            string[] tags = { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("smug", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Hehe!");
                embed.WithDescription(
                    $"{Context.User.Mention} was smug at themselves, self-infatuation much? \n**(Include a user with your command! Example: n!smug <person you want to be smug at>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Hehe!");
                embed.WithDescription($"{Context.User.Username} is looking all smug at {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
        }
    }
}
