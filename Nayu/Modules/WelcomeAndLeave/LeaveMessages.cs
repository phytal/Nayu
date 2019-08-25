using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;

namespace Nayu.Modules
{
    public class LeaveMessages : NayuModule
    {
        [Command("leave channel"), Alias("lc")]
        [Summary("Set where you want leave messages to be displayed")]
        [Remarks("n!leave channel <channel where welcome messages are> Ex: n!leave channel #general")]
        public async Task SetIdIntoConfig(SocketGuildChannel chnl)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithDescription($"Set this guild's leave channel to #{chnl}.");
                config.LeaveChannel = chnl.Id;
                GlobalGuildAccounts.SaveAccounts();
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("leave add")]
        [Summary("Announce a leaving user in the set announcement channel" +
             "with a random message out of the ones defined.")]
        [Remarks("n!leave add <leave message> Ex: `leave add Oh noo! <usermention>, left <guildname>...`\n" +
                 "Possible placeholders are: `<usermention>`, `<username>`, `<guildname>`, " +
                 "`<botname>`, `<botdiscriminator>`, `<botmention>`")]
        public async Task AddLeaveMessage([Remainder] string message)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var response = $"Failed to add this Leave Message...";
                if (guildAcc.LeaveMessages.Contains(message) == false)
                {
                    guildAcc.LeaveMessages.Add(message);
                    GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                    response = $"Successfully added `{message}` as Leave Message!";
                }

                await ReplyAsync(response);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("leave remove"), Summary("Removes a Leave Message from the ones available")]
        [Remarks("n!leave remove <leave message number (shown in n!leave list)> Ex: n!leave remove 1")]
        public async Task RemoveLeaveMessage(int messageIndex)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var messages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).LeaveMessages;
                var response = $"Failed to remove this Leave Message... Use the number shown in `leave list` next to the `#` sign!";
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
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("leave list"), Summary("Shows all currently set Leave Messages")]
        [Remarks("n!leave list")]
        public async Task ListLeaveMessages()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var leaveMessages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).LeaveMessages;
                var embB = new EmbedBuilder().WithTitle("No Leave Messages set yet... add some if you want a message to be shown if someone leaves.");
                if (leaveMessages.Count > 0) embB.WithTitle("Possible Leave Messages:");

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
