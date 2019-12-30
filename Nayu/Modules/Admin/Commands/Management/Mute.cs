using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Management
{
    public class Mute : NayuModule
    {
        private static readonly OverwritePermissions denyOverwrite = new OverwritePermissions(addReactions: PermValue.Deny, sendMessages: PermValue.Deny, attachFiles: PermValue.Deny);

        [Command("mute")]
        [Summary("Mutes @Username")]
        [Remarks("n!mute <user you want to mute> <reason> Ex: n!mute @Phytal spammed in the no spam channel")]
        [Cooldown(5)]
        public async Task MuteAsync(SocketGuildUser user)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                var muteRole = await GetMuteRole(user.Guild);
                if (!user.Roles.Any(r => r.Id == muteRole.Id))
                    await user.AddRoleAsync(muteRole).ConfigureAwait(false);
                var gld = Context.Guild as SocketGuild;
                var muted = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MUTED").FirstOrDefault() as SocketRole;
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $"**{user.Username}** was muted";
                embed.Description = $"**Username: **{user.Username}\n**Muted by: **{Context.User.Username}";
                await user.AddRoleAsync(muted);
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("unmute")]
        [Summary("Unmutes @Username")]
        [Remarks("n!unmute <user you want to unmute> Ex: n!unmute @Phytal")]
        [Cooldown(5)]
        public async Task UnmuteAsync(SocketGuildUser user = null)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageRoles)
            {
                try
                {
                    try { await user.ModifyAsync(x => x.Mute = false).ConfigureAwait(false); } catch { }
                    try { await user.RemoveRoleAsync(await GetMuteRole(user.Guild)).ConfigureAwait(false); } catch { }
                    var muted = user.Guild.Roles.Where(input => input.Name.ToUpper() == "MUTED").FirstOrDefault() as SocketRole;
                    await ReplyAsync(":white_check_mark:  | " + Context.User.Mention + " unmuted " + user.Username);
                }
                catch
                {
                    await ReplyAsync(":hand_splayed:  | You must mention a valid user that is muted");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Manange Roles Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
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
                    muteRole = await guild.CreateRoleAsync(muteRoleName, GuildPermissions.None).ConfigureAwait(false);
                }
                catch
                {
                    muteRole = guild.Roles.FirstOrDefault(r => r.Name == muteRoleName) ?? await guild.CreateRoleAsync(defaultMuteRoleName, GuildPermissions.None).ConfigureAwait(false);
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
