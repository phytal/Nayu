using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Fun
{
    public class Leveling : NayuModule
    {
        [Subject(AdminCategories.BotSettings)]
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
                if (!config.Leveling)
                {
                    await SendMessage(Context, null, "You need to enable leveling on this server first!");
                    return;
                }
                if (preset == "dm" || preset == "server")
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(Global.NayuColor);
                    embed.WithDescription($"Set leveling messages to {preset}");

                    config.LevelingMsgs = preset;
                    GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                    await SendMessage(Context, embed.Build());
                }
                else
                {
                    await SendMessage(Context, null, "Make sure you set it to either `dm` or `server`! Ex:n!lvlmsg dm");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.Title = $"{Global.ENo} **|** You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Subject(AdminCategories.BotSettings)]
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
                if (result.Item1)
                {
                    bool argg = result.Item2;
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    var embed = new EmbedBuilder();
                    embed.WithColor(Global.NayuColor);
                    embed.WithDescription(argg ? "Enabled leveling for this server." : "Disabled leveling for this server.");
                    config.Leveling = argg;
                    GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);

                    await SendMessage(Context, embed.Build());
                }
                if (!result.Item1)
                {
                    await SendMessage(Context, null, $"Please say `n!leveling <on/off>`");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.Title = $"{Global.ENo} **|** You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
