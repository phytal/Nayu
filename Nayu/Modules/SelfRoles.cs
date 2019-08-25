using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;

namespace Nayu.Modules
{
    public class SelfRoles : NayuModule
    {
        [Command("Iam")]
        [Alias("iam")]
        [Summary("Gives you a self role")]
        public async Task GiveYourselfRole([Remainder]string role)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var user = Context.User as SocketGuildUser;
            var embed = new EmbedBuilder()
                .WithColor(37, 152, 255);

            var roleList = new List<string>();
            foreach (var roleName in config.SelfRoles) roleList.Add(roleName.ToLower());
            if (!roleList.Contains(role))
            {
                embed.WithDescription("This server doesn't have any self roles set.");
            }
            else
            {
                if (config.SelfRoles.Contains(role))
                {
                        var r = Context.Guild.Roles.First(x => x.Name.ToLower() == role.ToLower());
                        embed.WithDescription($"Gave you the **{r.Name}** role.");
                        await (Context.User as SocketGuildUser).AddRoleAsync(r);
                }
                else
                {
                    embed.WithDescription("That role isn't in the self roles list for this server.");
                }
            }

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("Iamnot"), Alias("Iamn", "iamnot", "iamn")]
        [Summary("Remove a self role from you")]
        public async Task TakeAwayRole([Remainder]string role)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var user = Context.User as SocketGuildUser;
            var embed = new EmbedBuilder()
                .WithColor(37, 152, 255);

            var roleList = new List<string>();
            foreach (var roleName in config.SelfRoles) roleList.Add(roleName.ToLower());
            if (roleList.Contains(role))
            {
                embed.WithDescription("This server doesn't have any self roles set.");
            }
            else
            {
                if (config.SelfRoles.Contains(role))
                {
                    var r = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == role.ToLower());
                    embed.WithDescription($"Removed your **{r.Name}** role.");
                    await (Context.User as SocketGuildUser).RemoveRoleAsync(r);
                }
                else
                {
                    embed.WithDescription("That role isn't in the self roles list for this server.");
                }
            }

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("SelfRoleList"), Alias("selfroles", "srl"), Summary("Shows all currently set Self Roles")]
        public async Task SelfRoleList()
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);

            var embed = new EmbedBuilder();
            if (config == null)
            {
                embed.WithDescription("This server doesn't have any self-assignable roles.");
            }
            else
            {
                config.SelfRoles.Sort();
                var roles = "\n";
                foreach (var role in config.SelfRoles) roles += $"**{role}**\n";

                embed.WithTitle("Roles you can self-assign: ");
                embed.WithDescription(roles);
            }
            await ReplyAsync("", embed: embed.Build());
        }
    }
}
