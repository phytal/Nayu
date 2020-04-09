using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.WeebDotSh.Interactive
{
    public class Punch : NayuModule
    {
        [Subject(Categories.Interaction)]
        [Command("punch")]
        [Summary("Displays an image of an anime punch gif")]
        [Remarks("Usage: n!punch <user you want to punch (or can be left empty)> Ex: n!punch @Phytal")]
        [Cooldown(5)]
        public async Task PunchUser(IGuildUser user = null)
        {
            string[] tags = {""};
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("punch", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithTitle("Punch!");
                embed.WithDescription(
                    $"{Context.User.Mention} punched themselves, that must have knocked a bit of sense into you. \n**(Include a user with your command! Example: n!punch <person you want to punch>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithImageUrl(url);
                embed.WithTitle("Punch!");
                embed.WithDescription($"{Context.User.Username} punched {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
        }
    }
}