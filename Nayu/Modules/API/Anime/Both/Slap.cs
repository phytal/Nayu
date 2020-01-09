using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;
using Nayu.Helpers;
using Nayu.Modules.API.Anime.NekosLife;
using WebRequest = Nayu.Modules.API.Anime.WeebDotSh.Helpers.WebRequest;

namespace Nayu.Modules.API.Anime.Both
{
    public class Slap : NayuModule
    {
        [Command("slap")]
        [Summary("Slap someone!")]
        [Remarks("n!slap <user you want to slap (if left empty you will slap yourself)> Ex: n!slap @Phytal")]
        [Cooldown(3)]
        public async Task GetRandomSlap(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string nekolink = NekosLifeHelper.GetNekoLink("slap");
                string description = user == null
                    ? $"{Context.User.Mention} slapped themselves... Don't do this to yourself! \n **(Include a user with your command! Example: n!slap <person you want to slap>)**"
                    : $"{Context.User.Username} slapped {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, "Slap!", description);
                await SendMessage(Context, embed);
            }

            if (rand == 2)
            {
                string[] tags = {""};
                WebRequest webReq = new WebRequest();
                RandomData result = await webReq.GetTypesAsync("slap", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                //string id = result.Id;

                string description = user == null
                    ? $"{Context.User.Mention} slapped themselves, masochism much? \n **(Include a user with your command! Example: n!slap <person you want to slap>)**"
                    : $"{Context.User.Username} slapped {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(url, Source.WeebDotSh, "Slap!", description);
                await SendMessage(Context, embed);
            }
        }
    }
}
