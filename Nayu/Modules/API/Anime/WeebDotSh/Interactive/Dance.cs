using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.WeebDotSh.Interactive
{
    public class Dance : NayuModule
    {        
        [Subject(Categories.Interaction)]
        [Command("dance")]
        [Summary("Displays an image of an anime dance gif")]
        [Remarks("Usage: n!dance <user you want to dance with (or can be left empty)> Ex: n!dance @Phytal")]
        [Cooldown(5)]
        public async Task DanceUser(IGuildUser user = null)
        {
            string[] tags = { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("dance", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Dance!");
                embed.WithDescription(
                    $"{Context.User.Mention} danced with themselves.. *I'm just going to whip out my camera...*? \n**(Include a user with your command! Example: n!dance <person you want to dance with>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Dance!");
                embed.WithDescription($"{Context.User.Username} danced with {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
        }
    }
}
