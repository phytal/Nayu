using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Management
{
    public class Clear : NayuModule
    {
        [Subject(AdminCategories.ServerManagement)]
        [Command("clear")]
        [Alias("purge", "delete")]
        [Summary("Purges A User's Last 100 Messages")]
        [Remarks("n!clear <user whose messages you want to clear> Ex: n!clear @Phytal")]
        [Cooldown(5)]
        public async Task ClearCMD(SocketGuildUser user)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.ManageMessages)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Manage Messages** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var messages = await Context.Channel.GetMessagesAsync(100).FlattenAsync();
            var result = messages.Where(x =>
                x.Author.Id == user.Id && x.CreatedAt >= DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(14)));
            if (Context.Channel is ITextChannel channel) await channel.DeleteMessagesAsync(result);
        }

        [Subject(AdminCategories.ServerManagement)]
        [Command("clear")]
        [Alias("purge", "delete")]
        [Summary("Clears *x* amount of messages")]
        [Remarks("n!clear <amount of messages you want to clear> Ex: n!clear 10")]
        [Cooldown(5)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        public async Task Purge([Remainder] int num = 0)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.ManageMessages)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Manage Messages** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            if (num <= 100)
            {
                var messagesToDelete = await Context.Channel.GetMessagesAsync(num + 1).FlattenAsync();
                if (Context.Channel is ITextChannel channel) await channel.DeleteMessagesAsync(messagesToDelete);
                if (num == 1) await ReplyAndDeleteAsync("✅  **|** Deleted 1 message.");
                else await ReplyAndDeleteAsync("✅  **|** Cleared " + num + " messages.", timeout: TimeSpan.FromSeconds(5));
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.Title = "{Global.ENo} **|** You cannot delete more than 100 messages at once!";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
