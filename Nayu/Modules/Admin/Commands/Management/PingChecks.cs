using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;
using Nayu.Preconditions;
using Nayu.Helpers;

namespace Nayu.Modules.Management.Commands
{
    public class PingChecks : NayuModule
    {
        [Command("PingChecks"), Alias("Pc")]
        [Summary("Turns on or off mass ping checks.")]
        [Remarks("n!pc <on/off> Ex: n!pc on")]
        [Cooldown(5)]
        public async Task SetBoolToJson(string arg)
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
                    GlobalGuildAccounts.SaveAccounts();
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
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
