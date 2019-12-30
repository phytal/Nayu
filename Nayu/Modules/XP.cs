using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Net;
using Nayu.Core.Features.GlobalAccounts;

namespace Nayu.Modules
{
    public class XP : NayuModule
    {
        [Command("addXP")]
        [Summary("Grants XP/Exp to selected user")]
        [Alias("givexp", "giveexp", "addexp")]
        [RequireOwner]
        public async Task AddXP(uint xp, IGuildUser user, [Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var userAccount = GlobalUserAccounts.GetUserAccount((SocketGuildUser)user);

            userAccount.XP += xp;
            GlobalGuildUserAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync($":white_check_mark:  | **{xp}** Exp were added to " + target.Username + "'s account.");
        }

        [Command("addrep")]
        [Summary("Grants reputation points to selected user")]
        [Alias("givepoints")]
        [RequireOwner]
        public async Task AddPoints(uint Points, SocketGuildUser user, [Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var userAccount = GlobalGuildUserAccounts.GetUserID(user);

            userAccount.Reputation += Points;
            GlobalGuildUserAccounts.SaveAccounts();

            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle($":white_check_mark:  | **{Points}** reputation points were added to " + target.Username + "'s account.");
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }
    }
}

