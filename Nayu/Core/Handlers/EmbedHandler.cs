using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Nayu.Core.Handlers
{
    public static class EmbedHandler
    {
        /// <summary>
        /// Create a new embed
        /// </summary>
        /// /// <param name="ctx">Command Context</param>
        /// <param name="title">Title of the embed</param>
        /// <param name="desc">Embed content</param>
        /// <param name="type">Type of the Embed (Error, Info, Exception, Success) -> Sets the color</param>
        /// <param name="withAuthorAndFooter">Adds an author and footer to the embed</param>
        /// <returns></returns>
        public static Embed CreateEmbed(ShardedCommandContext ctx, string title, string desc, EmbedMessageType type, bool withAuthorAndFooter = true)
        {
            var thumbnailurl = ctx.User.GetAvatarUrl();
            var boturl = Global.Client.CurrentUser.GetAvatarUrl();
            var auth = new EmbedAuthorBuilder()
            {
                IconUrl = thumbnailurl,
                Name = "Requested by " + ctx.User.Username
            };

            var footer = new EmbedFooterBuilder()
            {
                IconUrl = boturl,
                Text = type == EmbedMessageType.Error ? "Did you use the command correctly? If so, please report this to our discord server https://discord.gg/eyHg6hS" : "Nayu | n!help"
            };

            var embed = new EmbedBuilder()
            {
                Title = title,
                Description = desc,
            };

            if (withAuthorAndFooter)
            {
                embed.Author = auth;
                embed.Footer = footer;
                embed.Timestamp = DateTimeOffset.Now;
            }
            
            switch (type)
            {
                case EmbedMessageType.Info:
                    embed.WithColor(new Color(252, 132, 255));
                    break;
                case EmbedMessageType.Success:
                    embed.WithColor(new Color(153, 255, 255));
                    break;
                case EmbedMessageType.Error:
                    embed.WithColor(new Color(255, 153, 153));
                    break;
                case EmbedMessageType.Exception:
                    embed.WithColor(new Color(255, 204, 153));
                    break;
                default:
                    embed.WithColor(new Color(224, 224, 224));
                    break;
            }
            
            return embed.Build();
        }

        public enum EmbedMessageType
        {
            Success = 0,
            Info = 10,
            Error = 20,
            Exception = 30
        }
    }
}
