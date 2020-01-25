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
using Nayu.Core.Handlers;
using Nayu.Helpers;

namespace Nayu.Modules
{
    public class XP : NayuModule
    {
        [Subject(OwnerCategories.Owner)]
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
            var embed = EmbedHandler.CreateEmbed(Context, "Success!", $"✅  **|** **{xp}** xp were added to {target.Username}'s account.", EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(OwnerCategories.Owner)]
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

            
            var embed = EmbedHandler.CreateEmbed(Context, "Success!", $"✅  **|** **{Points}** reputation points were added to {target.Username}'s account.", EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }
    }
}

