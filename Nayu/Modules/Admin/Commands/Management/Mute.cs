using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Management
{
    public class Mute : NayuModule
    {
        private static readonly OverwritePermissions denyOverwrite =
            new OverwritePermissions(addReactions: PermValue.Deny, sendMessages: PermValue.Deny,
                attachFiles: PermValue.Deny);
        
        [Subject(AdminCategories.UserManagement)]
        [Command("mute")]
        [Summary("Mutes @Username")]
        [Remarks("n!mute <user you want to mute> Ex: n!mute @Phytal")]
        [Cooldown(5)]
        public async Task MuteAsync(SocketGuildUser user)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.ManageRoles)
            {
                string description =
                    $"{Global.ENo} | You Need the **Manage Roles** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var muteRole = await GetMuteRole(user.Guild);
            if (!user.Roles.Any(r => r.Id == muteRole.Id))
                await user.AddRoleAsync(muteRole).ConfigureAwait(false);
            var gld = Context.Guild as SocketGuild;
            var muted = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MUTED").FirstOrDefault() as SocketRole;
            var embed = new EmbedBuilder();
            embed.WithColor(Global.NayuColor);
            embed.Title = $"**{user.Username}** was muted";
            embed.Description = $"**Username: **{user.Username}\n**Muted by: **{Context.User.Username}";
            await user.AddRoleAsync(muted);
            await SendMessage(Context, embed.Build());
        }
        
        [Subject(AdminCategories.UserManagement)]
        [Command("unmute")]
        [Summary("Unmutes @Username")]
        [Remarks("n!unmute <user you want to unmute> Ex: n!unmute @Phytal")]
        [Cooldown(5)]
        public async Task UnmuteAsync(SocketGuildUser user = null)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.ManageRoles)
            {
                string description =
                    $"{Global.ENo} | You Need the **Manage Roles** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            try
            {
                try
                {
                    await user.ModifyAsync(x => x.Mute = false).ConfigureAwait(false);
                }
                catch
                {
                }

                try
                {
                    await user.RemoveRoleAsync(await GetMuteRole(user.Guild)).ConfigureAwait(false);
                }
                catch
                {
                }

                var muted =
                    user.Guild.Roles.Where(input => input.Name.ToUpper() == "MUTED").FirstOrDefault() as SocketRole;
                await ReplyAsync("✅  | " + Context.User.Mention + " unmuted " + user.Username);
            }
            catch
            {
                await ReplyAsync("🖐️ | You must mention a valid user that is muted");
            }
        }

        public async Task<IRole> GetMuteRole(IGuild guild)
        {
            const string defaultMuteRoleName = "Muted";

            var muteRoleName = "Muted";

            var muteRole = guild.Roles.FirstOrDefault(r => r.Name == muteRoleName);

            if (muteRole == null)
            {
                try
                {
                    muteRole = await guild.CreateRoleAsync(muteRoleName, GuildPermissions.None, Color.Magenta, false, null).ConfigureAwait(false);
                }
                catch
                {
                    muteRole = guild.Roles.FirstOrDefault(r => r.Name == muteRoleName) ?? await guild.CreateRoleAsync(defaultMuteRoleName, GuildPermissions.None, Color.Magenta, false, null).ConfigureAwait(false);
                }
            }

            foreach (var toOverwrite in (await guild.GetTextChannelsAsync()))
            {
                try
                {
                    if (!toOverwrite.PermissionOverwrites.Any(x => x.TargetId == muteRole.Id && x.TargetType == PermissionTarget.Role))
                    {
                        await toOverwrite.AddPermissionOverwriteAsync(muteRole, denyOverwrite)
                            .ConfigureAwait(false);

                        await Task.Delay(200).ConfigureAwait(false);
                    }
                }
                catch
                {

                }
            }

            return muteRole;
        }
    }
}
