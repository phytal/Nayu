using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.weebDotSh.Interactive
{
    public class WaifuInsult : NayuModule
    {
        [Command("waifuInsult")]
        [Summary("Displays an image of an anime waifu insult gif")]
        [Alias("insultWaifu")]
        [Remarks("Usage: n!caifuInsult <user you want to insult (or can be left empty)> Ex: n!caifuInsult @Phytal")]
        [Cooldown(5)]
        public async Task WaifuInsultUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("waifu_insult", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Insult!");
                embed.WithDescription(
                    $"{Context.User.Mention} insulted their own waifu! Wait, was that me? That's it, {Context.User.Username}, we're not friends anymore. \n**(Include a user with your command! Example: n!caifuInsult <person you want to insult>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Insult!");
                embed.WithDescription($"{Context.User.Username} insulted {user.Mention}'s waifu!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed);
            }
        }
    }
}
