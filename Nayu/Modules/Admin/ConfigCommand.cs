using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;
using Nayu.Preconditions;
using System;
using System.IO;

namespace Nayu.Modules.Management
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


        [Command("Config")]
        [Summary("Displays all of the bot settings on this server")]
        [Remarks("Ex: n!config")]
        [Cooldown(10)]
        public async Task MasterConfig()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
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
                                              $"- WelcomeMsg: {welcomemessages}\n" +
                                              $"- LeavingMsg: {leavemessages}", true);
                }
                else
                {
                    embed.AddField("Welcome/Leaving", "Off", true);
                }

                embed.AddField("Other", $"Antilink: {ConvertBoolean(config.Antilink)}\n" +
                                        $"Mass Ping Checks: {ConvertBoolean(config.MassPingChecks)}\n" +
                                        $"Blacklist: {ConvertBoolean(config.Filter)}\n" +
                                        $"Autorole: {config.Autorole}\n" +
                                        $"Leveling: {ConvertBoolean(config.Leveling)}\n" +
                                        $"Server Logging: {ConvertBoolean(config.IsServerLoggingEnabled)}\n" +
                                        $"Unflipping: {ConvertBoolean(config.Unflip)}\n");

                embed.WithThumbnailUrl(Context.Guild.IconUrl);
                embed.WithFooter("Guild Information is shown incorrectly or not shown at all? Use `n!syncguild` to sync the current server owner!");

                await MiscHelpers.SendMessage(Context, embed);
            }

            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("SyncGuild")]
        [Summary("Syncs the current guild information with the database")]
        [Remarks("n!SyncGuild")]
        [Cooldown(5)]
        public async Task SyncGuild()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var info = System.IO.Directory.CreateDirectory(Path.Combine(Constants.ResourceFolder, Constants.ServerUserAccountsFolder));
                ulong In = Context.Guild.Id;
                string Out = Convert.ToString(In);
                if (!Directory.Exists(Out))
                    Directory.CreateDirectory(Path.Combine(Constants.ServerUserAccountsFolder, Out));

                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.GuildOwnerId = Context.Guild.Owner.Id;
                GlobalGuildAccounts.SaveAccounts();
                await Context.Channel.SendMessageAsync($"Successfully synced the Guild's owner to <@{Context.Guild.OwnerId}>!");
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
