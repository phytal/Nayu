using System;
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
    public class ServerPrefix : NayuModule
    {
        [Subject(AdminCategories.BotSettings)]
        [Command("ServerPrefix")]
        [Alias("setprefix")]
        [Summary("Changes the prefix for the bot on the current server")]
        [Remarks("n!serverprefix <desired prefix> Ex: n!serverprefix ~")]
        [Cooldown(5)]
        public async Task SetGuildPrefix([Remainder] string prefix = null)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(Global.NayuColor);
            if (prefix == null)
            {
                config.CommandPrefix = "n!";
                embed.WithDescription($"Set server prefix to the default prefix **(n!)**");
            }
            else
            {
                config.CommandPrefix = prefix;
                embed.WithDescription($"Set server prefix to {prefix}");
            }

            GlobalUserAccounts.SaveAccounts(Context.Guild.Id);
            await SendMessage(Context, embed.Build());
        }
    }
}