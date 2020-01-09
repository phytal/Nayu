using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Fun
{
    public class Vote : NayuModule
    {
        [Command("Vote")]
        [Alias("poll")]
        [Summary("Creates a voting poll")]
        [Remarks("n!vote <what you want to vote on> Ex: n!vote is Phytal good at overwatch")]
        [Cooldown(5)]
        public async Task Poll([Remainder] string Input)
        {
            var user = Context.User as SocketGuildUser;
            if (user.GuildPermissions.Administrator)
            {
                var embed = new EmbedBuilder();
                embed.WithTitle("Vote Started");
                embed.WithDescription(Input);
                embed.WithFooter($"requested by: {Context.User.Username}");
                embed.WithColor(37, 152, 255);

                var CheckMark = new Emoji("✅");
                var XMark = new Emoji("❌");

                var msg = await Context.Channel.SendMessageAsync("", embed: embed.Build());
                await msg.AddReactionAsync(CheckMark);
                await msg.AddReactionAsync(XMark);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $"{Global.ENo} | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
