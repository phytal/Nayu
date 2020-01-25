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
    public class ChannelLocking : NayuModule
    {
        [Subject(AdminCategories.ServerManagement)]
        [Command("lockchannel"), Alias("lc")]
        [Summary("Locks the current channel (users will be unable to send messages, only admins)")]
        [Remarks("n!lc")]
        [Cooldown(5)]
        public async Task LockChannel()
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.ManageChannels)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Manage Channels** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var chnl = Context.Channel as ITextChannel;
            var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == "@everyone");
            var perms = new OverwritePermissions(
                sendMessages: PermValue.Deny
            );
            await chnl.AddPermissionOverwriteAsync(role, perms);

            var embed = MiscHelpers.CreateEmbed(Context, "Channel Locked", $":lock: Locked {Context.Channel.Name}.");
            await SendMessage(Context, embed.Build());
        }

        [Subject(AdminCategories.ServerManagement)]
        [Command("unlockchannel"), Alias("ulc")]
        [Summary("Unlocks the current channel (users can send messages again)")]
        [Remarks("n!ulc")]
        [Cooldown(5)]
        public async Task UnlockChannel()
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.ManageChannels)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Manage Channels** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var chnl = Context.Channel as ITextChannel;
            var role = Context.Guild.Roles.FirstOrDefault(r => r.Name == "@everyone");
            var perms = new OverwritePermissions(
                sendMessages: PermValue.Allow
            );
            await chnl.AddPermissionOverwriteAsync(role, perms);

            var embed = MiscHelpers
                .CreateEmbed(Context, "Channel Unlocked", $":unlock: Unlocked {Context.Channel.Name}.")
                .WithColor(Global.NayuColor);
            await SendMessage(Context, embed.Build());
        }
    }
}
