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
using Weeb.net;
using Weeb.net.Data;
using Nayu.Preconditions;
using Nayu.Core.Modules;

namespace Nayu.Modules.API.Anime
{
    public class Pat : NayuModule
    {
        [Command("pat")]
        [Summary("Pat someone! :3")]
        [Remarks("n!pat <user you want to pat (if left empty you will pat yourself)> Ex: n!pat @Phytal")]
        [Cooldown(10)]
        public async Task GetRandomNekoPat(IGuildUser user = null)
        {
            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string json = "";
                using (WebClient client = new WebClient())
                {
                    json = client.DownloadString("https://nekos.life/api/v2/img/pat");
                }

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string nekolink = dataObject.url.ToString();

                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Pat!");
                    embed.WithDescription(
                        $"{Context.User.Mention} patted thin air... You can pat me if you would like! \n **(Include a user with your command! Example: n!pat <person you want to pat>)**");
                    embed.WithImageUrl(nekolink);
                    embed.WithFooter($"Powered by nekos.life");

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithImageUrl(nekolink);
                    embed.WithTitle("Pat!");
                    embed.WithDescription($"{Context.User.Username} patted {user.Mention}!");
                    embed.WithFooter($"Powered by nekos.life");

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
            }

            if (rand == 2)
            {
                string[] tags = new[] { "" };
                weebDotSh.Helpers.WebRequest webReq = new weebDotSh.Helpers.WebRequest();
                RandomData result = await webReq.GetTypesAsync("pat", tags, FileType.Gif, NsfwSearch.False, false);
                string url = result.Url;
                string id = result.Id;
                if (user == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Pat!");
                    embed.WithDescription(
                        $"{Context.User.Mention} patted themselves, how lonely can you be? \n **(Include a user with your command! Example: n!pat <person you want to pat>)**");
                    embed.WithImageUrl(url);
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithImageUrl(url);
                    embed.WithTitle("Pat!");
                    embed.WithDescription($"{Context.User.Username} patted {user.Mention}!");
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
            }
        }
    }
}
