using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Management
{
    public class CustomCommand : NayuModule
    {
        [Subject(AdminCategories.FunStuff)]
        [Command("CustomCommandAdd")]
        [Alias("Cca")]
        [Summary("Add a custom command")]
        [Remarks("n!cca <command name> <bot response> Ex: n!cca whatsup hey man")]
        [Cooldown(5)]
        public async Task AddCustomCommand(string commandName, [Remainder] string commandValue)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            config.CustomCommands.Add(commandName, commandValue);
            GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
            var embed = new EmbedBuilder()
                .WithTitle("Custom Command Added!")
                .AddField("Command Name", $"__{commandName}__")
                .AddField("Bot Response", $"**{commandValue}**")
                .WithColor(37, 152, 255);

            await SendMessage(Context, embed.Build());
        }

        [Subject(AdminCategories.FunStuff)]
        [Command("CustomCommandRemove")]
        [Alias("Ccr")]
        [Summary("Remove a custom command")]
        [Remarks("n!ccr <custom command name you want to remove> Ex: n!ccr whatsup")]
        public async Task RemCustomCommand(string commandName)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} | You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var embed = new EmbedBuilder()
                .WithColor(37, 152, 255);
            if (config.CustomCommands.Keys.Contains(commandName))
            {
                embed.WithDescription($"Removed **{commandName}** as a command!");
                config.CustomCommands.Remove(commandName);
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
            }
            else
            {
                embed.WithDescription($"**{commandName}** isn't a command on this server.");
            }

            await SendMessage(Context, embed.Build());
        }

        [Subject(AdminCategories.FunStuff)]
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

            await SendMessage(Context, embed.Build());
        }
    }
}
