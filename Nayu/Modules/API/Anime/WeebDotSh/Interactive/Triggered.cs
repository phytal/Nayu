using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.WeebDotSh.Interactive
{
    public class Triggered : NayuModule
    {        
        [Subject(Categories.Interaction)]
        [Command("triggered")]
        [Summary("Displays an image of an anime triggered gif")]
        [Remarks("Usage: n!triggered <user you want to be triggered at (or can be left empty)> Ex: n!triggered @Phytal")]
        [Cooldown(5)]
        public async Task TriggeredUser(IGuildUser user = null)
        {
            string[] tags = { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("triggered", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithTitle("Triggered!");
                embed.WithDescription(
                    $"{Context.User.Mention} is triggered at themselves! What happened? May I be of assistance? \n**(Include a user with your command! Example: n!triggered <person you want to be triggered at>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithImageUrl(url);
                embed.WithTitle("Triggered!");
                embed.WithDescription($"{Context.User.Username} is triggered {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
        }
    }
}
