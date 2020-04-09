using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Helpers;

namespace Nayu.Modules.WelcomeAndLeave
{
    public class LeaveMessages : NayuModule
    {
        [Subject(AdminCategories.LeavingMessages)]
        [Command("leave channel"), Alias("lc")]
        [Summary("Set where you want leave messages to be displayed")]
        [Remarks("n!leave channel <channel where welcome messages are> Ex: n!leave channel #general")]
        public async Task SetIdIntoConfig(SocketGuildChannel channel)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var response = $"Set this guild's leave channel to #{channel}.";
            config.LeaveChannel = channel.Id;
            GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success,
                false);
            await SendMessage(Context, embed);
        }

        [Command("leave add"), Alias("la")]
        [Summary("Announce a leaving user in the set announcement channel" +
                 "with a random message out of the ones defined.")]
        [Remarks("n!la <leave message> Ex: `la Oh noo! <usermention>, left <guildname>...`\n" +
                 "Possible placeholders are: `<usermention>`, `<username>`, `<guildname>`, " +
                 "`<botname>`, `<botdiscriminator>`, `<botmention>`")]
        public async Task AddLeaveMessage([Remainder] string message)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var response = $"Failed to add this leave message...";
            if (guildAcc.LeaveMessages.Contains(message) == false)
            {
                guildAcc.LeaveMessages.Add(message);
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                response = $"Successfully added `{message}` as a leave message!";
            }

            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success,
                false);
            await SendMessage(Context, embed);
        }

        [Subject(AdminCategories.LeavingMessages)]
        [Command("leave remove"), Alias("lr")]
        [Summary("Removes a leave message from the ones available")]
        [Remarks("n!lr <leave message number (shown in n!leave list)> Ex: n!lr 1")]
        public async Task RemoveLeaveMessage(int messageIndex)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var messages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).LeaveMessages;
            var response =
                $"Failed to remove this leave message... Use the number shown in `leave list` next to the `#` sign!";
            if (messages.Count > messageIndex - 1)
            {
                messages.RemoveAt(messageIndex - 1);
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                response = $"Successfully removed message #{messageIndex} as possible Welcome Message!";
            }

            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success,
                false);
            await SendMessage(Context, embed);
        }

        [Subject(AdminCategories.LeavingMessages)]
        [Command("leave list"), Alias("ll")]
        [Summary("Shows all currently set leave messages")]
        [Remarks("n!leave list")]
        public async Task ListLeaveMessages()
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var leaveMessages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).LeaveMessages;
            var embed = new EmbedBuilder().WithTitle(
                "No leave messages set yet... add some if you want a message to be shown if someone leaves.");
            if (leaveMessages.Count > 0) embed.WithTitle("Possible leave messages:");
            embed.WithColor(Global.NayuColor);

            for (var i = 0; i < leaveMessages.Count; i++)
            {
                embed.AddField($"Message #{i + 1}:", leaveMessages[i], true);
            }

            await SendMessage(Context, embed.Build());
        }
    }
}