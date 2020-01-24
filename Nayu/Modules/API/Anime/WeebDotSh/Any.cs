using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.WeebDotSh
{
    public class Any : NayuModule
    {        
        [Subject(Categories.Images)]
        [Command("anyweeb")]
        [Summary("Displays an image of a sfw anime gif/image, provided with the type")]
        [Remarks("Usage: n!anyweeb <type> Ex: n!anyweeb lick")]
        [Cooldown(5)]
        public async Task AnyUser([Remainder] string type)
        {
            try
            {
                string[] tags = {""};
                Helpers.WebRequest webReq = new Helpers.WebRequest();
                RandomData result = await webReq.GetTypesAsync(type, tags, FileType.Any, NsfwSearch.False, false);
                string url = result.Url;
                string id = result.Id;

                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithTitle("QWERTY!");
                embed.WithDescription($"Looks like {Context.User.Username} is interested in some {type} owo");
                embed.WithImageUrl(url);
                embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                await SendMessage(Context, embed.Build());
            }
            catch
            {
                await SendMessage(Context, null, 
                    "Did you enter a valid type? \nView all types with the `n!ceebtypes` command" +
                    "\nOtherwise did you use the command correctly?\nUsage: n!anyweeb <type> Ex: n!anyweeb lick");
            }
        }
    }
}
