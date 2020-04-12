using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Helpers;
using Nayu.Modules;
using Nayu.Preconditions;

namespace Nayu.Core.LevelingSystem
{
    public class StatsModule : NayuModule
    {
        [Subject(Categories.EconomyGambling)]
        [Command("stats")]
        [Summary("Checks your stats (level, xp, reputation)")]
        [Alias("userstats")]
        [Remarks("n!stats <person you want to check(will default to you if left empty)> Ex: n!stats @Phytal")]
        [Cooldown(5)]
        public async Task Stats([Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            var userAccountt = GlobalGuildUserAccounts.GetUserId((SocketGuildUser) target);
            var userAccount = GlobalUserAccounts.GetUserAccount(target);

            uint Level = userAccount.LevelNumber;
            var xp = userAccount.XP;
            var requiredXp = (Math.Pow(userAccount.LevelNumber + 1, 2) * 50);
            var thumbnailurl = target.GetAvatarUrl();
            var auth = new EmbedAuthorBuilder()
            {
                Name = target.Username,
                IconUrl = thumbnailurl,
            };

            var embed = new EmbedBuilder()
            {
                Author = auth
            };

            embed.WithColor(Global.NayuColor);
            embed.AddField("Affection Lvl.", Level, true);
            embed.AddField("Exp.", $"{xp}/{requiredXp} (tot. {userAccount.XP})", true);
            embed.AddField("Reputation Points", userAccountt.Reputation, true);
            embed.AddField("Taiyaki", userAccount.Taiyaki, true);
            embed.AddField("Taiyaki From Messages", userAccount.TaiyakiFromMessages, true);
            embed.AddField("Taiyaki From Gambling", userAccount.TaiyakiFromGambling, true);
            embed.AddField("Active Chomusuke", userAccount.ActiveChomusuke);
            await SendMessage(Context, embed.Build());
        }
    }
}