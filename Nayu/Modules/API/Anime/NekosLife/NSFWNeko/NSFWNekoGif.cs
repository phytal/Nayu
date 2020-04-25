using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.NekosLife.NSFWNeko
{
    public class NSFWNekoGif : NayuModule
    {
        [Subject(NSFWCategories.Neko)]
        [Command("nekonsfwgif")]
        [Summary("Displays a nsfw neko gif")]
        [Remarks("Ex: n!nekonsfwgif")]
        [Cooldown(5)]
        public async Task GetRandomNSFWNekoGif()
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

            string nekoLink = NekosLifeHelper.GetNekoLink("nsfw_neko_gif");
            var title = "Randomly generated nsfw neko just for you <3!";
            var embed = ImageEmbed.GetImageEmbed(nekoLink, Source.NekosLife, title);
            await SendMessage(Context, embed);
        }
    }
}