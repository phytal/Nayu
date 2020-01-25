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
    public class PingChecks : NayuModule
    {
        [Command("PingChecks"), Alias("Pc")]
        [Summary("Turns on or off mass ping checks.")]
        [Remarks("n!pc <on/off> Ex: n!pc on")]
        [Cooldown(5)]
        public async Task PingCheck(string arg)
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

            var result = ConvertBool.ConvertStringToBoolean(arg);
            if (result.Item1)
            {
                bool argg = result.Item2;
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithDescription(argg
                    ? "Enabled mass ping checks for this server."
                    : "Disabled mass ping checks for this server.");
                config.MassPingChecks = argg;
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                await ReplyAsync("", embed: embed.Build());
            }

            if (result.Item1 == false)
            {
                await SendMessage(Context, null, $"Please say `n!pc <on/off>`");
                return;
            }
        }
    }
}
