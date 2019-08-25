using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Nayu.Core.Modules;
using Nayu.Preconditions;
using Nayu.Helpers;

namespace Nayu.Modules.Management.Commands
{
    public class Rename : NayuModule
    {
        [Command("Rename")]
        [Alias("Nick")]
        [Summary("Changes a user's nickname")]
        [Remarks("n!rename <user you want to rename> <desired nickname> Ex: n!rename @Phytal dumb kid")]
        [Cooldown(5)]
        public async Task SetUsersNickname(SocketGuildUser user, [Remainder]string nick)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageMessages)
            {
                await user.ModifyAsync(x => x.Nickname = nick);
                var embed = MiscHelpers.CreateEmbed(Context, "User Nicked", $"Set <@{user.Id}>'s nickname on this server to **{nick}**!").WithColor(37, 152, 255);
                await MiscHelpers.SendMessage(Context, embed);
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
