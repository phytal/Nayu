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
    public class Cuddle : NayuModule
    {
        [Command("cuddle")]
        [Summary("Displays an random cuddle picture!")]
        [Remarks("n!cuddle <user you want to cuddle (if left empty you will cuddle yourself)> Ex: n!cuddle @Phytal")]
        [Cooldown(10)]
        public async Task GetRandomCuddle(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string nekolink = NekosLifeHelper.GetNekoLink("cuddle");
                string description = user == null
                    ? $"{Context.User.Mention} cuddled with themselves... Maybe you can cuddle with a friend? \n **(Include a user with your command! Example: n!cuddle <person you want to cuddle with>)**"
                    : $"{Context.User.Username} cuddled with {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, "Cuddle!", description);
                await SendMessage(Context, embed);
            }

            if (rand == 2)
            {
                string[] tags = {""};
                WebRequest webReq = new WebRequest();
                RandomData result = await webReq.GetTypesAsync("cuddle", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                //string id = result.Id;

                string description = user == null
                    ? $"{Context.User.Mention} cuddled themselves, that's some intense self-love right there.. \n **(Include a user with your command! Example: n!cuddle <person you want to cuddle>)**"
                    : $"{Context.User.Username} cuddled with {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(url, Source.WeebDotSh, "Cuddle!", description);
                await SendMessage(Context, embed);
            }
        }
    }
}
