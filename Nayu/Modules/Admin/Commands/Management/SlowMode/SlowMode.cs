using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;

namespace Nayu.Modules.Admin.Commands.Management.SlowMode
{
    public class SlowMode : NayuModule
    {
        private static DiscordShardedClient _client = Program._client;

        public static async Task Slowmode(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            var context = new ShardedCommandContext(_client, msg);
            var config = GlobalGuildAccounts.GetGuildAccount(context.Guild.Id);
            if (config.IsSlowModeEnabled)
            {
                if (context.User is IGuildUser user && user.GuildPermissions.ManageChannels) return;
                if (msg == null) return;
                if (msg.Channel == msg.Author.GetOrCreateDMChannelAsync()) return;
                if (msg.Author.IsBot) return;

                var userAcc = GlobalUserAccounts.GetUserAccount(msg.Author.Id);

                DateTime now = DateTime.UtcNow;

                if (now < userAcc.LastMessage.AddSeconds(config.SlowModeCooldown))
                {
                    var difference1 = now - userAcc.LastMessage;
                    var time = new TimeSpan((long)config.SlowModeCooldown*10000000);
                    var difference = time - difference1;
                    var timeSpanString = string.Format("{0:%s} seconds", difference);
                    await msg.DeleteAsync();
                    var dm = await context.User.GetOrCreateDMChannelAsync();
                    await dm.SendMessageAsync($"Slow down! You can send a message in **{timeSpanString}** in **{context.Guild.Name}**.");
                }
                else
                {
                    userAcc.LastMessage = now;
                }
            }
        }
    }
}
