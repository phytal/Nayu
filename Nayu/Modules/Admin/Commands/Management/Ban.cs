using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.Management.Commands
{
    public class Ban : NayuModule
    {
        [Command("ban")]
        [Summary("Bans a specified user")]
        [Remarks("n!ban <user you want to ban> Ex: n!ban @Phytal")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [Cooldown(5)]
        public async Task BanAsync(IGuildUser user, string reason = "No reason provided.")
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var guildUser = Context.User as SocketGuildUser;
            if (guildUser.GuildPermissions.BanMembers)
            {
                try
                {
                    var kb = (Context.Client as DiscordShardedClient).GetChannel(config.ServerLoggingChannel) as SocketTextChannel;
                    var gld = Context.Guild as SocketGuild;
                    var embed = new EmbedBuilder();
                    embed.WithColor(new Color(37, 152, 255));
                    embed.Title = $"**{user.Username}** was banned";
                    embed.Description = $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**Banned by: **{Context.User.Mention}\n**Reason: **{reason}";

                    await gld.AddBanAsync(user);
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                    await kb.SendMessageAsync("", embed: embed.Build());
                }
                catch
                {
                    await ReplyAsync(":hand_splayed:  | You must mention a valid user that has a low enough rank to be banned.");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Ban Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
                return;
            }
        }

        [Command("UnBans")]
        [Summary("UnBans A User")]
        [Remarks("n!UnBans <user you want to UnBans> Ex: n!UnBans @Phytal#8213")]
        [Cooldown(5)]
        public async Task Unban([Remainder]string user2)
        {
            var user = Context.User as SocketGuildUser;
            if (user.GuildPermissions.BanMembers)
            {

                var bans = await Context.Guild.GetBansAsync();
                var theUser = bans.FirstOrDefault(x => x.User.ToString().ToLowerInvariant() == user2.ToLowerInvariant());

                await Context.Guild.RemoveBanAsync(theUser.User).ConfigureAwait(false);
                await Context.Channel.SendMessageAsync($":white_check_mark:  | Unbanned {user2}.");
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Ban Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("SoftBan"), Alias("Sb")]
        [Summary("Bans then UnBans a user.")]
        [Remarks("n!SoftBan <user you want to soft ban> Ex: n!SoftBan @Phytal")]
        [Cooldown(5)]
        public async Task SoftBan(SocketGuildUser user)
        {
            var guildUser = Context.User as SocketGuildUser;
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            if (guildUser.GuildPermissions.BanMembers)
            {
                try
                {
                    var embed = MiscHelpers.CreateEmbed(Context, "SoftBan", $"{Context.User.Mention} SoftBanned <@{user.Id}>, deleting the last 7 days of messages from that user.");
                    await MiscHelpers.SendMessage(Context, embed);
                    await Context.Guild.AddBanAsync(user, 7);
                    await Context.Guild.RemoveBanAsync(user);
                }
                catch
                {
                    await ReplyAsync(":hand_splayed:  | You must mention a valid user");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Ban Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("IdBan")]
        [Summary("Ban a user by their ID")]
        [Remarks("n!IdBan <user you want to IdBan> Ex: n!IdBan 264897146837270529")]
        [Cooldown(5)]
        public async Task BanUserById(ulong userid, [Remainder]string reason = "No reason provided.")
        {
            var guildUser = Context.User as SocketGuildUser;
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            if (guildUser.GuildPermissions.BanMembers)
            {
                try
                {
                    await Context.Guild.AddBanAsync(userid, 7, reason);
                    var embed = new EmbedBuilder();
                    embed.WithColor(new Color(37, 152, 255));
                    embed.Title = $"**{userid}** was banned";
                    embed.Description = $"**Username: **{userid}\n**Banned by: **{Context.User.Mention}\n**Reason: **{reason}";
                    await MiscHelpers.SendMessage(Context, embed);
                }
                catch
                {
                    await ReplyAsync(":hand_splayed:  | You must enter a valid user-id");
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You need the Ban Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
