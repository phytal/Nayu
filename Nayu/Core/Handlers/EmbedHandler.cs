using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nayu.Helpers;
using Discord;
using Discord.WebSocket;

namespace Nayu.Handlers
{
    public static class EmbedHandler
    {
        /// <summary>
        /// Create a new embed
        /// </summary>
        /// <param name="title">Title of the embed</param>
        /// <param name="body">Embed content</param>
        /// <param name="type">Type of the Embed (Error, Info, Exception, Success) -> Sets the color</param>
        /// <param name="withTimeStamp">Adds the current Timestamp to the embed</param>
        /// <returns></returns>
        public static Embed CreateEmbed(string title, string body, EmbedMessageType type, SocketUser target)
        {
            var embed = new EmbedBuilder();
            var thumbnailUrl = target.GetAvatarUrl();
            var auth = new EmbedAuthorBuilder()
            {
                Name = target.Username,
                IconUrl = thumbnailUrl,
            };
            embed.WithAuthor(auth);
            embed.WithTitle(title);
            embed.WithDescription(body);

            switch (type)
            {
                case EmbedMessageType.Info:
                    embed.WithColor(new Color(52, 152, 219));
                    break;
                case EmbedMessageType.Success:
                    embed.WithColor(new Color(37, 152, 255));
                    break;
                case EmbedMessageType.Error:
                    embed.WithColor(new Color(192, 57, 43));
                    break;
                case EmbedMessageType.Exception:
                    embed.WithColor(new Color(230, 126, 34));
                    break;
                default:
                    embed.WithColor(new Color(149, 165, 166));
                    break;
            }

                embed.WithCurrentTimestamp();

            return embed.Build();
        }


        public static async Task<Embed> CreateBasicEmbed(string title = null, string description = null, string footer = null)
        {
            var embed = await Task.Run(() => (new EmbedBuilder()
                .WithTitle(title)
                .WithDescription(description)
                .WithFooter(footer)
                .WithColor(252, 132, 255).Build())); //Pink
            return embed;
        }

        public static async Task<Embed> CreateMusicEmbed(string title, string description, string footer = null)
        {
            var embed = await Task.Run(() => (new EmbedBuilder()
                .WithTitle(title)
                .WithDescription(description)
                .WithFooter(footer)
                .WithColor(37, 152, 255)
                .WithCurrentTimestamp().Build()));
            return embed;
        }

        public static async Task<Embed> CreateErrorEmbed(string source, string error, string footer = null)
        {
            var embed = await Task.Run(() => new EmbedBuilder()
                .WithTitle($"Error Source: {source}")
                .WithDescription($"**Error: {error}**")
                .WithFooter(footer)
                .WithColor(Color.Red).Build());
            return embed;
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
