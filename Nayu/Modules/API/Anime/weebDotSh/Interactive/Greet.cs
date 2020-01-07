using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.weebDotSh.Interactive
{
    public class Greet : NayuModule
    {
        [Command("greet")]
        [Summary("Displays an image of an anime greet gif")]
        [Remarks("Usage: n!greet <user you want to greet (or can be left empty)> Ex: n!greet @Phytal")]
        [Cooldown(5)]
        public async Task GreetUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("greet", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Konichiwa!");
                embed.WithDescription(
                    $"{Context.User.Mention} greeted themselves! You know I'm here.. right? \n**(Include a user with your command! Example: n!greet <person you want to greet>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Konichiwa!");
                embed.WithDescription($"{Context.User.Username} greeted {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed);
            }
        }
    }
}
