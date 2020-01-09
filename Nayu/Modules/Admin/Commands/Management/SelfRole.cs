using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Management
{
    public class SelfRole : NayuModule
    {

        [Command("SelfRoleAdd"), Alias("SRA")]
        [Summary("Adds a role a user can add themselves with n!Iam or n!Iamnot")]
        [Remarks("n!sra <role you want to be available> Ex: n!sra Member")]
        [Cooldown(5)]
        public async Task AddStringToList([Remainder] string role)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder()
                .WithColor(37, 152, 255)
                .WithDescription($"Added the {role} to the Config.");
            await SendMessage(Context, embed.Build());
            config.SelfRoles.Add(role);
            GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
        }

        [Command("SelfRoleRem"), Alias("SRR")]
        [Summary("Removes a Self Role. Users can add a role themselves with n!Iam or n!Iamnot")]
        [Remarks("n!srr <self role you want to be removed> Ex: n!srr Member")]
        [Cooldown(5)]
        public async Task RemoveStringFromList([Remainder] string role)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            if (config.SelfRoles.Contains(role))
            {
                config.SelfRoles.Remove(role);
                embed.WithDescription($"Removed {role} from the Self Roles list.");
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
            }
            else
            {
                embed.WithDescription("That role doesn't exist in your Guild Config.");
            }

            await SendMessage(Context, embed.Build());
        }

        [Command("SelfRoleClear"), Alias("SRC")]
        [Summary("Clears all Self Roles. Users can add a role themselves with n!Iam or n!Iamnot")]
        [Remarks("n!src")]
        [Cooldown(5)]
        public async Task ClearListFromConfig()
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            if (config == null)
            {
                embed.WithDescription("You don't have a Guild Config created.");
            }
            else
            {
                embed.WithDescription($"Cleared {config.SelfRoles.Count} roles from the self role list.");
                config.SelfRoles.Clear();
                GlobalUserAccounts.SaveAccounts(Context.Guild.Id);
            }

            await SendMessage(Context, embed.Build());
        }
    }
}
