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
    public class Kiss : NayuModule
    {
        [Subject(Categories.Interaction)]
        [Command("kiss")]
        [Summary("Kiss someone! :3")]
        [Remarks("n!kiss <user you want to kiss (if left empty you will kiss yourself)> Ex: n!kiss @Phytal")]
        [Cooldown(5)]
        public async Task GetRandomKiss(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string nekoLink = NekosLifeHelper.GetNekoLink("kiss");
                string description = user == null
                    ? $"{Context.User.Mention} you can't really kiss yourself... Don't worry, how about a kiss from me?... \n **(Include a user with your command! Example: n!kiss <person you want to kiss>)**"
                    : $"❤ **|**  {Context.User.Username} kissed {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(nekoLink, Source.NekosLife, "Kiss!", description);
                await SendMessage(Context, embed);
            }

            if (rand == 2)
            {
                string[] tags = {""};
                WebRequest webReq = new WebRequest();
                RandomData result = await webReq.GetTypesAsync("kiss", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                //string id = result.Id;

                string description = user == null
                    ? $"{Context.User.Mention} kissed themselves, welcome to quantum mechanics class everybody. \n **(Include a user with your command! Example: n!kiss <kiss you want to hug>)**"
                    : $"❤ **|**  {Context.User.Username} kissed {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(url, Source.WeebDotSh, "Kiss!", description);
                await SendMessage(Context, embed);
            }
        }
    }
}