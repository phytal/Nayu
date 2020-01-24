using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.NekosLife.NSFWHentai
{
    public class Boobs : NayuModule
    {        
        [Subject(NSFWCategories.Hentai)]
        [Command("boobs")]
        [Alias("tits")]
        [Summary("Displays hentai boobs")]
        [Remarks("Ex: n!boobs")]
        [Cooldown(5)]
        public async Task GetRandomBoobs()
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

            string[] options = {"boobs", "tits"};
            string nekolink = NekosLifeHelper.GetNekoLink(options[Global.Rng.Next(options.Length)]);
            var title = "Randomly generated hentai boobs just for you <3!";
            var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, title);
            await SendMessage(Context, embed);
        }
    }
}
