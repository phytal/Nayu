using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Configuration;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.WeebDotSh.Interactive
{
    public class Bite : NayuModule
    {        
        [Subject(Categories.Interaction)]
        [Command("bite")]
        [Summary("Displays an image of an anime bite gif")]
        [Remarks("Usage: n!bite <user you want to bite (or can be left empty)> Ex: n!bite @Phytal")]
        [Cooldown(5)]
        public async Task BiteUser(IGuildUser user = null)
        {
            string[] tags = { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("bite", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithTitle("Bite!");
                embed.WithDescription(
                    $"{Context.User.Mention} bit themselves, did that taste good? \n**(Include a user with your command! Example: n!bite <person you want to bite>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithImageUrl(url);
                embed.WithTitle("Bite!");
                embed.WithDescription($"{Context.User.Username} bit {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
        }
    }
}
