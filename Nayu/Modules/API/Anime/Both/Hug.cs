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
    public class Hug : NayuModule
    {        
        [Subject(Categories.Interaction)]
        [Command("hug")]
        [Summary("Hug someone!")]
        [Remarks("n!hug <user you want to hug (if left empty you will hug yourself)> Ex: n!hug @Phytal")]
        [Cooldown(5)]
        public async Task GetRandomNHug(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string nekolink = NekosLifeHelper.GetNekoLink("hug");
                string description = user == null
                    ? $"{Context.User.Mention} hugged themselves... Aw, don't be sad, you can hug me! \n **(Include a user with your command! Example: n!hug <person you want to hug>)**"
                    : $"❤ **|** {Context.User.Username} hugged {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, "Hug!", description);
                await SendMessage(Context, embed);
            }

            if (rand == 2)
            {
                string[] tags = {""};
                WebRequest webReq = new WebRequest();
                RandomData result = await webReq.GetTypesAsync("hug", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                //string id = result.Id;

                string description = user == null
                    ? $"{Context.User.Mention} hugged themselves, how is that even physically possible? \n **(Include a user with your command! Example: n!hug <person you want to hug>)**"
                    : $"❤ **|**  {Context.User.Username} hugged {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(url, Source.WeebDotSh, "Hug!", description);
                await SendMessage(Context, embed);
            }
        }
    }
}
