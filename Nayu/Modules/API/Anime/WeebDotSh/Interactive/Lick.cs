using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.WeebDotSh.Interactive
{
    public class Lick : NayuModule
    {
        [Subject(Categories.Interaction)]
        [Command("lick")]
        [Summary("Displays an image of an anime lick gif")]
        [Remarks("Usage: n!lick <user you want to lick (or can be left empty)> Ex: n!lick @Phytal")]
        [Cooldown(5)]
        public async Task LickUser(IGuildUser user = null)
        {
            string[] tags = {""};
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("lick", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithTitle("Lick!");
                embed.WithDescription(
                    $"{Context.User.Mention} licked themselves... I'll stay out of this for now... \n**(Include a user with your command! Example: n!lick <person you want to lick>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithImageUrl(url);
                embed.WithTitle("Lick!");
                embed.WithDescription($"{Context.User.Username} licked {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
        }
    }
}