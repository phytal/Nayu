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
    public class Poke : NayuModule
    {
        [Subject(Categories.Interaction)]
        [Command("poke")]
        [Summary("Poke someone! :3")]
        [Remarks("n!poke <user you want to poke (if left empty you will poke yourself)> Ex: n!poke @Phytal")]
        [Cooldown(5)]
        public async Task GetRandomPoke(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string nekoLink = NekosLifeHelper.GetNekoLink("poke");
                string description = user == null
                    ? $"{Context.User.Mention} poked themselves... I guess you can poke yourself if you're lonely... \n **(Include a user with your command! Example: n!poke <person you want to poke>)**"
                    : $"{Context.User.Username} poked {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(nekoLink, Source.NekosLife, "Poke!", description);
                await SendMessage(Context, embed);
            }

            if (rand == 2)
            {
                string[] tags = {""};
                WebRequest webReq = new WebRequest();
                RandomData result = await webReq.GetTypesAsync("poke", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                //string id = result.Id;

                string description = user == null
                    ? $"{Context.User.Mention} poked themselves, yes, I know the human body is  *i n t e r e s t i n g*  :3. \n **(Include a user with your command! Example: n!poke <person you want to poke>)**"
                    : $"{Context.User.Username} poked {user.Mention}!";

                var embed = ImageEmbed.GetImageEmbed(url, Source.WeebDotSh, "Poke!", description);
                await SendMessage(Context, embed);
            }
        }
    }
}