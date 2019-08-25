using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nayu.Features.GlobalAccounts;

namespace Nayu.Helpers
{
    class MiscHelpers
    {
        public static async Task SendMessage(ShardedCommandContext ctx, EmbedBuilder embed = null, string msg = "")
        {
            if (embed == null)
            {
                await ctx.Channel.SendMessageAsync(msg);
            }
            else
            {
                await ctx.Channel.SendMessageAsync(msg, false, embed.Build());
            }
        }

        internal static bool UserHasRole(ShardedCommandContext ctx, ulong roleId)
        {
            var targetRole = ctx.Guild.Roles.FirstOrDefault(r => r.Id == roleId);
            var gUser = ctx.User as SocketGuildUser;

            return (gUser.Roles.Contains(targetRole));
        }

        public static EmbedBuilder CreateEmbed(ShardedCommandContext ctx, string title, string desc)
        {
            var thumbnailurl = ctx.User.GetAvatarUrl();
            var boturl = Global.Client.CurrentUser.GetAvatarUrl();
            var auth = new EmbedAuthorBuilder()
            {
                IconUrl = thumbnailurl,
                Name = ctx.User.Username
            };

            var footer = new EmbedFooterBuilder()
            {
                IconUrl = boturl,
                Text = "Nayu | n!help"
            };

            var embed = new EmbedBuilder()
            {
                Author = auth,
                Footer = footer,
                Color = new Color(37, 152, 255),
                Title = title,
                Description = desc,
            };
            return embed;
        }
    }
}
