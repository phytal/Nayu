using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Preconditions;

namespace Nayu.Modules.Inbox
{
    public class DeleteMessage : NayuModule
    {
        [Command("deletemessage")]
        [Summary("Delete a message in your inbox through its ID")]
        [Alias("dm")]
        [Remarks("n!dm <message ID> Ex: n!dm 2")]
        [Cooldown(3)]
        public async Task DeleteMessageCommand(ulong id)
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (id < 1 || id > config.InboxIDTracker) throw new ArgumentException("Please enter a valid ID");
            
            if (!config.Inbox.Any()) throw new ArgumentException("You don't have any messages to delete! (Empty Inbox)");

            var validMessages = config.Inbox.Where(i => i.ID == id);
            
            foreach (var msg in validMessages)
            {
                config.Inbox.Remove(msg);
                GlobalUserAccounts.SaveAccounts(id);
                await ReplyAsync($"Deleted message **({msg.Title})**.");
                break;
            }
        }
    }
}