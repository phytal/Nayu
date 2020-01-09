﻿using System;
using System.Linq;
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
    public class Ban : NayuModule
    {
        [Command("ban")]
        [Summary("Bans a specified user")]
        [Remarks("n!ban <user you want to ban> Ex: n!ban @Phytal")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [Cooldown(5)]
        public async Task BanAsync(IGuildUser user, string reason = "No reason provided.")
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.BanMembers)
            {
                string description =
                    $"{Global.ENo} | You Need the **Ban Members** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            try
            {
                var logs =
                    (Context.Client).GetChannel(config.ServerLoggingChannel) as
                    SocketTextChannel;
                var gld = Context.Guild as SocketGuild;
                var embed = new EmbedBuilder();
                embed.WithColor(new Color(37, 152, 255));
                embed.Title = $"**{user.Username}** was banned";
                embed.Description =
                    $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**Banned by: **{Context.User.Mention}\n**Reason: **{reason}";

                await gld.AddBanAsync(user);
                await SendMessage(Context, embed.Build());
                await logs.SendMessageAsync("", embed: embed.Build());
            }
            catch
            {
                var embed = EmbedHandler.CreateEmbed(Context, "Error",
                    "🖐️ | You must mention a valid user that has a low enough rank to be banned.",
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: embed);
            }
        }

        [Command("Unban")]
        [Summary("Unban A User")]
        [Remarks("n!Unban <user you want to Unban> Ex: n!UnBans @Phytal#8213")]
        [Cooldown(5)]
        public async Task Unban([Remainder] string user2)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.BanMembers)
            {
                string description =
                    $"{Global.ENo} | You Need the **Ban Members** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            try
            {
                var bans = await Context.Guild.GetBansAsync();
                var theUser =
                    bans.FirstOrDefault(x => x.User.ToString().ToLowerInvariant() == user2.ToLowerInvariant());

                await Context.Guild.RemoveBanAsync(theUser.User).ConfigureAwait(false);
                await SendMessage(Context, null, $"✅  | Unbanned {user2}.");
            }
            catch
            {
                var embed = EmbedHandler.CreateEmbed(Context, "Error",
                    "🖐️ | You must enter a valid user (Ex: Phytal#8213)",
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: embed);
            }
        }

        [Command("SoftBan"), Alias("Sb")]
        [Summary("Bans then UnBans a user.")]
        [Remarks("n!SoftBan <user you want to soft ban> Ex: n!SoftBan @Phytal")]
        [Cooldown(5)]
        public async Task SoftBan(SocketGuildUser user)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.BanMembers)
            {
                string description =
                    $"{Global.ENo} | You Need the **Ban Members** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            try
            {
                var embed = MiscHelpers.CreateEmbed(Context, "SoftBan",
                    $"{Context.User.Mention} SoftBanned <@{user.Id}>, deleting the last 7 days of messages from that user.");
                await MiscHelpers.SendMessage(Context, embed);
                await Context.Guild.AddBanAsync(user, 7);
                await Context.Guild.RemoveBanAsync(user);
            }
            catch
            {
                var embed = EmbedHandler.CreateEmbed(Context, "Error",
                    "🖐️ | You must enter a valid user",
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: embed);
            }
        }

        [Command("IdBan")]
        [Summary("Ban a user by their ID")]
        [Remarks("n!IdBan <user you want to IdBan> Ex: n!IdBan 264897146837270529")]
        [Cooldown(5)]
        public async Task BanUserById(ulong userid, [Remainder] string reason = "No reason provided.")
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.BanMembers)
            {
                string description =
                    $"{Global.ENo} | You Need the **Ban Members** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            try
            {
                await Context.Guild.AddBanAsync(userid, 7, reason);
                var embed = new EmbedBuilder();
                embed.WithColor(new Color(37, 152, 255));
                embed.Title = $"**{userid}** was banned";
                embed.Description =
                    $"**Username: **{userid}\n**Banned by: **{Context.User.Mention}\n**Reason: **{reason}";
                await MiscHelpers.SendMessage(Context, embed);
            }
            catch
            {
                var embed = EmbedHandler.CreateEmbed(Context, "Error",
                    "🖐️ | You must enter a valid user-id",
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: embed);
            }
        }
    }
}
