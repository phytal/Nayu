using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Handlers;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Management
{
    public class ServerName : NayuModule
    {

        [Command("ServerName")]
        [Summary("Changes the name of the server")]
        [Remarks("n!servername <new name of the server> Ex: n!servername roblox is best")]
        [Cooldown(5)]
        public async Task ModifyServerName([Remainder] string name)
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

            await Context.Guild.ModifyAsync(x => x.Name = name);
            var embed = new EmbedBuilder();
            embed.WithDescription($"Set this server's name to **{name}**!");
            embed.WithColor(37, 152, 255);

            await ReplyAsync("", embed: embed.Build());
        }
    }
}
