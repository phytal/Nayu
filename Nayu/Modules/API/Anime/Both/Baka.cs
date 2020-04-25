using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Modules.API.Anime.NekosLife;
using Nayu.Preconditions;
using Newtonsoft.Json;
using WebRequest = Nayu.Modules.API.Anime.WeebDotSh.Helpers.WebRequest;

namespace Nayu.Modules.API.Anime.Both
{
    public class Baka : NayuModule
    {
        [Subject(Categories.Interaction)]
        [Command("baka")]
        [Summary("Displays an image of an anime baka gif")]
        [Remarks("Usage: n!baka <user you want to call a baka (or can be left empty)> Ex: n!baka @Phytal")]
        [Cooldown(5)]
        public async Task bakaUser(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string nekoLink = NekosLifeHelper.GetNekoLink("baka");
                string description = user == null
                    ? $"{Context.User.Mention} called themselves a baka! I kind of agree with that.. \n**(Include a user with your command! Example: n!baka <person you want to call a baka>)**"
                    : $"{Context.User.Username} cuddled with {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(nekoLink, Source.NekosLife, "Baka!", description);
                await SendMessage(Context, embed);
            }

            if (rand == 2)
            {
                string[] tags = {""};
                WebRequest webReq = new WebRequest();
                RandomData result = await webReq.GetTypesAsync("baka", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                //string id = result.Id;

                string description = user == null
                    ? $"{Context.User.Mention}, baka desuka? \n **(Include a user with your command! Example: n!baka <person you want to call a baka>)**"
                    : $"{Context.User.Username} called {user.Mention} a baka!";

                var embed = ImageEmbed.GetImageEmbed(url, Source.WeebDotSh, "Baka!", description);
                await SendMessage(Context, embed);
            }
        }
    }
}