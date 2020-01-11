using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Fun
{
    public class Unflip : NayuModule
    {
        [Subject(AdminCategories.FunStuff)]
        [Command("unflip"), Alias("uf")]
        [Summary("Enables or disables unflipping reactions for the server.")]
        [Remarks("n!uf <on/off> Ex: n!uf on")]
        [Cooldown(5)]
        public async Task UnflipCMD(string arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var result = ConvertBool.ConvertStringToBoolean(arg);
                if (result.Item1)
                {
                    bool argg = result.Item2;
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription(argg ? "I'll maintain your anger! **(Enabled unflipping for this server)**" : "You may freely rampage at your own will. **(Disabled unflipping for this server)**");
                    config.Unflip = argg;
                    GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);

                    await SendMessage(Context, embed.Build());
                }
                if (!result.Item1)
                {
                    await SendMessage(Context, null,$"Please say `n!uf <on/off>`");
                }
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
