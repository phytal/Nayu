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
    public class Tickle : NayuModule
    {
        [Subject(Categories.Interaction)]
        [Command("tickle")]
        [Summary("Tickle someone! :3")]
        [Remarks("n!tickle <user you want to tickle (if left empty you will tickle yourself)> Ex: n!tickle @Phytal")]
        [Cooldown(3)]
        public async Task GetRandomTickle(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string nekoLink = NekosLifeHelper.GetNekoLink("tickle");
                string description = user == null
                    ? $"{Context.User.Mention} tickled themselves... I'll stay out of this for now... \n **(Include a user with your command! Example: n!tickle <person you want to tickle>)**"
                    : $"{Context.User.Username} tickled {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(nekoLink, Source.NekosLife, "Tickle!", description);
                await SendMessage(Context, embed);
            }

            if (rand == 2)
            {
                string[] tags = {""};
                WebRequest webReq = new WebRequest();
                RandomData result = await webReq.GetTypesAsync("tickle", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                //string id = result.Id;

                string description = user == null
                    ? $"{Context.User.Mention} tickled themselves, apparently you can get this lonely? \n **(Include a user with your command! Example: n!tickle <person you want to tickle>)**"
                    : $"{Context.User.Username} tickled {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(url, Source.WeebDotSh, "Tickle!", description);
                await SendMessage(Context, embed);
            }
        }
    }
}