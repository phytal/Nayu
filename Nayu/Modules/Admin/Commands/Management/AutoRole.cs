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
    public class AutoRole : NayuModule
    {
        [Subject(AdminCategories.Roles)]
        [Command("AutoRole")]
        [Summary("Adds a role that new members will receive automatically")]
        [Remarks("n!autorole <role name> Ex: n!autorole Member")]
        [Cooldown(5)]
        public async Task AutoRoleRoleAdd([Remainder]string arg = "")
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            if (arg == null) await ReplyAndDeleteAsync("Please include the name of the role you want to autorole");
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            config.Autorole = arg;
            GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);

            var embed = new EmbedBuilder();
            embed.WithDescription($"Added the **{arg}** role to Autorole!");
            embed.WithColor(Global.NayuColor);
            embed.WithFooter("Make sure that Nayu has a higher role than the autoroled role!");

            await SendMessage(Context, embed.Build());
        }
    }
}
