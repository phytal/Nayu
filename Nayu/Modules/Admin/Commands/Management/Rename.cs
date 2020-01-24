using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Management
{
    public class Rename : NayuModule
    {
        [Subject(AdminCategories.UserManagement)]
        [Command("Rename")]
        [Alias("Nick")]
        [Summary("Changes a user's nickname")]
        [Remarks("n!rename <user you want to rename> <desired nickname> Ex: n!rename @Phytal dumb kid")]
        [Cooldown(5)]
        public async Task SetUsersNickname(SocketGuildUser user, [Remainder] string nick)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.ManageNicknames)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Manage Nicknames** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            await user.ModifyAsync(x => x.Nickname = nick);
            var embed = MiscHelpers
                .CreateEmbed(Context, "User Nicked", $"Set <@{user.Id}>'s nickname on this server to **{nick}**!")
                .WithColor(Global.NayuColor);
            await SendMessage(Context, embed.Build());
        }
    }
}
