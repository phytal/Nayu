using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;
using Nayu.Preconditions;

namespace Nayu.Modules.Management.Commands
{
    public class Clear : NayuModule
    {

        [Command("clear")]
        [Alias("purge", "delete")]
        [Summary("Purges A User's Last 100 Messages")]
        [Remarks("n!clear <user whose messages you want to clear> Ex: n!clear @Phytal")]
        [Cooldown(5)]
        public async Task ClearCMD(SocketGuildUser user)
        {
            if (user.GuildPermissions.ManageMessages)
            {
                var messages = await Context.Channel.GetMessagesAsync(100).FlattenAsync();
                var result = messages.Where(x => x.Author.Id == user.Id && x.CreatedAt >= DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(14)));
                if (Context.Channel is ITextChannel channel) await channel.DeleteMessagesAsync(result);

            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Messages Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("clear")]
        [Alias("purge", "delete")]
        [Summary("Clears *x* amount of messages")]
        [Remarks("n!clear <amount of messages you want to clear> Ex: n!clear 10")]
        [Cooldown(5)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        public async Task Purge([Remainder] int num = 0)
        {
            if (Context.Channel is ITextChannel text)
            {
                var user = Context.User as SocketGuildUser;
                if (user.GuildPermissions.ManageMessages)
                {
                    if (num <= 100)
                    {
                        var messagesToDelete = await Context.Channel.GetMessagesAsync(num + 1).FlattenAsync();
                        if (Context.Channel is ITextChannel channel) await channel.DeleteMessagesAsync(messagesToDelete);
                        if (num == 1) await ReplyAndDeleteAsync(":white_check_mark:  | Deleted 1 message.");
                        else await ReplyAndDeleteAsync(":white_check_mark:  | Cleared " + num + " messages.", timeout: TimeSpan.FromSeconds(5));
                    }
                    else
                    {
                        var embed = new EmbedBuilder();
                        embed.WithColor(37, 152, 255);
                        embed.Title = ":x:  | You cannot delete more than 100 messages at once!";
                        await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
                    }
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $":x:  | You need the Manange Messages Permission to do that {Context.User.Username}";
                    await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
                }
            }
        }
    }
}
