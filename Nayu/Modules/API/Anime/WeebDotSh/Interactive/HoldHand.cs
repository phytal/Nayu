using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.WeebDotSh.Interactive
{
    public class HoldHand : NayuModule
    {
        [Command("holdHand")]
        [Summary("Displays an image of an anime hand holding gif")]
        [Remarks("Usage: n!holdHand <user you want to holdHand (or can be left empty)> Ex: n!holdHand @Phytal")]
        [Cooldown(5)]
        public async Task HandHoldUser(IGuildUser user = null)
        {
            string[] tags = { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("handholding", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("kawaii-ne!");
                embed.WithDescription(
                    $"{Context.User.Mention} held their own hand! Hey, I thought you loved ***me**, {Context.User.Username}! \n**(Include a user with your command! Example: n!holdHand <person you want to hold hands with>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("kawaii-ne!");
                embed.WithDescription($"{Context.User.Username} is holding hands with {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
        }
    }
}
