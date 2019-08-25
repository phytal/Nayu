using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Discord;
using Nayu.Preconditions;
using Nayu.Core.Modules;

namespace Nayu.Modules.API
{
    public class FoxGirl : NayuModule
    {
        [Command("foxgirl")]
        [Summary("Displays an random fox girl :3")]
        [Remarks("Ex: n!neko")]
        [Cooldown(10)]
        public async Task GetRandomFoxGirl()
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://nekos.life/api/v2/img/fox_girl");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string nekolink = dataObject.url.ToString();

            var embed = new EmbedBuilder();
            embed.WithTitle("Randomly generated fox girl just for you <3!");
            embed.WithImageUrl(nekolink);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }
    }
}