using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.Inbox
{
    public class OpenMessage : NayuModule
    {
        [Subject(Categories.Inbox)]
        [Command("openMessage")]
        [Summary("Open a message in your inbox through its ID")]
        [Alias("om")]
        [Remarks("n!om <message ID> Ex: n!om 2")]
        [Cooldown(5)]
        public async Task OpenMessageCommand(ulong id)
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (id < 1 || id > config.InboxIDTracker) throw new ArgumentException("Please enter a valid ID");

            if (!config.Inbox.Any()) throw new ArgumentException("You don't have any messages to open! (Empty Inbox)");

            var validMessages = config.Inbox.Where(i => i.ID == id);

            foreach (var msg in validMessages)
            {
                string readIcon = null;
                if (!msg.Read) readIcon = ":small_blue_diamond: ";
                var embB = new EmbedBuilder()
                    .WithTitle($"{readIcon}{msg.Title}")
                    .WithDescription(msg.Content)
                    .WithFooter($"Accessed {DateTime.Now:f}")
                    .WithColor(Global.NayuColor);
                msg.Read = true;
                GlobalUserAccounts.SaveAccounts(Context.User.Id);
                await ReplyAsync("", false, embB.Build());
                break;
            }
        }
    }
}