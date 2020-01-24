using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nayu.Helpers
{
    class MiscHelpers
    {

        internal static bool UserHasRole(ShardedCommandContext ctx, ulong roleId)
        {
            var targetRole = ctx.Guild.Roles.FirstOrDefault(r => r.Id == roleId);
            var guildUser = ctx.User as SocketGuildUser;

            return (guildUser.Roles.Contains(targetRole));
        }
        
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example>string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;</example>
        public static T GetAttributeOfType<T>(Enum enumVal) where T:Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
        
        public static CommandAttribute GetCommandAttribute(MethodInfo methodInfo)
        { 
            return methodInfo.GetCustomAttribute<CommandAttribute>();
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
                Color = Global.NayuColor,
                Title = title,
                Description = desc,
            };
            return embed;
        }
    }
}
