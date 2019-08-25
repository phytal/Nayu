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
    public class Slowmode : NayuModule
    {
        [Command("SlowMode")]
        [Summary("Adds a slowmode to the entire server (usually for large servers)")]
        [Remarks("n!slowmode <length between messages> Ex: n!slowmode 5")]
        [Cooldown(5)]
        public async Task SlowMode(ulong length)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageChannels)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.IsSlowModeEnabled = true;
                config.SlowModeCooldown = length;
                GlobalGuildAccounts.SaveAccounts();

                await Context.Channel.SendMessageAsync($":snail:  | Successfully turned on Slowmode for **{length}** seconds.");
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Manage Channels Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("SlowModeOff")]
        [Summary("Disables Slowmode")]
        [Remarks("n!slowmodeoff")] //        [Remarks("n! <> Ex: n!")]
        [Cooldown(5)]
        public async Task SlowModeOff()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageChannels)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.IsSlowModeEnabled = false;
                config.SlowModeCooldown = 0;
                GlobalGuildAccounts.SaveAccounts();

                await Context.Channel.SendMessageAsync($":snail:  | Successfully turned off Slowmode.");
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Manage Channels Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
