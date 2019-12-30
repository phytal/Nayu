using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Preconditions;

namespace Nayu.Modules.Inbox
{
    public class Inbox : NayuModule
    {
        [Command("inbox")]
        [Summary("Shows your inbox of messages. Pageable to see older messages.")]
        [Alias("mail", "email", "messages", "msgs")]
        [Remarks("n!inbox <page number (if left empty it will default to 1)> Ex: n!inbox 2")]
        [Cooldown(5)]
        public async Task InboxCommand(int page = 1)
        {
            if (page < 1)
            {
                await ReplyAsync("Are you really trying that right now? ***REALLY?***");
                return;
            }

            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (!config.Inbox.Any())
            {
                await ReplyAsync("Your inbox is empty :c....");
                return;
            }

            const int msgsPerPage = 9;

            var lastPageNumber = 1 + config.Inbox.Count / (msgsPerPage + 1);
            if (page > lastPageNumber)
            {
                await ReplyAsync($"There are not that many pages...\nPage {lastPageNumber} is the last one...");
                return;
            }
            
            var ordered = config.Inbox.OrderByDescending(msg => msg.Time).ToList();
            ulong newMessages = config.InboxIDTracker - config.InboxIDLastRead;
            
            var embB = new EmbedBuilder()
                .WithTitle($"{Context.User}'s Inbox ({newMessages} new message{(newMessages == 1 ? "" : "s")})")
                .WithFooter($"Page {page}/{lastPageNumber} - {DateTime.Now:f}");

            page--;
            for (var i = 1; i <= msgsPerPage && i + msgsPerPage * page <= ordered.Count; i++)
            {
                var msg = ordered[i - 1 + msgsPerPage * page];
                embB.WithColor(37, 152, 255);
                string readIcon = null;
                if (!msg.Read) readIcon = ":small_blue_diamond: ";
                var difference = DateTime.Now - msg.Time;
                string time = null;
                if (difference.Days > 0) time = $"{difference.Days} day{(difference.Days == 1 ? "" : "s")} ago";
                else if (difference.Hours > 0)
                    time = $"{difference.Hours} hour{(difference.Hours == 1 ? "" : "s")} ago";
                else if (difference.Minutes > 0)
                    time = $"{difference.Minutes} minute{(difference.Minutes == 1 ? "" : "s")} ago";
                else if (difference.Seconds > 0)
                    time = $"{difference.Seconds} second{(difference.Seconds == 1 ? "" : "s")} ago";
                string content = msg.Content.Substring(0,msg.Content.Length > 50 ? 50 : msg.Content.Length);
                embB.AddField(readIcon + $"{msg.Title} - {time}", $"{content}{(msg.Content.Length > 50 ? "..." : "")}\n---\nID:{msg.ID}", true);
            }

            config.InboxIDLastRead = config.InboxIDTracker;
            GlobalUserAccounts.SaveAccounts();
            await ReplyAsync("", false, embB.Build());
        }
    }
}