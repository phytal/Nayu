using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Helpers;

namespace Nayu.Modules.WelcomeAndLeave
{
    public class WelcomeMessages : NayuModule
    {
        [Subject(AdminCategories.WelcomeMessages)]
        [Command("welcome channel"), Alias("Wc")]
        [Summary("Set where you want welcome messages to be displayed")]
        [Remarks("n!wc channel <channel where welcome messages are> Ex: n!wc channel #general")]
        public async Task SetIdIntoConfig(SocketGuildChannel chnl)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var response = $"Set this guild's welcome channel to #{chnl}.";
            config.WelcomeChannel = chnl.Id;
            GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(AdminCategories.WelcomeMessages)]
        [Command("welcome add"), Alias("wa")]
        [Summary("Add a welcome message for people that join!")]
        [Remarks("n!wa <welcome message> Ex: n!wa <usermention>, welcome to **<guildname>**! " +
                 "`\n" +
                 "Possible placeholders are: `<usermention>`, `<username>`, `<guildname>`, " +
                 "`<botname>`, `<botdiscriminator>`, `<botmention>` ")]
        public async Task AddWelcomeMessage([Remainder] string message)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var response = $"Failed to add this Welcome Message...";
            if (guildAcc.WelcomeMessages.Contains(message) == false)
            {
                guildAcc.WelcomeMessages.Add(message);
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                response = $"Successfully added ```\n{message}\n``` as Welcome Message!";
            }

            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(AdminCategories.WelcomeMessages)]
        [Command("welcome remove"), Alias("wr")]
        [Summary("Removes a Welcome Message from the ones avaliable")]
        [Remarks("n!wr <welcome message number (can be shown with n!welcome list) Ex: n!wr 1")]
        public async Task RemoveWelcomeMessage(int messageIndex)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var messages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).WelcomeMessages;
            var response =
                $"Failed to remove this Welcome Message... Use the number shown in `welcome list` next to the `#` sign!";
            if (messages.Count > messageIndex - 1)
            {
                messages.RemoveAt(messageIndex - 1);
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                response = $"Successfully removed message #{messageIndex} as possible Welcome Message!";
            }

            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(AdminCategories.WelcomeMessages)]
        [Command("welcome list"), Alias("wl")]
        [Summary("Shows all currently set Welcome Messages")]
        [Remarks("n!wl")]
        public async Task ListWelcomeMessages()
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description =
                    $"{Global.ENo} **|** You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var welcomeMessages = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id).WelcomeMessages;
            var embed = new EmbedBuilder().WithTitle(
                "No Welcome Messages set yet... add some if you want to greet incoming people!");
            if (welcomeMessages.Count > 0) embed.WithTitle("Possible Welcome Messages:");
            embed.WithColor(Global.NayuColor);
            
            for (var i = 0; i < welcomeMessages.Count; i++)
            {
                embed.AddField($"Message #{i + 1}:", welcomeMessages[i], true);
            }

            await ReplyAsync("", false, embed.Build());
        }
    }
}
