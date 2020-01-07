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
    public class AntiLink : NayuModule
    {
        [Command("Antilink"), Alias("Al")]
        [Summary("Turns on or off the link filter.")]
        [Remarks("n!al <on/off> Ex: n!al on")]
        [Cooldown(5)]
        public async Task SetBoolIntoConfig(string arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageMessages)
            {
                var result = ConvertBool.ConvertStringToBoolean(arg);
                if (result.Item1 == true)
                {
                    bool setting = result.Item2;
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    config.Antilink = setting;
                    GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription(setting ? "Enabled Antilink for this server." : "Disabled Antilink for this server.");
                    await ReplyAsync("", embed: embed.Build());
                }
                if (!result.Item1)
                {
                    await SendMessage(Context, null, $"Please say `n!al <on/off>`");
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

        [Command("AntilinkIgnore"), Alias("Ali")]
        [Summary("Sets a channel that if Antilink is turned on, it will be disabled there")]
        [Remarks("n!ali <channel you want anti-link to be ignored> Ex: n!ali #links-only")]
        [Cooldown(5)]
        public async Task SetChannelToBeIgnored(string type, SocketGuildChannel chnl = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageMessages)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                switch (type)
                {
                    case "add":
                    case "Add":
                        config.AntilinkIgnoredChannels.Add(chnl.Id);
                        embed.WithDescription($"Added <#{chnl.Id}> to the list of ignored channels for Antilink.");
                        break;
                    case "rem":
                    case "Rem":
                        config.AntilinkIgnoredChannels.Remove(chnl.Id);
                        embed.WithDescription($"Removed <#{chnl.Id}> from the list of ignored channels for Antilink.");
                        break;
                    case "clear":
                    case "Clear":
                        config.AntilinkIgnoredChannels.Clear();
                        embed.WithDescription("List of channels to be ignored by Antilink has been cleared.");
                        break;
                    default:
                        embed.WithDescription($"Valid types are `add`, `rem`, and `clear`. Syntax: `n!ali {{add/rem/clear}} [channelMention]`");
                        break;
                }
                GlobalUserAccounts.SaveAccounts(Context.Guild.Id);
                await SendMessage(Context, embed);
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
