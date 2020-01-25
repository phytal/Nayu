using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin
{
    public class MasterConfigCommand : NayuModule
    {
        private DiscordShardedClient _client = Program._client;

        public static string ConvertBoolean(bool boolean)
        {
            return boolean == true ? "**On**" : "**Off**";
        }

        public static string ConvertList(List<string> list, int count)
        {
            return list.Count >= count ? "**On**" : "**Off**";
        }

        public static string ConvertList(List<ulong> list, int count)
        {
            return list.Count >= count ? "**On**" : "**Off**";
        }

        public static string ConvertDict(Dictionary<string, string> dict, int count)
        {
            return dict.Count >= count ? "**On**" : "**Off**";
        }

        [Subject(AdminCategories.ServerManagement)]
        [Command("Config")]
        [Summary("Displays all of the bot settings on this server")]
        [Remarks("Ex: n!config")]
        [Cooldown(5)]
        public async Task MasterConfig()
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = MiscHelpers.CreateEmbed(Context, Context.Guild.Name, $"Server ID: {config.Id}\n" +
                                                                             $"Owner: <@{config.GuildOwnerId}>");

            var welcomemessages = "";
            for (var i = 0; i < config.WelcomeMessages.Count; i++)
            {
                welcomemessages = (config.WelcomeMessages[i]);
            }

            var leavemessages = "";
            for (var i = 0; i < config.LeaveMessages.Count; i++)
            {
                leavemessages = (config.LeaveMessages[i]);
            }

            if (config.WelcomeChannel != 0)
            {
                embed.AddField("Welcome/Leaving", "On:\n" +
                                                  $"- Welcome Channel: <#{config.WelcomeChannel}>\n" +
                                                  $"- Leave Channel: <#{config.LeaveChannel}>\n" +
                                                  $"- Welcome Messages: {string.Join(", ", config.WelcomeMessages.ToArray())}\n" +
                                                  $"- Leaving Messges: {string.Join(", ", config.LeaveMessages.ToArray())}",
                    true);
            }
            else
            {
                embed.AddField("Welcome/Leaving", "Off", true);
            }

            var antiLinkIgnoredChannels = "#" + string.Join(", #", config.AntilinkIgnoredChannels.ToArray());
            embed.AddField("Other", $"Antilink: {ConvertBoolean(config.Antilink)}\n" +

                                    $"Autorole: {config.Autorole}\n" +
                                    $"Anti-link Ignored Channels: {antiLinkIgnoredChannels}\n" +
                                    $"Blacklist: {ConvertBoolean(config.Filter)}\n" +
                                    $"Custom Blacklist: {string.Join(", ", config.CustomFilter.ToArray())}\n" +
                                    $"Custom Currency: {config.Currency}\n" +
                                    $"Custom Prefix: {config.CommandPrefix}\n" +
                                    $"Slow mode: {ConvertBoolean(config.IsSlowModeEnabled)}\n" +
                                    $"Leveling: {ConvertBoolean(config.Leveling)}\n" +
                                    $"Mass Ping Checks: {ConvertBoolean(config.MassPingChecks)}\n" +
                                    $"Unflipping: {ConvertBoolean(config.Unflip)}\n");

            embed.WithThumbnailUrl(Context.Guild.IconUrl);
            embed.WithFooter(
                "Guild Information is shown incorrectly or not shown at all? Use `n!syncguild` to sync the current server owner!");

            await SendMessage(Context, embed.Build());
        }

        [Command("SyncGuild")]
        [Summary("Syncs the current guild information with the database")]
        [Remarks("n!SyncGuild")]
        [Cooldown(5)]
        public async Task SyncGuild()
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            ulong In = Context.Guild.Id;
            string Out = Convert.ToString(In);
            if (!Directory.Exists(Out))
                Directory.CreateDirectory(Path.Combine(Constants.ServerUserAccountsFolder, Out));

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            config.GuildOwnerId = Context.Guild.Owner.Id;
            GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
            await SendMessage(Context, null, $"Successfully synced the Guild's owner to <@{Context.Guild.OwnerId}>!");
        }
    }
}
