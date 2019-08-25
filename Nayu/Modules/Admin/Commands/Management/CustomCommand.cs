using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;
using Nayu.Preconditions;

namespace Nayu.Modules.Management.Commands
{
    public class CustomCommand : NayuModule
    {
        [Command("CustomCommandAdd")]
        [Alias("Cca")]
        [Summary("Add a custom command")]
        [Remarks("n!cca <command name> <bot response> Ex: n!cca whatsup hey man")]
        [Cooldown(5)]
        public async Task AddCustomCommand(string commandName, [Remainder] string commandValue)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                config.CustomCommands.Add(commandName, commandValue);
                GlobalGuildAccounts.SaveAccounts();
                var embed = new EmbedBuilder()
                    .WithTitle("Custom Command Added!")
                    .AddField("Command Name", $"__{commandName}__")
                    .AddField("Bot Response", $"**{commandValue}**")
                    .WithColor(37, 152, 255);

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("CustomCommandRemove")]
        [Alias("Ccr")]
        [Summary("Remove a custom command")]
        [Remarks("n!ccr <custom command name you want to remove> Ex: n!ccr whatsup")]
        public async Task RemCustomCommand(string commandName)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.Administrator)
            {
                var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                var embed = new EmbedBuilder()
                    .WithColor(37, 152, 255);
                if (config.CustomCommands.Keys.Contains(commandName))
                {
                    embed.WithDescription($"Removed **{commandName}** as a command!");
                    config.CustomCommands.Remove(commandName);
                    GlobalGuildAccounts.SaveAccounts();
                }
                else
                {
                    embed.WithDescription($"**{commandName}** isn't a command on this server.");
                }

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("CustomCommandList")]
        [Alias("Ccl")]
        [Summary("Lists all custom commands and its outputs")]
        [Remarks("n!ccr <custom command name you want to remove> Ex: n!ccr whatsup")]
        public async Task CustomCommandList()
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var cmds = config.CustomCommands;
            var embed = new EmbedBuilder().WithTitle("No custom commands set up yet... add some!");
            if (cmds.Count > 0) embed.WithTitle("Here are all available custom commands:");

            foreach (var cmd in cmds)
            {
                embed.AddField(cmd.Key, cmd.Value, true);
            }

            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }
    }
}
