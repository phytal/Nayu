using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
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
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var result = ConvertBool.ConvertStringToBoolean(arg);
                if (result.Item1 == true)
                {
                    bool argg = result.Item2;
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription(argg
                        ? "Enabled mass ping checks for this server."
                        : "Disabled mass ping checks for this server.");
                    config.MassPingChecks = argg;
                    GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                    await ReplyAsync("", embed: embed.Build());
                }
                if (result.Item1 == false)
                {
                    await Context.Channel.SendMessageAsync($"Please say `n!pc <on/off>`");
                    return;
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
