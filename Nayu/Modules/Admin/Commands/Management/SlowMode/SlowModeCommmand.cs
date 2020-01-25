using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Management.SlowMode
{
    public class Slowmode : NayuModule
    {        
        [Subject(AdminCategories.ServerManagement)]
        [Command("SlowMode"), Alias("sm")]
        [Summary("Adds a slowmode to the entire server (usually for large servers)")]
        [Remarks("n!slowmode <length between messages> Ex: n!slowmode 5")]
        [Cooldown(5)]
        public async Task SlowMode(ulong length)
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

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            config.IsSlowModeEnabled = true;
            config.SlowModeCooldown = length;
            GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);

            await SendMessage(Context, null, $":snail:  **|** Successfully turned on slow mode for **{length}** seconds.");
        }

        [Subject(AdminCategories.ServerManagement)]
        [Command("SlowModeOff"), Alias("smo")]
        [Summary("Disables Slowmode")]
        [Remarks("n!slowmodeoff")] //        [Remarks("n! <> Ex: n!")]
        [Cooldown(5)]
        public async Task SlowModeOff()
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

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            config.IsSlowModeEnabled = false;
            config.SlowModeCooldown = 0;
            GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);

            await SendMessage(Context, null, $":snail:  **|** Successfully turned off slow mode.");
        }
    }
}
