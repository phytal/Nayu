using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Preconditions;

namespace Nayu.Modules.Inbox
{
    public class ClearInbox : NayuModule
    {
        [Command("clearinbox")]
        [Summary("Clears your entire inbox (irreversible)")]
        [Alias("ci")]
        [Remarks("n!clearinbox Ex: n!clearinbox")]
        [Cooldown(3)]
        public async Task ClearInboxCommand()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (!config.Inbox.Any())
                throw new ArgumentException("Your inbox is already empty!");

            await ReplyAsync("Are you sure you want to clear your inbox? (This cannot be reversed!)\nType `yes` or `no` to confirm your decision");
            var response = await NextMessageAsync();

            if (response.Content.Equals("yes", StringComparison.CurrentCultureIgnoreCase) &&
                (response.Author.Equals(Context.User)))
            {
                config.Inbox.Clear();
                GlobalUserAccounts.SaveAccounts(Context.User.Id);
                await ReplyAsync($"✅  | Cleared your inbox!");
            }

            else if (response.Content.Equals("no", StringComparison.CurrentCultureIgnoreCase) &&
                (response.Author.Equals(Context.User)))
            {
                await ReplyAsync($"✅ | Successfully canceled the action.");
            }
            else throw new ArgumentException("Please enter a valid response `(yes/no)`.");
        }
    }
}
