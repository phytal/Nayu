using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Fun
{
    public class CustomCurrency : NayuModule
    {
        [Command("customcurrency"), Alias("cc")]
        [Summary("Make a custom currency for the server! (Defaulted to Taiyakis)")]
        [Remarks("n!cc <name of your custom currency> Ex: n!cc Credits")]
        [Cooldown(5)]
        public async Task CustomCurrencyCMD([Remainder]string arg)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                    var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.WithDescription($"The server's currency is now set to the **{arg}**!");
                    config.Currency = arg;
                    GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);

                    await SendMessage(Context, embed.Build());
                if (arg == string.Empty)
                {
                    await SendMessage(Context, null, $"The server currency is now set to the default **Taiyaki** To change this, you can use `n!cc <name of your custom currency>`");
                    config.Currency = "Taiyakis";
                    GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
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
