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
    public class Filtering : NayuModule
    {
        [Command("Filter"), Alias("blacklist", "bl", "fil")]
        [Summary("Turns on or off filter. Usage: n!filter true/false")]
        [Remarks("n!filter <on/off> Ex: n!filter on")]
        [Cooldown(5)]
        public async Task SetBoolIntoConfigFilter(string setting)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description = $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            var result = ConvertBool.ConvertStringToBoolean(setting);
            if (result.Item1)
            {
                var argg = result.Item2;
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.WithDescription(argg
                    ? "✅  | Filter successfully turned on. Stay safe!"
                    : "✅  | Filter successfully turned off. Daredevil!");
                await ReplyAsync("", embed: embed.Build());
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.Filter = argg;
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
            }
            else
            {
                await SendMessage(Context, null, $"Please say `n!filter <on/off>`");
            }
        }

        [Command("FilterIgnore"), Alias("Fi")]
        [Summary("Sets a channel that if Filter is turned on, it will be disabled there")]
        [Remarks("n!fi <channel you want filter to be ignored> Ex: n!fi #nsfw")]
        [Cooldown(5)]
        public async Task SetChannelToBeIgnoredByFilter(string type, SocketGuildChannel chnl = null)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description = $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            switch (type)
            {
                case "add":
                case "Add":
                    config.NoFilterChannels.Add(chnl.Id);
                    embed.WithDescription($"Added <#{chnl.Id}> to the list of ignored channels for Filter.");
                    break;
                case "rem":
                case "Rem":
                    config.NoFilterChannels.Remove(chnl.Id);
                    embed.WithDescription($"Removed <#{chnl.Id}> from the list of ignored channels for Filter.");
                    break;
                case "clear":
                case "Clear":
                    config.NoFilterChannels.Clear();
                    embed.WithDescription("List of channels to be ignored by Filter has been cleared.");
                    break;
                default:
                    embed.WithDescription(
                        $"Valid types are `add`, `rem`, and `clear`. Syntax: `n!fi {{add/rem/clear}} [channelMention]`");
                    break;
            }

            GlobalUserAccounts.SaveAccounts(Context.Guild.Id);
            await SendMessage(Context, embed.Build());
        }

        [Command("BlacklistAdd")]
        [Alias("Bladd")]
        [Summary("Add a word to the filter")]
        [Remarks("n!bladd <word you want to add> Ex: n!bladd gay")]
        [Cooldown(5)]
        public async Task AddStringToBl([Remainder] string word)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description = $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            await SendMessage(Context, null, $"✅  | Added **{word}** to the Blacklist.");

            config.CustomFilter.Add(word);
            GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
        }

        [Command("BlacklistRemove")]
        [Alias("Blrem")]
        [Summary("Remove a custom word from filter")]
        [Remarks("n!blrem <word you want to remove> Ex: n!blrem gay")]
        [Cooldown(5)]
        public async Task RemoveStringFromBl([Remainder] string bl)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description = $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            if (!config.CustomFilter.Contains(bl))
            {
                embed.WithDescription($"`{bl}` isn't present in the Blacklist.");
            }
            else
            {
                embed.WithDescription($"Removed {bl} from the Blacklist.");
                config.CustomFilter.Remove(bl);
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
            }

            await SendMessage(Context, embed.Build());
        }

        [Command("BlacklistClear")]
        [Alias("Blcl")]
        [Summary("Clears the custom words in the filter")]
        [Remarks("n!blcl")]
        [Cooldown(5)]
        public async Task ClearBlacklist()
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description = $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            config.CustomFilter.Clear();
            GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithDescription("Cleared the Blacklist for this server.");
            embed.WithColor(37, 152, 255);

            await SendMessage(Context, embed.Build());
        }

        [Command("BlacklistList")]
        [Alias("Bll")]
        [Summary("Lists the custom words in the filter")]
        [Remarks("n!bll")]
        [Cooldown(5)]
        public async Task ListBlacklist()
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.ManageMessages)
            {
                string description = $"{Global.ENo} | You Need the **Manage Messages** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            string list = string.Empty;
            foreach (var word in config.CustomFilter)
            {
                list += $"{word}  ";
            }

            var embed = new EmbedBuilder();
            embed.WithTitle($"Blacklisted words in {Context.Guild.Name}");
            embed.WithDescription(list);
            embed.WithColor(37, 152, 255);

            await SendMessage(Context, embed.Build());
        }
    }
}
