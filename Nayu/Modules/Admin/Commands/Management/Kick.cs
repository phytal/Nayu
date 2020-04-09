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
    public class Kick : NayuModule
    {
        [Subject(AdminCategories.UserManagement)]
        [Command("kick")]
        [Summary("Kicks @Username")]
        [Remarks("n!kick <user you want to kick> Ex: n!kick @Phytal")]
        [Cooldown(5)]
        public async Task KickAsync(IGuildUser user, string reason = "No reason provided.")
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.KickMembers)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Kick Members** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            try
            {
                await user.KickAsync();
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.Title = $" {user.Username} has been kicked from {user.Guild.Name}";
                embed.Description =
                    $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**Kicked by: **{Context.User.Mention}\n**Reason: **{reason}";
                await SendMessage(Context, embed.Build());
            }
            catch
            {
                await ReplyAndDeleteAsync("🖐️ **|** You must mention a valid user",
                    timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}