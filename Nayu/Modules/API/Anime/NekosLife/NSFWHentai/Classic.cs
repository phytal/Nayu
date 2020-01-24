using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.NekosLife.NSFWHentai
{
    public class Classic : NayuModule
    {        
        [Subject(NSFWCategories.Hentai)]
        [Command("classic")]
        [Summary("Displays classic hentai")]
        [Remarks("Ex: n!classic")]
        [Cooldown(5)]
        public async Task GetRandomClassic()
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

            string nekolink = NekosLifeHelper.GetNekoLink("classic");
            var title = "Randomly generated classic hentai just for you <3!";
            var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, title);
            await SendMessage(Context, embed);
        }
    }
}

