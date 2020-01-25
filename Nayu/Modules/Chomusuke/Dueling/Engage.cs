using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Helpers;

namespace Nayu.Modules.Chomusuke.Dueling
{
    public class Engage : NayuModule
    {        
        [Subject(ChomusukeCategories.Chomusuke)]
        [Command("engage"), Alias("attack", "item", "giveup")]
        [Summary("Opens the duels engagment GUI")]
        [Remarks("Ex: n!engage")]
        public async Task EngageCommand()
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalUserAccounts.GetUserAccount((SocketGuildUser) Context.User);

            var choms = ActiveChomusuke.GetActiveChomusuke(Context.User.Id, config.OpponentId);
            var activeChomusuke = choms.Item1;
            var activeChomusukee = choms.Item2;

            if (config.Fighting != true)
            {
                await ReplyAndDeleteAsync($"**{Context.User.Username}** there is no fight going on!", false, null,
                    TimeSpan.FromSeconds(5), null);
                return;
            }

            var player2 = user.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);

            if (config.WhosTurn != user.Mention)
            {
                await ReplyAndDeleteAsync($"**{Context.User.Username}** it is not your turn!", false, null,
                    TimeSpan.FromSeconds(5), null);
                return;
            }

            var embed = new EmbedBuilder()
                .WithColor(Global.NayuColor)
                .WithFooter("Type [cancel] anytime to cancel the engagement.");
            embed.WithTitle("What do you want to do?");
            embed.AddField("[1]", "**ATTACK**");
            embed.AddField("[2]", "**ITEM**");
            embed.AddField("[3]", "**GIVE UP**");

            var gui = await Context.Channel.SendMessageAsync("", embed: embed.Build());
            var response = await NextMessageAsync();

            if (response == null)
            {
                await gui.DeleteAsync();
                await SendMessage(Context, null, 
                    $"{Context.User.Mention},The interface has closed due to inactivity");
                return;
            }

            if (response.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) &&
                (response.Author.Equals(Context.User)))
            {
                var embeddd = new EmbedBuilder()
                    .WithColor(Global.NayuColor)
                    .WithFooter("Type [cancel] anytime to cancel the engagement.");

                embeddd.AddField("[1]", activeChomusuke.Attack1);
                embeddd.AddField("[2]", activeChomusuke.Attack2);
                embeddd.AddField("[3]", activeChomusuke.Attack3);
                embeddd.AddField("[4]", activeChomusuke.Attack4);
                var newGui = await Context.Channel.SendMessageAsync("", embed: embeddd.Build());

                var newresponse = await NextMessageAsync();
                if (newresponse == null)
                {
                    await gui.DeleteAsync();
                    await newGui.DeleteAsync();
                    await SendMessage(Context, null, 
                        $"{Context.User.Mention},The interface has closed due to inactivity");
                }

                else if (newresponse.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) &&
                         (response.Author.Equals(Context.User)))
                {
                    var result = Helpers.ExecuteAttack(Context, activeChomusuke, activeChomusuke.Attack1);

                    if (!result.Success)
                    {
                        await SendMessage(Context, null, result.Response);
                        return;
                    }

                    await SendMessage(Context, null, result.Response);

                    Tuple<bool, string> death = await CheckDeath(Context, activeChomusuke, activeChomusukee);
                    if (death.Item1)
                    {
                        await SendMessage(Context, null, death.Item2);
                        return;
                    }

                    var turnmsg = await NextTurn(Context);
                    await SendMessage(Context, null, turnmsg);
                }

                else if (newresponse.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) &&
                         (response.Author.Equals(Context.User)))
                {
                    var result = Helpers.ExecuteAttack(Context, activeChomusuke, activeChomusuke.Attack2);

                    if (!result.Success)
                    {
                        await SendMessage(Context, null, result.Response);
                        return;
                    }

                    await SendMessage(Context, null, result.Response);

                    Tuple<bool, string> death = await CheckDeath(Context, activeChomusuke, activeChomusukee);
                    if (death.Item1)
                    {
                        await SendMessage(Context, null, death.Item2);
                        return;
                    }

                    var turnmsg = await NextTurn(Context);
                    await SendMessage(Context, null, turnmsg);
                }

                else if (newresponse.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) &&
                         (response.Author.Equals(Context.User)))
                {
                    var result = Helpers.ExecuteAttack(Context, activeChomusuke, activeChomusuke.Attack3);

                    if (!result.Success)
                    {
                        await SendMessage(Context, null, result.Response);
                        return;
                    }

                    await SendMessage(Context, null, result.Response);

                    Tuple<bool, string> death = await CheckDeath(Context, activeChomusuke, activeChomusukee);
                    if (death.Item1)
                    {
                        await SendMessage(Context, null, death.Item2);
                        return;
                    }

                    var turnmsg = await NextTurn(Context);
                    await SendMessage(Context, null, turnmsg);
                }

                else if (newresponse.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) &&
                         (response.Author.Equals(Context.User)))
                {
                    var result = Helpers.ExecuteAttack(Context, activeChomusuke, activeChomusuke.Attack4);

                    if (!result.Success)
                    {
                        await SendMessage(Context, null, result.Response);
                        return;
                    }

                    await SendMessage(Context, null, result.Response);

                    Tuple<bool, string> death = await CheckDeath(Context, activeChomusuke, activeChomusukee);
                    if (death.Item1)
                    {
                        await SendMessage(Context, null, death.Item2);
                        return;
                    }

                    var turnmsg = await NextTurn(Context);
                    await SendMessage(Context, null, turnmsg);
                }

                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) &&
                    (response.Author.Equals(Context.User)))
                {
                    await gui.ModifyAsync(m =>
                    {
                        m.Content = $":shield:  **|**  **{Context.User.Username}**, engagement cancelled.";
                    });
                    return;
                }

                await gui.ModifyAsync(m =>
                {
                    m.Content = $"{Global.ENo} **|** That is an invalid response. Please try again.";
                });
                return;
            }

            if (response.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) &&
                (response.Author.Equals(Context.User)))
            {
                var values = 0;
                var embedd = new EmbedBuilder()
                    .WithColor(Global.NayuColor)
                    .WithFooter("Type [cancel] anytime to cancel the engagement.");

                embedd.WithTitle("Which item would you like to use?");
                embedd.WithDescription("These are all of the items you currently have");
                int numkeys = 0;
                int numkeys2 = 0;
                int[] num = Enumerable.Range(0, config.Items.Keys.Count + 2).ToArray();
                foreach (var keys in config.Items.Keys)
                {
                    var entry = config.Items[keys];
                    if (entry == 0) continue;
                    values = Helpers.GetValueFromKey(keys, user);

                    int newnum = Helpers.GetNextElement(num, numkeys);
                    numkeys++;
                    embedd.AddField($"[{newnum}]", $"{keys} x {values}");
                }

                var newGui = await Context.Channel.SendMessageAsync("", embed: embedd.Build());
                var newresponsee = await NextMessageAsync();
                string target = null;
                if (newresponsee == null)
                {
                    await gui.DeleteAsync();
                    await newGui.DeleteAsync();
                    await SendMessage(Context, null, 
                        $"{Context.User.Mention},The interface has closed due to inactivity");
                    return;
                }

                if (newresponsee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) &&
                    (response.Author.Equals(Context.User)))
                {
                    await gui.DeleteAsync();
                    await newGui.DeleteAsync();
                    await SendMessage(Context, null, 
                        $":shield:  **|**  **{Context.User.Username}**, engagement cancelled.");
                    return;
                }

                await SendMessage(Context, null, "Who do you want to use this item on? (@mention them)");
                var newresponseee = await NextMessageAsync();
                if (!newresponseee.Content.Equals(Context.User.Username, StringComparison.CurrentCultureIgnoreCase) ||
                    !newresponseee.Content.Equals(config.OpponentName, StringComparison.CurrentCultureIgnoreCase) &&
                    (response.Author.Equals(Context.User)))
                {
                    await gui.ModifyAsync(m =>
                    {
                        m.Content =
                            $"{Global.ENo} **|** That is an invalid response. Please try again.";
                    });
                    return;
                }

                if (newresponseee.Content.Equals(Context.User.Username, StringComparison.CurrentCultureIgnoreCase) &&
                    (response.Author.Equals(Context.User)))
                {
                    target = Context.User.Username;
                }

                if (newresponseee.Content.Equals(config.OpponentName, StringComparison.CurrentCultureIgnoreCase) &&
                    (response.Author.Equals(Context.User)))
                {
                    target = config.OpponentName;
                }

                if (newresponseee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) &&
                    (response.Author.Equals(Context.User)))
                {
                    await gui.ModifyAsync(m =>
                    {
                        m.Content = $":shield:  **|**  **{Context.User.Username}**, engagement cancelled.";
                    });
                    return;
                }

                if (newresponseee == null)
                {
                    await gui.ModifyAsync(m =>
                    {
                        m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity";
                    });
                    return;
                }

                var itemChoosen = config.Items.ElementAt(int.Parse(newresponsee.ToString())).Key;
                var result = Helpers.ExecutePotion(Context, itemChoosen, config.Id, configg.Id);

                if (result.Success)
                {
                    await SendMessage(Context, null, result.Response);
                    var turnmsg = await NextTurn(Context);
                    await SendMessage(Context, null, turnmsg);
                    return;
                }
                else
                {
                    await SendMessage(Context, null, result.Response);
                    return;
                }
            }

            if (response.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) &&
                (response.Author.Equals(Context.User)))
            {
                if (config.Fighting || configg.Fighting)
                {
                    await ReplyAsync(":flag_white: **|** " + Context.User.Mention + " gave up. The fight stopped.");
                    config.Wins += 1;
                    configg.Losses += 1;
                    await Reset(Context, activeChomusuke, activeChomusukee);

                    GlobalUserAccounts.SaveAccounts(config.Id, configg.Id);
                }
            }

            if (response == null)
            {
                await gui.ModifyAsync(m =>
                {
                    m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity";
                });
                return;
            }
        }

        public static async Task<string> NextTurn(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            config.PlaceHolder = config.WhosTurn;
            config.WhosTurn = config.WhoWaits;
            config.WhoWaits = config.PlaceHolder;
            configg.PlaceHolder = configg.WhosTurn;
            configg.WhosTurn = configg.WhoWaits;
            configg.WhoWaits = configg.PlaceHolder;

            await Helpers.ApplyEffects(context, context.User.Id, player2.Id);
            
            GlobalUserAccounts.SaveAccounts(config.Id, configg.Id);
            return $"{config.WhosTurn}, your turn!";
        }
        //TODO: add more attacks and xp gain

        public static async Task<Tuple<bool, string>> CheckDeath(ShardedCommandContext context, Core.Entities.Chomusuke chom1, Core.Entities.Chomusuke chom2)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            string response = string.Empty;
            bool success = false;

            if (chom1.Health <= 0)
            {
                response = $"{context.User.Username} died. {config.OpponentName} wins!";
                configg.Wins += 1;
                config.Losses += 1;
                configg.WinStreak += 1;
                config.WinStreak = 0;
                success = true;
                await Reset(context, chom1, chom2);
            }
            if (chom2.Health <= 0)
            {
                response = $"{player2.Username} died. {configg.OpponentName} wins!";
                config.Wins += 1;
                configg.Losses += 1;
                success = true;
                config.WinStreak += 1;
                configg.WinStreak = 0;
                await Reset(context, chom1, chom2);
            }
            if (chom2.Health <= 0 && chom1.Health <= 0)
            {
                response = "Both players died. It is a draw.";
                config.Draws += 1;
                configg.Draws += 1;
                success = true;
                await Reset(context, chom1, chom2);
            }
            GlobalUserAccounts.SaveAccounts(config.Id, configg.Id);

            return new Tuple<bool, string>(success, response);
        }

        public static async Task Reset(ShardedCommandContext context, Core.Entities.Chomusuke chom1, Core.Entities.Chomusuke chom2)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);

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
            await ActiveChomusuke.ConvertActiveVariable(context.User.Id, config.OpponentId, chom1, chom2);

            GlobalUserAccounts.SaveAccounts(config.Id, configg.Id);
        }
    }
}

