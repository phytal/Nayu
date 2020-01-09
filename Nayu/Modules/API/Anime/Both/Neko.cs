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
    public class Neko : NayuModule
    {
        [Command("neko")]
        [Summary("Displays an random neko :3")]
        [Remarks("Ex: n!neko")]
        [Cooldown(10)]
        public async Task GetRandomNeko()
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string nekolink = NekosLifeHelper.GetNekoLink("neko");
                string description = "Randomly generated neko just for you <3!";

                var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, "Neko!", description);
                await SendMessage(Context, embed);
            }

            if (rand == 2)
            {
                string[] tags = {""};
                WebRequest webReq = new WebRequest();
                RandomData result = await webReq.GetTypesAsync("neko", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                //string id = result.Id;

                string description = "Randomly generated neko just for you <3!";

                var embed = ImageEmbed.GetImageEmbed(url, Source.WeebDotSh, "Neko!", description);
                await SendMessage(Context, embed);
            }
        }
    }
}
