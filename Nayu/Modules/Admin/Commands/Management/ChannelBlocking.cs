using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Management
{
    public class ChannelBlocking : NayuModule
    {
        [Subject(AdminCategories.ServerManagement)]
        [Command("blockChannel"), Alias("bc")]
        [Summary("Blocks the current channel (users will be unable to use commands, only admins)")]
        [Remarks("n!bc")]
        [Cooldown(5)]
        public async Task BlockChannel()
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

            var config = BotAccounts.GetAccount();
            config.BlockedChannels.Add(Context.Channel.Id, Context.Guild.Id);
            BotAccounts.SaveAccounts();

            var embed = MiscHelpers.CreateEmbed(Context, "Channel Blocked", $":lock: Blocked {Context.Channel.Name}.");
            await SendMessage(Context, embed.Build());
        }

        [Subject(AdminCategories.ServerManagement)]
        [Command("unblockChannel"), Alias("ubc")]
        [Summary("Unblocks the current channel (users can use commands again)")]
        [Remarks("n!ubc")]
        [Cooldown(5)]
        public async Task UnblockChannel()
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

            var config = BotAccounts.GetAccount();
            config.BlockedChannels.Remove(Context.Channel.Id);
            BotAccounts.SaveAccounts();

            var embed = MiscHelpers
                .CreateEmbed(Context, "Channel Unblocked", $":unlock: Unblocked {Context.Channel.Name}.")
                .WithColor(Constants.DefaultColor);
            await SendMessage(Context, embed.Build());
        }
    }
}