using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;
using Nayu.Preconditions;

namespace Nayu.Modules.Management.Commands
{
    public class AutoRole : NayuModule
    {
        [Command("AutoRole")]
        [Summary("Adds a role that new members will recieve automatically")]
        [Remarks("n!autorole <role name> Ex: n!autorole Member")]
        [Cooldown(5)]
        public async Task AutoRoleRoleAdd(string arg = "")
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                if (arg == null) await ReplyAndDeleteAsync("Please include the name of the role you want to autorole");
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.Autorole = arg;
                GlobalGuildAccounts.SaveAccounts();

                var embed = new EmbedBuilder();
                embed.WithDescription($"Added the **{arg}** role to Autorole!");
                embed.WithColor(37, 152, 255);
                embed.WithFooter("Make sure that Nayu has a higher role than the autoroled role!");

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
