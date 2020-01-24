using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.NekosLife.NSFWHentai
{
    public class Les : NayuModule
    {        
        [Subject(NSFWCategories.Hentai)]
        [Command("les")]
        [Summary("Displays a les")]
        [Remarks("Ex: n!les")]
        [Cooldown(5)]
        public async Task GetRandomLes()
        {
            var channel = Context.Channel as ITextChannel;
            if (!channel.IsNsfw)
            {
                var nsfwText = $"{Global.ENo} **|** You need to use this command in a NSFW channel, {Context.User.Username}!";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", nsfwText,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            string nekolink = NekosLifeHelper.GetNekoLink("les");
            var title = "Randomly generated lesbian hentai just for you <3!";
            var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, title);
            await SendMessage(Context, embed);
        }
    }
}
