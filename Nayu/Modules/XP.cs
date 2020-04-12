using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Helpers;

namespace Nayu.Modules
{
    public class Xp : NayuModule
    {
        [Subject(OwnerCategories.Owner)]
        [Command("addXp")]
        [Summary("Grants Xp/Exp to selected user")]
        [Alias("giveXp", "giveExp", "addExp")]
        [RequireOwner]
        public async Task AddXp(uint xp, SocketGuildUser user)
        {
            var userAccount = GlobalUserAccounts.GetUserAccount(user);
            userAccount.Xp += xp;
            GlobalGuildUserAccounts.SaveAccounts(user);
            
            var embed = EmbedHandler.CreateEmbed(Context, "Success!",
                $"✅  **|** **{xp}** xp were added to {user.Username}'s account.",
                EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(OwnerCategories.Owner)]
        [Command("addRep")]
        [Summary("Grants reputation points to selected user")]
        [Alias("givePoints")]
        [RequireOwner]
        public async Task AddPoints(uint points, SocketGuildUser user)
        {
            var userAccount = GlobalGuildUserAccounts.GetUserId(user);
            userAccount.Reputation += points;
            GlobalGuildUserAccounts.SaveAccounts(user);

            var embed = EmbedHandler.CreateEmbed(Context, "Success!",
                $"✅  **| {points}** reputation points were added to {user.Username}'s account.",
                EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }
    }
}