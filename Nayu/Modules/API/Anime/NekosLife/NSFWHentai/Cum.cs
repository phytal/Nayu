using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.NekosLife.NSFWHentai
{
    public class Cum : NayuModule
    {
        [Subject(NSFWCategories.Hentai)]
        [Command("cum")]
        [Summary("Displays a hentai cum")]
        [Remarks("Ex: n!cum")]
        [Cooldown(5)]
        public async Task GetRandomCum()
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

            string nekoLink = NekosLifeHelper.GetNekoLink("cum");
            var title = "Randomly generated hentai cum just for you <3!";
            var embed = ImageEmbed.GetImageEmbed(nekoLink, Source.NekosLife, title);
            await SendMessage(Context, embed);
        }
    }
}