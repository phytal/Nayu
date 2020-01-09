using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.NekosLife.NSFWHentai
{
    public class Erofeet : NayuModule
    {
        [Command("eroFeet")]
        [Summary("Displays a ero feet")]
        [Remarks("Ex: n!eroFeet")]
        [Cooldown(5)]
        public async Task GetRandomErofeet()
        {
            var channel = Context.Channel as ITextChannel;
            if (!channel.IsNsfw)
            {
                var nsfwText = $"{Global.ENo} | You need to use this command in a NSFW channel, {Context.User.Username}!";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", nsfwText,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed, timeout: TimeSpan.FromSeconds(5));
            }

            string nekolink = NekosLifeHelper.GetNekoLink("erofeet");
            var title = "Randomly generated ero feet just for you <3!";
            var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, title);
            await SendMessage(Context, embed);
        }
    }
}
