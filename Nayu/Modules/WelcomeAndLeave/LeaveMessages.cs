using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;

namespace Nayu.Modules.WelcomeAndLeave
{
    public class LeaveMessages : NayuModule
    {
        [Command("leave channel"), Alias("lc")]
        [Summary("Set where you want leave messages to be displayed")]
        [Remarks("n!leave channel <channel where welcome messages are> Ex: n!leave channel #general")]
        public async Task SetIdIntoConfig(SocketGuildChannel channel)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (guildUser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithDescription($"Set this guild's leave channel to #{channel}.");
                config.LeaveChannel = channel.Id;
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                await SendMessage(Context, embed);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
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
            if (guildUser.GuildPermissions.Administrator)
            {
                var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var response = $"Failed to add this leave message...";
                if (guildAcc.LeaveMessages.Contains(message) == false)
                {
                    guildAcc.LeaveMessages.Add(message);
                    GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                    response = $"Successfully added `{message}` as a leave message!";
                }

                await ReplyAsync(response);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("leave remove"), Alias("lr")]
        [Summary("Removes a leave message from the ones available")]
        [Remarks("n!lr <leave message number (shown in n!leave list)> Ex: n!lr 1")]
        public async Task RemoveLeaveMessage(int messageIndex)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (guildUser.GuildPermissions.Administrator)
            {
                var messages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).LeaveMessages;
                var response = $"Failed to remove this leave message... Use the number shown in `leave list` next to the `#` sign!";
                if (messages.Count > messageIndex - 1)
                {
                    messages.RemoveAt(messageIndex - 1);
                    GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                    response = $"Successfully removed message #{messageIndex} as possible Welcome Message!";
                }

                await ReplyAsync(response);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("leave list"), Alias("ll")]
        [Summary("Shows all currently set leave messages")]
        [Remarks("n!leave list")]
        public async Task ListLeaveMessages()
        {
            var guildUser = Context.User as SocketGuildUser;
            if (guildUser.GuildPermissions.Administrator)
            {
                var leaveMessages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).LeaveMessages;
                var embB = new EmbedBuilder().WithTitle("No leave messages set yet... add some if you want a message to be shown if someone leaves.");
                if (leaveMessages.Count > 0) embB.WithTitle("Possible leave messages:");

                for (var i = 0; i < leaveMessages.Count; i++)
                {
                    embB.AddField($"Message #{i + 1}:", leaveMessages[i], true);
                }
                await ReplyAsync("", false, embB.Build());
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
