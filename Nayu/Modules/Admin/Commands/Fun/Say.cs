using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Fun
{
    public class Say : NayuModule
    {
        [Command("say")]
        [Summary("Lets you speak for the bot anonymously")]
        [Remarks("n!say <your message> Ex: n!say whats up my doots")]
        [Cooldown(5)]
        public async Task SayCMD([Remainder] string input)
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            if (user.GuildPermissions.Administrator)
            {
                if (config.MassPingChecks == true)
                {
                    if (input.Contains("@everyone") || input.Contains("@here")) return;
                }

                var messagesToDelete = await Context.Channel.GetMessagesAsync(1).FlattenAsync();
                if (Context.Channel is ITextChannel text) await text.DeleteMessagesAsync(messagesToDelete);
                //input.Replace("@everyone", "@\u200beveryone").Replace("@here", "@\u200bhere");
                await Context.Channel.SendMessageAsync(input);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
