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
    public class Feed : NayuModule
    {
        [Command("feed")]
        [Summary("Feed someone!")]
        [Remarks("n!feed <user you want to feed (if left empty you will feed yourself)> Ex: n!feed @Phytal")]
        [Cooldown(10)]
        public async Task GetRandomFeed(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string nekolink = NekosLifeHelper.GetNekoLink("feed");
                string description = user == null
                    ? $"{Context.User.Mention} fed themselves... Let's hope they don't get fat... \n **(Include a user with your command! Example: n!feed <person you want to feed>)**"
                    : $"{Context.User.Username} fed {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, "Munch!", description);
                await SendMessage(Context, embed);
            }

            if (rand == 2)
            {
                string[] tags = {""};
                WebRequest webReq = new WebRequest();
                RandomData result = await webReq.GetTypesAsync("feed", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                //string id = result.Id;

                string description = user == null
                    ? $"{Context.User.Mention} fed themselves, I think that's a bit too normal. \n **(Include a user with your command! Example: n!feed <person you want to feed>)**"
                    : $"{Context.User.Username} fed {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(url, Source.WeebDotSh, "Munch!", description);
                await SendMessage(Context, embed);
            }
        }
    }
}
