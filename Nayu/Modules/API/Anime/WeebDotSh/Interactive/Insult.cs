using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.WeebDotSh.Interactive
{
    public class Insult : NayuModule
    {        
        [Subject(Categories.Interaction)]
        [Command("insult")]
        [Summary("Displays an image of an anime insult gif")]
        [Remarks("Usage: n!insult <user you want to insult (or can be left empty)> Ex: n!insult @Phytal")]
        [Cooldown(5)]
        public async Task InsultUser(IGuildUser user = null)
        {
            string[] tags = { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("insult", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Insult!");
                embed.WithDescription(
                    $"{Context.User.Mention} insulted themselves! Why the pessimism? \n**(Include a user with your command! Example: n!insult <person you want to insult>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Insult!");
                embed.WithDescription($"{Context.User.Username} insulted {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
        }
    }
}
