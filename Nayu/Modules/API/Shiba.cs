using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.API
{
    public class Shiba : NayuModule
    {
        [Subject(Categories.Images)]
        [Command("shiba")]
        [Alias("shibe")]
        [Summary("Sends an image of a Shiba Inu :3")]
        [Remarks("Ex: n!shiba")]
        [Cooldown(5)]
        public async Task GetRandomShiba()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("http://shibe.online/api/shibes?count=1&urls=true&httpsUrls=false");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string link = dataObject[0].ToString();

            var title = "🐶 **|** Here's a Shiba!";
            var embed = ImageEmbed.GetImageEmbed(link, Source.None, title);
            await SendMessage(Context, embed);
        }
    }
}