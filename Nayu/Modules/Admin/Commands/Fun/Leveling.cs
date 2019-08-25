using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;
using Nayu.Preconditions;
using Nayu.Helpers;

namespace Nayu.Modules.Management.Commands
{
    public class Leveling : NayuModule
    {
        [Command("levelingmsg")]
        [Alias("lvlmsg")]
        [Summary("Sets the way leveling messages are sent")]
        [Remarks("n!lvlmsg <dm/server> Ex: n!lvlmsg dm")]
        [Cooldown(5)]
        public async Task SetLvlingMsgStatus([Remainder]string preset)
        {
            var guser = Context.User as SocketGuildUser;
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            if (guser.GuildPermissions.Administrator)
            {
                if (config.Leveling == false)
                {
                    await Context.Channel.SendMessageAsync("You need to enable leveling on this server first!");
                    return;
                }
                if (preset == "dm" || preset == "server")
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription($"Set leveling messages to {preset}");

                    config.LevelingMsgs = preset;
                    GlobalGuildAccounts.SaveAccounts();
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Make sure you set it to either `dm` or `server`! Ex:n!lvlmsg dm");
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

        [Command("Leveling"), Alias("L")]
        [Summary("Enables or disables leveling for the server.")]
        [Remarks("n!leveling <on/off> Ex: n!leveling on")]
        [Cooldown(5)]
        public async Task LevelingToggle(string arg)
        {
            var user = Context.User as SocketGuildUser;
            if (user.GuildPermissions.Administrator)
            {
                var result = ConvertBool.ConvertStringToBoolean(arg);
                if (result.Item1 == true)
                {
                    bool argg = result.Item2;
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription(argg ? "Enabled leveling for this server." : "Disabled leveling for this server.");
                    config.Leveling = argg;
                    GlobalGuildAccounts.SaveAccounts();

                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                if (result.Item1 == false)
                {
                    await Context.Channel.SendMessageAsync($"Please say `n!leveling <on/off>`");
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
