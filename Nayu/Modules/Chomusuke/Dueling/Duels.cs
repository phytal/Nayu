using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Preconditions;

namespace Nayu.Modules.Chomusuke.Dueling
{
    public struct PendingDuelBase
    {
        public ulong PlayerId { get; set; }
        public ulong Requester { get; set; }
        public RestUserMessage Message { get; set; }
    }
    public static class PendingDuelProvider
    {
        public static List<PendingDuelBase> games;

        static PendingDuelProvider()
        {
            games = new List<PendingDuelBase>();
        }

        public static bool UserIsPlaying(ulong userId)
        {
            return games.Any(g => g.PlayerId == userId);
        }

        public static ulong RequestUser(ulong userId)
        {
            var game = games.FirstOrDefault(g => g.PlayerId == userId);
            return game.Requester;
        }

        public static void CreateNewGame(ulong userId, ulong req, RestUserMessage message)
        {
            var game = new PendingDuelBase()
            {
                PlayerId = userId,
                Message = message,
                Requester = req 
            };

            games.Add(game);
        }
    }
    public class Duel : NayuModule
    {
        [Command("duelHelp")]
        [Alias("duelsHelp", "helpDuel", "dHelp")]
        [Summary("Shows all possible commands for dueling")]
        [Remarks("Ex: n!dHelp")]
        public async Task DuelHelp()
        {
            string[] footers = new string[]
{
                "If you want to gain health but still want to do damage, use n!absorb!",
                "You have a maximum of 6 Med Kits you can use in battle, use them wisely!",
                "",
                                "If you want to deflect some damage from your opponent's next attack, use n!deflect!",
                                "Each duel command has a cooldown of 3 seconds.",
                                "Absorbing is powerful, but it is rare that it can hit the target."
};
            Random rand = new Random();
            int randomIndex = rand.Next(footers.Length);
            string text = footers[randomIndex];

            var embed = new EmbedBuilder();
            embed.WithTitle(":crossed_swords:  Duel Command List / Help");
            embed.AddField("n!duels help", "Brings up the help commmand (lol)", true);
            embed.AddField("n!duel", "Starts a duel with the confirmation specified user!", true);
            embed.AddField("n!engage", "Enables you to attack, use an item, or give up in a duel!", true);
            embed.AddField("n!duelsBuy", "Opens the duel shop interface.", true);
            embed.AddField("n!duelsInventory", "View all of your items for duels.", true);
            embed.AddField("n!duelsLeaderboard", "Views the dueling leaderboard for your guild.", true);
            embed.AddField("n!globalDuelsLeaderboard", "Views the dueling leaderboard for the world.", true);
            embed.AddField("n!duelStats", "View yours or someone else's stats for duels.", true);
            embed.AddField("n!attacks", "Views your 4 attacks.", true);
            embed.AddField("n!replaceattack", "Replaces an attack.", true);
            embed.AddField("n!blessings", "Shows all of your possessed blessings.", true);
            embed.AddField("n!activeblessing", "Set your active blessing.", true);
            embed.WithFooter(text);
            embed.WithColor(Global.NayuColor);
            await SendMessage(Context, embed.Build());
        }

        [Command("duel")]
        [Alias("Duel", "dual")]
        [Summary("Starts a duel with the specified user!")]
        [Remarks("n!duel <user you want to duel> Ex: n!duel @Phytal")]
        public async Task Pvp(SocketGuildUser user)
        {
            var config = GlobalUserAccounts.GetUserAccount((SocketGuildUser)Context.User);
            var player2 = user.Guild.GetUser(user.Id);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            if (config.ActiveChomusuke == 0 || configg.ActiveChomusuke == 0)
            {
                await ReplyAndDeleteAsync($"{Global.ENo} **|** Both players need an active chomusuke to start the duel!");
                return;
            }
            if (!config.Fighting && configg.Fighting)
            {
                if (PendingDuelProvider.UserIsPlaying(Context.User.Id))
                {
                    await ReplyAndDeleteAsync("You already sent a duel request to someone. Cancel it with `n!duelCancel` or wait until your opponent accepts your duel request.");
                    return;
                }
                var msg = await Context.Channel.SendMessageAsync($"{Context.User.Mention} challenges {user.Mention} to a duel! {user.Username}, do you accept? (react with the emojis)");
                await msg.AddReactionAsync(new Emoji("✅"));
                await msg.AddReactionAsync(Global.ENo);
                PendingDuelProvider.CreateNewGame(user.Id, Context.User.Id, msg);
            }
            else
            {
                await ReplyAsync(":expressionless:  **|** " + Context.User.Mention + ", sorry, either you or your opponent are currently fighting or you just tried to fight yourself...");
            }
        }

        [Command("duelStats")]
        [Alias("ds")]
        [Summary("Gets yours or someone else's duel stats!")]
        [Remarks("n!duelStats <target user (will be defaulted to you if left empty)> Ex: n!duelStats @Phytal")]
        public async Task DuelStats([Remainder] string user = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var config = GlobalUserAccounts.GetUserAccount(target);
            var embed = new EmbedBuilder();
            embed.WithColor(Global.NayuColor);
            embed.WithTitle($"{Context.User.Username}'s Duel Stats :crossed_swords: ");
            embed.AddField("**Wins**", config.Wins);
            embed.AddField("**Losses**", config.Losses);
            embed.AddField("**Win Streak**", config.WinStreak);
            await SendMessage(Context, embed.Build());
        }

        public static async Task StartDuel(ISocketMessageChannel channel, SocketGuildUser user, SocketUser req)
        {
            var config = GlobalUserAccounts.GetUserAccount(req);
            var player2 = user.Guild.GetUser(user.Id);
            var configg = GlobalUserAccounts.GetUserAccount(player2);

            configg.OpponentId = req.Id;
            config.OpponentId = user.Id;
            configg.OpponentName = req.Username;
            config.OpponentName = user.Username;
            config.Fighting = true;
            configg.Fighting = true;


            string[] whoStarts =
            {
                    req.Mention,
                    user.Mention
            };

            Random rand = new Random();

            int randomIndex = rand.Next(whoStarts.Length);
            string text = whoStarts[randomIndex];

            config.WhosTurn = text;
            configg.WhosTurn = text;
            if (text == req.Mention)
            {
                config.WhoWaits = user.Mention;
                configg.WhoWaits = user.Mention;
            }
            else
            {
                config.WhoWaits = req.Mention;
                configg.WhoWaits = req.Mention;
            }
            GlobalUserAccounts.SaveAccounts(config.Id, configg.Id);
            var choms = ActiveChomusuke.GetActiveChomusuke(user.Id, config.OpponentId);
            var chom1 = choms.Item1;
            var chom2 = choms.Item2;
            await channel.SendMessageAsync($":crossed_swords:  **|** {req.Mention} challenged {user.Mention} to a duel!\n\n**{chom1.Name}** has **{chom1.Health}** health!\n**{chom2.Name}** has **{chom2.Health}** health!\n\n{text}, you go first!");
        }

        [Command("duelsLeaderboard"), Alias("dlb")]
        [Summary("Shows a leaderboard sorted by duel wins. Pageable to see lower ranked users.")]
        [Remarks("n!dlb <page number (if left empty it will default to 1)> Ex: n!dlb 2")]
        [Cooldown(15)]
        public async Task DuelsLeaderBoard(int page = 1)
        {
            if (page < 1)
            {
                await ReplyAsync("Are you really trying that right now? No book has less than 1 page..");
                return;
            }

            var guildUserIds = Context.Guild.Users.Select(user => user.Id);
            var accounts = GlobalUserAccounts.GetFilteredAccounts(acc => guildUserIds.Contains(acc.Id));

            const int usersPerPage = 9;
            // Calculate the highest accepted page number => amount of pages we need to be able to fit all users in them
            // (amount of users) / (how many to show per page + 1) results in +1 page more every time we exceed our usersPerPage  
            var lastPageNumber = 1 + (accounts.Count / (usersPerPage + 1));
            if (page > lastPageNumber)
            {
                await ReplyAsync($"There are not that many pages...\nPage {lastPageNumber} is the last one...");
                return;
            }
            // Sort the accounts descending by Duel wins
            var ordered = accounts.OrderByDescending(acc => acc.Wins).ToList();

            var embed = new EmbedBuilder()
                .WithTitle($"Duels Leaderboard:")
                .WithFooter($"Page {page}/{lastPageNumber}");

            page--;
            for (var i = 1; i <= usersPerPage && i + usersPerPage * page <= ordered.Count; i++)
            {
                var account = ordered[i - 1 + usersPerPage * page];
                var user = Global.Client.GetUser(account.Id);
                embed.WithColor(Global.NayuColor);
                embed.AddField($"#{i + usersPerPage * page} {user.Username}", $"{account.Wins} Wins", true);
            }

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("globalDuelsLeaderboard"), Alias("gdlb")]
        [Summary("Shows a global leaderboard sorted by duel wins. Pageable to see lower ranked users.")]
        [Remarks("n!gdlb <page number (if left empty it will default to 1)> Ex: n!gdlb 2")]
        [Cooldown(15)]
        public async Task GDLB(int page = 1)
        {
            if (page < 1)
            {
                await ReplyAsync("Are you really trying that right now? ***REALLY?***");
                return;
            }

            var accounts = GlobalUserAccounts.GetAllAccounts();

            const int usersPerPage = 9;
            // Calculate the highest accepted page number => amount of pages we need to be able to fit all users in them
            // (amount of users) / (how many to show per page + 1) results in +1 page more every time we exceed our usersPerPage  
            var lastPageNumber = 1 + (accounts.Count / (usersPerPage + 1));
            if (page > lastPageNumber)
            {
                await ReplyAsync($"There are not that many pages...\nPage {lastPageNumber} is the last one...");
                return;
            }
            // Sort the accounts descending by duel wins
            var ordered = accounts.OrderByDescending(acc => acc.Wins).ToList();

            var embed = new EmbedBuilder()
                .WithTitle($"Global Duels Leaderboard:")
                .WithFooter($"Page {page}/{lastPageNumber}");

            page--;
            for (var i = 1; i <= usersPerPage && i + usersPerPage * page <= ordered.Count; i++)
            {
                var account = ordered[i - 1 + usersPerPage * page];
                var user = Global.Client.GetUser(account.Id);
                embed.WithColor(Global.NayuColor);
                embed.AddField($"#{i + usersPerPage * page} {user.Username}", $"{account.Wins} Wins", true);
            }

            await ReplyAsync("", embed: embed.Build());
        }

        [Command("endfight")]
        [Summary("Ends the fight between users (DEBUG PURPOSES ONLY)")]
        [Remarks("n!endfight")]
        [Cooldown(15)]
        public async Task Endfight()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            var configg = GlobalUserAccounts.GetUserAccount(config.OpponentId);
            await ReplyAsync(":flag_white:  **|** " + Context.User.Mention + " ended the fight."); 
            var choms = ActiveChomusuke.GetActiveChomusuke(Context.User.Id, config.OpponentId);
            var chom1 = choms.Item1;
            var chom2 = choms.Item2;
            
            config.Fighting = false;
            configg.Fighting = false;
            chom1.Health = chom1.HealthCapacity;
            chom2.Health = chom2.HealthCapacity;
            config.OpponentId = 0;
            configg.OpponentId = 0;
            config.OpponentName = null;
            configg.OpponentName = null;
            config.WhosTurn = null;
            config.WhoWaits = null;
            config.PlaceHolder = null;
            configg.WhosTurn = null;
            configg.WhoWaits = null;
            chom1.Effects.Clear();
            chom2.Effects.Clear();
            chom1.PotionEffects.Clear();
            chom2.PotionEffects.Clear();

            GlobalUserAccounts.SaveAccounts(config.Id, configg.Id);
        }
    }
}

