﻿using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;
using Nayu.Preconditions;

namespace Nayu.Modules.Management.Commands
{
    public class SelfRole : NayuModule
    {

        [Command("SelfRoleAdd"), Alias("SRA")]
        [Summary("Adds a role a user can add themselves with n!Iam or n!Iamnot")]
        [Remarks("n!sra <role you want to be available> Ex: n!sra Member")]
        [Cooldown(5)]
        public async Task AddStringToList([Remainder]string role)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder()
                    .WithColor(37, 152, 255)
                    .WithDescription($"Added the {role} to the Config.");
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
                config.SelfRoles.Add(role);
                GlobalGuildAccounts.SaveAccounts();
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("SelfRoleRem"), Alias("SRR")]
        [Summary("Removes a Self Role. Users can add a role themselves with n!Iam or n!Iamnot")]
        [Remarks("n!srr <self role you want to be removed> Ex: n!srr Member")]
        [Cooldown(5)]
        public async Task RemoveStringFromList([Remainder]string role)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                if (config.SelfRoles.Contains(role))
                {
                    config.SelfRoles.Remove(role);
                    embed.WithDescription($"Removed {role} from the Self Roles list.");
                    GlobalGuildAccounts.SaveAccounts();
                }
                else
                {
                    embed.WithDescription("That role doesn't exist in your Guild Config.");
                }
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("SelfRoleClear"), Alias("SRC")]
        [Summary("Clears all Self Roles. Users can add a role themselves with n!Iam or n!Iamnot")]
        [Remarks("n!src")]
        [Cooldown(5)]
        public async Task ClearListFromConfig()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
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
                }

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
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
