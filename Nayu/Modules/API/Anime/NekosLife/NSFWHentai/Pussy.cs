﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Anime.NekosLife.NSFWHentai
{
    public class Pussy : NayuModule
    {
        [Command("pussy")]
        [Summary("Displays a neko pussy")]
        [Remarks("Ex: n!pussy")]
        [Cooldown(5)]
        public async Task GetRandomPussy()
        {
            var channel = Context.Channel as ITextChannel;
            if (!channel.IsNsfw)
            {
                var nsfwText = $"{Global.ENo} | You need to use this command in a NSFW channel, {Context.User.Username}!";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", nsfwText,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed, timeout: TimeSpan.FromSeconds(5));
            }

            string nekolink = NekosLifeHelper.GetNekoLink("pussy");
            var title = "Randomly generated hentai pussy just for you <3!";
            var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, title);
            await SendMessage(Context, embed);
        }
    }
}