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
    public class Pat : NayuModule
    {
        [Command("pat")]
        [Summary("Pat someone! :3")]
        [Remarks("n!pat <user you want to pat (if left empty you will pat yourself)> Ex: n!pat @Phytal")]
        [Cooldown(10)]
        public async Task GetRandomPat(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string nekolink = NekosLifeHelper.GetNekoLink("pat");
                string description = user == null
                    ? $"{Context.User.Mention} patted thin air... You can pat me if you would like! \n **(Include a user with your command! Example: n!pat <person you want to pat>)**"
                    : $"{Context.User.Username} patted {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, "Pat!", description);
                await SendMessage(Context, embed);
            }

            if (rand == 2)
            {
                string[] tags = {""};
                WebRequest webReq = new WebRequest();
                RandomData result = await webReq.GetTypesAsync("pat", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                //string id = result.Id;

                string description = user == null
                    ? $"{Context.User.Mention} patted themselves, how lonely can you be? \n **(Include a user with your command! Example: n!pat <person you want to pat>)**"
                    : $"{Context.User.Username} patted {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(url, Source.WeebDotSh, "Pat!", description);
                await SendMessage(Context, embed);
            }
        }
    }
}
