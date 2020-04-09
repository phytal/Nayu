using System;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Modules.API.Anime.NekosLife;
using Nayu.Preconditions;
using Newtonsoft.Json;
using WebRequest = Nayu.Modules.API.Anime.WeebDotSh.Helpers.WebRequest;

namespace Nayu.Modules.API.Anime.Both
{
    public class Lewd : NayuModule
    {
        [Subject(NSFWCategories.Neko)]
        [Command("lewd")]
        [Summary("Displays a lewd image (nsfw)")]
        [Remarks("Ex: n!lewd")]
        [Cooldown(5)]
        public async Task GetRandomNekoLewd()
        {
            var channel = Context.Channel as ITextChannel;
            if (!channel.IsNsfw)
            {
                var nsfwText =
                    $"{Global.ENo} **|** You need to use this command in a NSFW channel, {Context.User.Username}!";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", nsfwText,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            int rand = Global.Rng.Next(1, 3);
            if (rand == 1)
            {
                string nekolink = NekosLifeHelper.GetNekoLink("lewd");
                string description = "Randomly generated lewd neko just for you <3!";

                var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, description);
                await SendMessage(Context, embed);
            }

            if (rand == 2)
            {
                string[] tags = {""};
                WebRequest webReq = new WebRequest();
                RandomData result = await webReq.GetTypesAsync("neko", tags, FileType.Any, NsfwSearch.Only, false);
                string url = result.Url;
                //string id = result.Id;

                string description = "Randomly generated lewd neko just for you <3!";

                var embed = ImageEmbed.GetImageEmbed(url, Source.WeebDotSh, description);
                await SendMessage(Context, embed);
            }
        }
    }
}