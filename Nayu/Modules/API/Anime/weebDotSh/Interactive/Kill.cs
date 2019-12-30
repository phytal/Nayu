using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.weebDotSh.Interactive
{
    public class Kill : NayuModule
    {
        [Command("kill")]
        [Summary("Displays an image of an anime kill gif")]
        [Alias("waste")]
        [Remarks("Usage: n!kill <user you want to kill (or can be left empty)> Ex: n!kill @Phytal")]
        [Cooldown(5)]
        public async Task KillUser(IGuildUser user = null)
        {
            string[] tags = new[] { "" };
            Helpers.WebRequest webReq = new Helpers.WebRequest();
            RandomData result = await webReq.GetTypesAsync("wasted", tags, FileType.Gif, NsfwSearch.False, false);
            string url = result.Url;
            string id = result.Id;
            if (user == null)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithTitle("Wasted!");
                embed.WithDescription(
                    $"{Context.User.Mention} killed themselves... I guess the world is cruel to everyone... \n**(Include a user with your command! Example: n!kill <person you want to kill>)**");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithImageUrl(url);
                embed.WithTitle("Wasted!");
                embed.WithDescription($"{Context.User.Username} killed {user.Mention}!");
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }


    }
}
