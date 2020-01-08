using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Libs.CustomLibraries.Discord.Addons.Interactive;

namespace Nayu.Modules
{
    public abstract class NayuModule : InteractiveBase
    {
        public bool Enabled = true;
        public bool Disabled = false;
        public int Zero = 0;

        protected static async Task SendMessage(ShardedCommandContext ctx, Embed embed = null, string msg = "")
        {
            if (embed == null)
            {
                await ctx.Channel.SendMessageAsync(msg);
            }
            else
            {
                await ctx.Channel.SendMessageAsync(msg, false, embed);
            }
        }
    }
}
