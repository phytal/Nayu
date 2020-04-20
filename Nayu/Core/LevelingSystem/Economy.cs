using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.Economy;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules;
using Nayu.Preconditions;
using Nayu.Helpers;

namespace Nayu.Core.LevelingSystem
{
    public class Economy : NayuModule
    {
        [Subject(Categories.EconomyGambling)]
        [Command("daily")]
        [Alias("GetDaily", "ClaimDaily")]
        [Summary("Claims your daily Taiyakis!")]
        [Remarks("Ex: n!daily")]
        [Cooldown(60)]
        public async Task GetDaily()
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var result = Daily.GetDaily(Context.User.Id);

            if (result.Success)
            {
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithDescription(
                    $"{Global.ETaiyaki}  **|** Here's **{Constants.DailyTaiyakiGain}** {config.Currency}, {Context.User.Mention}! Come back tomorrow for more!");
                await SendMessage(Context, embed.Build());
            }
            else
            {
                var timeSpanString =
                    string.Format("{0:%h} hours {0:%m} minutes {0:%s} seconds", result.RefreshTimeSpan);
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithDescription(
                    $"{Global.ETaiyaki}  **|** **You have already claimed your free daily {config.Currency}, {Context.User.Mention}.\nCome back in {timeSpanString}.**");
                await SendMessage(Context, embed.Build());
            }
        }

        [Subject(Categories.EconomyGambling)]
        [Command("reputation")]
        [Alias("rep")]
        [Summary("Gives a mentioned user reputation points, you can only use this once every 24 hours.")]
        [Remarks("n!rep <person you want to rep> Ex: n!rep @Phytal")]
        [Cooldown(30)]
        public async Task GetRep([NoSelf] SocketGuildUser userB)
        {
            var result = Daily.GetRep((SocketGuildUser) Context.User);

            if (result.Success)
            {
                var mentionedAccount = GlobalGuildUserAccounts.GetUserID(userB);
                mentionedAccount.Reputation += 1;
                GlobalGuildUserAccounts.SaveAccounts();
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithDescription(
                    $":diamond_shape_with_a_dot_inside:   **|** {Context.User.Mention} gave {userB.Mention} a reputation point!");
                await SendMessage(Context, embed.Build());
            }
            else
            {
                var timeSpanString =
                    string.Format("{0:%h} hours {0:%m} minutes {0:%s} seconds", result.RefreshTimeSpan);
                var embed = new EmbedBuilder();
                embed.WithColor(Global.NayuColor);
                embed.WithDescription(
                    $":diamond_shape_with_a_dot_inside::arrows_counterclockwise:  | **You already gave someone reputation points recently, {Context.User.Mention}.\nCome back in {timeSpanString}.**");
                await SendMessage(Context, embed.Build());
            }
        }

        [Subject(Categories.EconomyGambling)]
        [Command("gift")]
        [Alias("grant", "pay")]
        [Summary(
            "Gifts/Pays Taiyakis to a selected user (of course taken from your balance) Ex: n!gift <amount of Taiyakis> @user")]
        [Remarks("n!gift <amount> <user you want to gift to> Ex: n!gift 500 @Phytal")]
        [Cooldown(5)]
        public async Task Gift(uint taiyaki, IGuildUser userB)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var giver = GlobalUserAccounts.GetUserAccount(Context.User);

            if (giver.Taiyaki < taiyaki)
            {
                await ReplyAsync(
                    $":angry:  **|** Stop trying to gift an amount of {config.Currency} over your account balance! ");
            }
            else
            {
                if (userB == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(Global.NayuColor);
                    embed.WithTitle(
                        $"🖐️ **|** Please say who you want to gift {config.Currency} to. Ex: n!gift <amount of {config.Currency}s> @user");
                    await SendMessage(Context, embed.Build());
                }
                else
                {
                    var recipient = GlobalUserAccounts.GetUserAccount((SocketUser) userB);

                    giver.Taiyaki -= taiyaki;
                    recipient.Taiyaki += taiyaki;
                    GlobalUserAccounts.SaveAccounts(giver.Id, recipient.Id);

                    await SendMessage(Context, null,
                        $"✅  **|** {Context.User.Mention} has gifted {userB.Mention} {taiyaki} {config.Currency}(s). How generous.");
                }
            }
        }

        [Subject(OwnerCategories.Owner)]
        [Command("addTaiyakis")]
        [Summary("Grants Taiyakis to selected user")]
        [Alias("giveptaiyakis")]
        [RequireOwner]
        public async Task AddTaiyakis(uint Taiyaki, IGuildUser user)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            SocketUser target;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var userAccount = GlobalUserAccounts.GetUserAccount((SocketUser) user);

            userAccount.Taiyaki += Taiyaki;
            GlobalUserAccounts.SaveAccounts(userAccount.Id);

            var embed = new EmbedBuilder();
            embed.WithColor(Global.NayuColor);
            embed.WithTitle(
                $"✅  **|** **{Taiyaki}** {config.Currency} were added to " + target.Username + "'s account.");
            await SendMessage(Context, embed.Build());
        }

        [Subject(Categories.EconomyGambling)]
        [Command("levels")]
        [Summary("Shows a user list of the sorted by currency. Pageable to see lower ranked users.")]
        [Alias("Top", "Top10", "richest", "rank")]
        [Remarks("n!level <page number (if left empty it will default to 1)> Ex: n!levels 2")]
        [Cooldown(15)]
        public async Task ShowRichesPeople(int page = 1)
        {
            if (page < 1)
            {
                await ReplyAsync("Are you really trying that right now? ***REALLY?***");
                return;
            }

            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
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

            // Sort the accounts descending by currency
            var ordered = accounts.OrderByDescending(acc => acc.Taiyaki).ToList();

            var embB = new EmbedBuilder()
                .WithTitle($"{config.Currency} Leaderboard:")
                .WithFooter($"Page {page}/{lastPageNumber}");

            page--;
            for (var i = 1; i <= usersPerPage && i + usersPerPage * page <= ordered.Count; i++)
            {
                var account = ordered[i - 1 + usersPerPage * page];
                var user = Global.Client.GetUser(account.Id);
                embB.WithColor(Global.NayuColor);
                embB.AddField($"#{i + usersPerPage * page} {user.Username}", $"{account.Taiyaki} {config.Currency}",
                    true);
            }

            await ReplyAsync("", false, embB.Build());
        }

        [Subject(Categories.EconomyGambling)]
        [Command("balance")]
        [Alias("Cash", "Taiyaki", "bal")]
        [Summary("Checks the balance for your, or an mentioned account")]
        [Remarks("n!bal <person you want to check (will default to you if left empty)> Ex: n!bal @Phytal")]
        [Cooldown(5)]
        public async Task CheckTaiyakis([Remainder] string arg = "")
        {
            SocketUser target;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var account = GlobalUserAccounts.GetUserAccount(target.Id);
            await SendMessage(Context, null,
                $"{Global.ETaiyaki}  **|** {target.Mention} has **{account.Taiyaki} {config.Currency}**!");
        }
    }
}