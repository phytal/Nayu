using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;
using Nayu.Modules.Fun.Dueling;

namespace Nayu.Modules
{
    public class Engage : NayuModule
    {
        [Command("engage"), Alias("attack", "item", "giveup")]
        [Summary("Opens the duels engagment GUI")]
        [Remarks("Ex: n!engage")]
        public async Task DuelsArmoury()
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalUserAccounts.GetUserAccount((SocketGuildUser)Context.User);
            if (config.Fighting != true) { await ReplyAndDeleteAsync($"**{Context.User.Username}** there is no fight going on!", false, null, TimeSpan.FromSeconds(5), null); return; }
            var player2 = user.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            if (config.WhosTurn != user.Mention) { await ReplyAndDeleteAsync($"**{Context.User.Username}** it is not your turn!", false, null, TimeSpan.FromSeconds(5), null); return; }
            var embed = new EmbedBuilder()
                .WithColor(37, 152, 255)
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
                await Context.Channel.SendMessageAsync($"{Context.User.Mention},The interface has closed due to inactivity");
                return;
            }

            if (response.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                var embeddd = new EmbedBuilder()
                .WithColor(37, 152, 255)
                .WithFooter("Type [cancel] anytime to cancel the engagement.");

                embeddd.AddField("[1]", config.Attack1);
                embeddd.AddField("[2]", config.Attack2);
                embeddd.AddField("[3]", config.Attack3);
                embeddd.AddField("[4]", config.Attack4);
                var newGui = await Context.Channel.SendMessageAsync("", embed: embeddd.Build());

                var newresponse = await NextMessageAsync();
                if (newresponse == null)
                {
                    await gui.DeleteAsync();
                    await newGui.DeleteAsync();
                    await Context.Channel.SendMessageAsync($"{Context.User.Mention},The interface has closed due to inactivity");
                    return;
                }
                if (newresponse.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    //string name = config.Attack1.Replace(" ", "");
                    var result = new Tuple<bool, string, int>(false, null, 0);
                    if (config.Attack1 == "Slash") result = await Slash(Context);
                    if (config.Attack1 == "Block") result = Block(Context);
                    if (config.Attack1 == "Deflect") result = Deflect(Context);
                    if (config.Attack1 == "Absorb") result = await Absorb(Context);
                    if (config.Attack1 == "Bash") result = await Bash(Context);
                    if (config.Attack1 == "Fireball") result = await Fireball(Context);
                    if (config.Attack1 == "Earth Shatter") result = await EarthShatter(Context);
                    if (config.Attack1 == "Meditate") result = Meditate(Context);
                    //MethodInfo theMethod = this.GetType().GetMethod(name);
                    //var result = (Tuple<bool, string, int>)theMethod.Invoke(this, new object[] { Context });
                    if (result.Item1 != true)
                    {
                        await Context.Channel.SendMessageAsync(result.Item2);
                        return;
                    }
                    Tuple<bool, string> death = await CheckDeath(Context);
                    if (death.Item1 == true)
                    {
                        await Context.Channel.SendMessageAsync(death.Item2);
                    }
                    var turnmsg = await NextTurn(Context);
                    await Context.Channel.SendMessageAsync(result.Item2);
                    await Context.Channel.SendMessageAsync(turnmsg);
                    return;
                }
                if (newresponse.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    var result = new Tuple<bool, string, int>(false, null, 0);
                    if (config.Attack2 == "Slash") result = await Slash(Context);
                    if (config.Attack2 == "Block") result = Block(Context);
                    if (config.Attack2 == "Deflect") result = Deflect(Context);
                    if (config.Attack2 == "Absorb") result = await Absorb(Context);
                    if (config.Attack2 == "Bash") result = await Bash(Context);
                    if (config.Attack2 == "Fireball") result = await Fireball(Context);
                    if (config.Attack2 == "Earth Shatter") result = await EarthShatter(Context);
                    if (config.Attack2 == "Meditate") result = Meditate(Context);

                    Tuple<bool, string> death = await CheckDeath(Context);
                    if (death.Item1 == true)
                    {
                        await Context.Channel.SendMessageAsync(death.Item2);
                    }

                    //string name = config.Attack2.Replace(" ", "");
                    //MethodInfo theMethod = this.GetType().GetMethod(name);
                    //var result = (Tuple<bool, string, int>)theMethod.Invoke(this, new object[] { Context });
                    if (result.Item1 != true)
                    {
                        await Context.Channel.SendMessageAsync(result.Item2);
                        return;
                    }
                    var turnmsg = await NextTurn(Context);
                    await Context.Channel.SendMessageAsync(result.Item2);
                    await Context.Channel.SendMessageAsync(turnmsg);
                    return;
                }
                if (newresponse.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    var result = new Tuple<bool, string, int>(false, null, 0);
                    if (config.Attack3 == "Slash") result = await Slash(Context);
                    if (config.Attack3 == "Block") result = Block(Context);
                    if (config.Attack3 == "Deflect") result = Deflect(Context);
                    if (config.Attack3 == "Absorb") result = await Absorb(Context);
                    if (config.Attack3 == "Bash") result = await Bash(Context);
                    if (config.Attack3 == "Fireball") result = await Fireball(Context);
                    if (config.Attack3 == "Earth Shatter") result = await EarthShatter(Context);
                    if (config.Attack3 == "Meditate") result = Meditate(Context);

                    Tuple<bool, string> death = await CheckDeath(Context);
                    if (death.Item1 == true)
                    {
                        await Context.Channel.SendMessageAsync(death.Item2);
                    }

                    // name = config.Attack3.Replace(" ", "");
                   // MethodInfo theMethod = this.GetType().GetMethod(name);
                   // var result = (Tuple<bool, string, int>)theMethod.Invoke(this, new object[] { Context });
                    if (result.Item1 != true)
                    {
                        await Context.Channel.SendMessageAsync(result.Item2);
                        return;
                    }
                    var turnmsg = await NextTurn(Context);
                    await Context.Channel.SendMessageAsync(result.Item2);
                    await Context.Channel.SendMessageAsync(turnmsg);
                    return;
                }
                if (newresponse.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    var result = new Tuple<bool, string, int>(false, null, 0);
                    if (config.Attack4 == "Slash") result = await Slash(Context);
                    if (config.Attack4 == "Block") result = Block(Context);
                    if (config.Attack4 == "Deflect") result = Deflect(Context);
                    if (config.Attack4 == "Absorb") result = await Absorb(Context);
                    if (config.Attack4 == "Bash") result = await Bash(Context);
                    if (config.Attack4 == "Fireball") result = await Fireball(Context);
                    if (config.Attack4 == "Earth Shatter") result = await EarthShatter(Context);
                    if (config.Attack4 == "Meditate") result = Meditate(Context);

                    Tuple<bool, string> death = await CheckDeath(Context);
                    if (death.Item1 == true)
                    {
                        await Context.Channel.SendMessageAsync(death.Item2);
                    }

                    //string name = config.Attack4.Replace(" ", "");
                    //MethodInfo theMethod = this.GetType().GetMethod(name);
                    //var result = (Tuple<bool, string, int>)theMethod.Invoke(this, new object[] { Context });
                    if (result.Item1 != true)
                    {
                        await Context.Channel.SendMessageAsync(result.Item2);
                        return;
                    }
                    var turnmsg = await NextTurn(Context);
                    await Context.Channel.SendMessageAsync(result.Item2);
                    await Context.Channel.SendMessageAsync(turnmsg);
                    return;
                }
                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await gui.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, engagement cancelled."; });
                    return;
                }

                else
                {
                    await gui.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                    return;
                }
            }
            if (response.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                var info = new Tuple<string, string>(null, null);
                var values = 0;
                var embedd = new EmbedBuilder()
                .WithColor(37, 152, 255)
                .WithFooter("Type [cancel] anytime to cancel the engagement.");

                embedd.WithTitle("Which item would you like to use?");
                embedd.WithDescription("These are all of the items you currently have");
                List<string> nkeys = new List<string>();
                List<int> nvalues = new List<int>();
                int numkeys = 1;
                int numkeys2 = 0;
                int[] num = Enumerable.Range(0, config.Items.Keys.Count + 2).ToArray();
                embedd.AddField($"[1]", $"Med Kits x {config.Meds}");
                foreach (var keys in config.Items.Keys)
                {
                    nkeys.Add(keys);
                }
                foreach (var keys in config.Items.Keys)
                {
                    var entry = config.Items[keys];
                    if (entry == 0) continue;
                    values = GetValueFromKey(keys, user);

                    int newnum = GetNextElement(num, numkeys);
                    numkeys = numkeys + 1;
                    string newnumtostring = newnum.ToString();
                    embedd.AddField($"[{newnum}]", $"{keys} x {values}");

                    info = new Tuple<string, string>(newnumtostring, keys);
                }
                var newGui = await Context.Channel.SendMessageAsync("", embed: embedd.Build());
                var newresponsee = await NextMessageAsync();
                string target = null;
                if (newresponsee == null)
                {
                    await gui.DeleteAsync();
                    await newGui.DeleteAsync();
                    await Context.Channel.SendMessageAsync( $"{Context.User.Mention},The interface has closed due to inactivity");
                    return;
                }
                if (newresponsee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await gui.DeleteAsync();
                    await newGui.DeleteAsync();
                    await Context.Channel.SendMessageAsync($":shield:   |  **{Context.User.Username}**, engagement cancelled.");
                    return;
                }
                await Context.Channel.SendMessageAsync("Who do you want to use this item on? (@mention them)");
                var newresponseee = await NextMessageAsync();
                if (newresponseee.Content.Equals(Context.User.Username, StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    target = Context.User.Username;
                }
                if (newresponseee.Content.Equals(config.OpponentName, StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    target = config.OpponentName;
                }

                if (newresponsee.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    var result = MedKit(Context, target);
                    if (result.Item1 != false)
                    {
                        await Context.Channel.SendMessageAsync(result.Item2);
                        var turnmsg = await NextTurn(Context);
                        await Context.Channel.SendMessageAsync(turnmsg);
                        return;
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync(result.Item2);
                        return;
                    }
                }
                //carrys out action
                foreach (var keys in config.Items.Keys)
                {
                    var entry = config.Items[keys];
                    values = GetValueFromKey(keys, user);

                    int newnum = GetNextElement(num, numkeys2);
                    numkeys2 = numkeys2 + 1;
                    string newnumtostring = newnum.ToString();

                    info = new Tuple<string, string>(newnumtostring, keys);
                    if (newresponseee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await gui.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, engagement cancelled."; });
                        return;
                    }
                    if (newresponseee == null)
                    {
                        await gui.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                        return;
                    }
                    if (!newresponseee.Content.Equals(info.Item1, StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await gui.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                        return;
                    }
                    if (newresponsee.Content.Equals(info.Item1, StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        string name = info.Item2.Replace(" ", "");

                        MethodInfo theMethod = this.GetType().GetMethod(name);
                        var result = (Tuple<bool, string>)theMethod.Invoke(this, new object[] { Context, target });
                        if (result.Item1 != false)
                        {
                            await Context.Channel.SendMessageAsync(result.Item2);
                            var turnmsg = await NextTurn(Context);
                            await Context.Channel.SendMessageAsync(turnmsg);
                            return;
                        }
                        else
                        {
                            await Context.Channel.SendMessageAsync(result.Item2);
                            return;
                        }
                    }
                }

                await Context.Channel.SendMessageAsync("", embed: embedd.Build());

                var newresponse = await NextMessageAsync();

                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await gui.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, engagement cancelled."; });
                    return;
                }
                if (newresponse == null)
                {
                    await gui.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                    return;
                }
                else
                {
                    await gui.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                    return;
                }
            }
            if (response.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                if (config.Fighting == true || configg.Fighting == true)
                {
                    await ReplyAsync(":flag_white:  | " + Context.User.Mention + " gave up. The fight stopped.");
                    config.Fighting = false;
                    configg.Fighting = false;
                    config.Health = 100;
                    configg.Health = 100;
                    config.OpponentId = 0;
                    configg.OpponentId = 0;
                    config.OpponentName = null;
                    configg.OpponentName = null;
                    config.WhosTurn = null;
                    config.WhoWaits = null;
                    config.placeHolder = null;
                    configg.WhosTurn = null;
                    configg.WhoWaits = null;
                    configg.placeHolder = null;
                    config.Meds = 6;
                    configg.Meds = 6;
                    config.Blocking = false;
                    configg.Blocking = false;
                    configg.Deflecting = false;
                    config.Deflecting = false;
                    config.Wins += 1;
                    configg.Losses += 1;
                    GlobalUserAccounts.SaveAccounts();
                }
            }
            if (response == null)
            {
                await gui.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                return;
            }
        }

        public static int GetNextElement(int[] strArray, int index)
        {
            if ((index > strArray.Length - 1) || (index < 0))
                throw new Exception("Invalid index");

            else if (index == strArray.Length - 1)
                index = 0;

            else
                ++index;

            return strArray[index];
        }

        internal static int GetValueFromKey(string keyName, SocketUser user)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            return config.Items[keyName];
        }

        [Command("chomusuke list")]
        public async Task Paginator()
        {
            var pages = new[] { "Page 1", "Page 2", "Page 3", "aaaaaa", "Page 5" };
            await PagedReplyAsync(pages);
        }

        public static Tuple<bool, string> MedKit(ShardedCommandContext context, string target)
        {
            var user = context.User as SocketGuildUser;
            var config = GlobalUserAccounts.GetUserAccount((SocketGuildUser)context.User);
            var player2 = user.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            string response = string.Empty;
            int healed = Global.Rng.Next(14, 30);
            if (config.Health > 75)
            {
                response = $"<:redcross:459549117488824320>  | **{configg.OpponentName}**, You used can't use your Med Kit since the max amount you can heal to is 75 health, **{configg.OpponentName}**, Try Again!";
                return new Tuple<bool, string>(false, response);
            }
            else
            {
                int requiredhp = 75 - config.Health;
                if (requiredhp > healed) config.Health = config.Health + healed;
                else config.Health = 75;
                config.Meds = config.Meds - 1;
                response = $"<:redcross:459549117488824320>  | **{configg.OpponentName}**, You used your Med Kit and healed **{healed}** health!\n\n**{configg.OpponentName}** has **{config.Health}** health and has **{config.Meds}** Med Kits left!\n\n {config.WhosTurn}, Your turn!";
                GlobalUserAccounts.SaveAccounts();
                return new Tuple<bool, string>(true, response);
            }
        }

        public static Tuple<bool, string> StrengthPotion(ShardedCommandContext context, string target)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            string response = string.Empty;
            if (config.HasStrengthPots <= 25)
            {
                config.Items["Strength Potion"] -= 1;
                if (target == context.User.Mention) { config.HasStrengthPots += 5; response = $"**{context.User.Mention}** used a **Strength Potion** on themselves!"; }
                if (target == player2.Mention) { configg.HasStrengthPots += 5; response = $"**{context.User.Mention}** used a **Strength Potion** on {target}!"; }
                GlobalUserAccounts.SaveAccounts();

                return new Tuple<bool, string>(true, response);
            }
            if (configg.HasStrengthPots <= 25)
            {
                config.Items["Strength Potion"] -= 1;
                if (target == context.User.Mention) { config.HasStrengthPots += 5; response = $"**{context.User.Mention}** used a **Strength Potion** on themselves!"; }
                if (target == player2.Mention) { configg.HasStrengthPots += 5; response = $"**{context.User.Mention}** used a **Strength Potion** on {target}!"; }
                GlobalUserAccounts.SaveAccounts();

                return new Tuple<bool, string>(true, response);
            }
            else
            {
                response = $"**{context.User.Mention}**, you cant use a **Strength Potion** because both players are already at the max strength level (25%)!";
                return new Tuple<bool, string>(false, response);
            }
        }

        public static Tuple<bool, string> DebuffPotion(ShardedCommandContext context, string target)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            string response = string.Empty;
            if (configg.HasDebuffPots <= 25)
            {
                config.Items["Debuff Potion"] -= 1;
                if (target == context.User.Mention)
                {
                    config.HasDebuffPots += 5;
                    response = $"**{context.User.Mention}** used a **Debuff Potion** on themselves!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }
                if (target == player2.Mention)
                {
                    configg.HasDebuffPots += 5;
                    response = $"**{context.User.Mention}** used a **Debuff Potion** on {target}!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }
            }
            if (configg.HasDebuffPots <= 25)
            {
                if (target == context.User.Mention)
                {
                    config.HasDebuffPots += 5;
                    response = $"**{context.User.Mention}** used a **Debuff Potion** on themselves!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }
                if (target == player2.Mention)
                {
                    configg.HasDebuffPots += 5;
                    response = $"**{context.User.Mention}** used a **Debuff Potion** on {target}!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }
            }

            response = $"**{context.User.Mention}**, you cant use a **Debuff Potion** because both players are already at the max debuff level (25%)!";
            return new Tuple<bool, string>(false, response);
        }
        public static Tuple<bool, string> SpeedPotion(ShardedCommandContext context, string target)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            string response = string.Empty;
            if (config.HasSpeedPots != true)
            {
                config.Items["Speed Potion"] -= 1;
                if (target == context.User.Mention)
                {
                    config.HasSpeedPots = true;
                    response = $"**{context.User.Mention}** used a **Speed Potion** on themselves!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }
                if (target == player2.Mention)
                {
                    configg.HasSpeedPots = true;
                    response = $"**{context.User.Mention}** used a **Speed Potion** on {target}!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }
            }
            if (configg.HasSpeedPots != true)
            {
                config.Items["Speed Potion"] -= 1;
                if (target == context.User.Mention)
                {
                    config.HasSpeedPots = true;
                    response = $"**{context.User.Mention}** used a **Speed Potion** on themselves!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }
                if (target == player2.Mention)
                {
                    configg.HasSpeedPots = true;
                    response = $"**{context.User.Mention}** used a **Speed Potion** on {target}!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }
            }
            response = $"**{context.User.Mention}**, you cant use a **Speed Potion** because both players already have the effect!";
            return new Tuple<bool, string>(false, response);
        }
        public static Tuple<bool, string> EqualizerPotion(ShardedCommandContext context, string target)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            string response = string.Empty;
            config.Items["Equalizer Potion"] -= 1;
            if (target == context.User.Mention)
            {
                config.HasDebuffPots = 0; config.HasStrengthPots = 0; config.HasSpeedPots = false;
                response = $"**{context.User.Mention}** used a **Equalizer Potion** on themselves!";
                GlobalUserAccounts.SaveAccounts();
                return new Tuple<bool, string>(true, response);
            }
            if (target == player2.Mention)
            {
                config.HasDebuffPots = 0; config.HasStrengthPots = 0; config.HasSpeedPots = false;
                response = $"**{context.User.Mention}** used a **Equalizer Potion** on {target}!";
                GlobalUserAccounts.SaveAccounts();
                return new Tuple<bool, string>(true, response);
            }
            //never going to happen but whatever
            GlobalUserAccounts.SaveAccounts();
            return new Tuple<bool, string>(true, response);
        }
        public static Tuple<bool, string> MetallicAcid(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            string response = string.Empty;
            if (configg.armour != null)
            {
                config.Items["Metallic Acid"] -= 1;
                if (configg.HasBasicTreatment)
                {
                    configg.HasBasicTreatment = false;
                    GlobalUserAccounts.SaveAccounts();
                    response = $"**{context.User.Username}** used **Metallic Acid**, but **{player2.Username}** had Basic Treatment. Therefore, the Metallic Acid had no effect!";
                    return new Tuple<bool, string>(true, response);
                }
                if (configg.HasDivineShield)
                {
                    GlobalUserAccounts.SaveAccounts();
                    response = $"**{context.User.Username}** used **Metallic Acid**, but **{player2.Username}** had a Divine Shield. Therefore, the Metallic Acid had no effect!";
                    return new Tuple<bool, string>(true, response);
                }
                else
                {
                    response = $"**{context.User.Username}** used **Metallic Acid** and dissolved **{player2.Username}**'s **{configg.armour}** armour!";
                    configg.armour = null;
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }
            }
            else
            {
                response = $"**{context.User.Username}**, you cant use **Metallic Acid** because **{player2.Username}** has no armour!";
                return new Tuple<bool, string>(false, response);
            }
        }
        public static Tuple<bool, string> WeaponLiquifier(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            string response = string.Empty;
            if (configg.weapon != null)
            {
                config.Items["Weapon Liquifier"] -= 1;
                if (configg.HasBasicTreatment)
                {
                    configg.HasBasicTreatment = false;
                    GlobalUserAccounts.SaveAccounts();
                    response = $"**{context.User.Username}** used **Weapon Liquifier**, but **{player2.Username}** had Basic Treatment. Therefore, the **Weapon Liquifier** had no effect!";

                }
                if (configg.HasDivineShield)
                {
                    GlobalUserAccounts.SaveAccounts();
                    response = $"**{context.User.Username}** used **Weapon Liquifier**, but **{player2.Username}** had a Divine Shield. Therefore, the **Weapon Liquifier** had no effect!";
                }
                else
                {
                    response = $"**{context.User.Username}** used **Weapon Liquifier** and liquified **{player2.Username}**'s **{configg.weapon}** weapon!";
                    configg.weapon = null;
                    configg.HasPoisonedWeapon = false;
                    GlobalUserAccounts.SaveAccounts();
                }
                return new Tuple<bool, string>(true, response);
            }
            else
            {
                response = $"**{context.User.Username}**, you cant use **Weapon Liquifier** because **{player2.Username}** has no weapon!";
                return new Tuple<bool, string>(false, response);
            }
        }
        public static Tuple<bool, string> BasicTreatment(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            string response = string.Empty;
            if (config.HasBasicTreatment != true)
            {
                config.Items["Basic Treatment"] -= 1;
                config.HasBasicTreatment = true;
                GlobalUserAccounts.SaveAccounts();
                response = $"**{context.User.Username}** used a **Basic Treatment**!";
                return new Tuple<bool, string>(true, response);
            }
            else
            {
                response = $"**{context.User.Username}**, you cant use **Basic Treatment** because you already have the effect!";
                return new Tuple<bool, string>(false, response);
            }
        }
        public static Tuple<bool, string> DivineShield(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            string response = string.Empty;
            if (config.HasDivineShield != true)
            {
                config.Items["Divine Shield"] -= 1;
                config.HasDivineShield = true;
                GlobalUserAccounts.SaveAccounts();
                response = $"**{context.User.Username}** used a **Divine Shield**!";
                return new Tuple<bool, string>(true, response);
            }
            else
            {
                response = $"**{context.User.Username}**, you cant use **Divine Shield** because you already have the effect!";
                return new Tuple<bool, string>(false, response);
            }
        }
        public static async Task<string> NextTurn(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            config.placeHolder = config.WhosTurn;
            config.WhosTurn = config.WhoWaits;
            config.WhoWaits = config.placeHolder;
            configg.placeHolder = configg.WhosTurn;
            configg.WhosTurn = configg.WhoWaits;
            configg.WhoWaits = configg.placeHolder;

            await Burned(context);
            GlobalUserAccounts.SaveAccounts();
            return $"{config.WhosTurn}, your turn!";
        }
        //add attacks
        public static Tuple<bool, string, int> Block(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            string response = string.Empty;
            bool success = false;
            int dmg = 0;
            if (config.Blocking == true)
            {
                response = "You are already in blocking formation! Try Again!";
            }
            if (config.Deflecting == true)
            {
                response = "You cannot block while already in deflecting formation! Try Again!";
            }
            config.Blocking = true;
            GlobalUserAccounts.SaveAccounts();
            response = $":shield:  | **{config.OpponentName}**, You are now in blocking formation!\n\n**{config.OpponentName}**'s shield will absorb 75% of the damage from the next attack";
            success = true;
            return new Tuple<bool, string, int>(success, response, dmg);
        }

        public static async Task<Tuple<bool, string, int>> Slash(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);

            string response = string.Empty;
            bool success = false;
            int dmg = 0;
            int hitormiss = Global.Rng.Next(1, 9);

            if (hitormiss > 1)
            {
                dmg = Global.Rng.Next(7, 15);
                var result = CheckModifiers.GetDMGModifiers(context.User, player2, dmg);
                dmg = result.Item1;
                int modifier = result.Item2;

                var block = CheckModifiers.CheckForBlock(context.User, player2, dmg);
                if (block.Item1 == true)
                {
                    configg.Health = config.Health - block.Item3;
                    GlobalUserAccounts.SaveAccounts();
                    response = $":dagger:  | **{context.User.Username}**, You hit and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{block.Item2}** damage was blocked!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n **{config.OpponentName}** has **{configg.Health}** health left!";
                    dmg = block.Item3;
                }
                var deflect = CheckModifiers.CheckForDeflect(context.User, player2, dmg);
                if (deflect.Item1 == true)
                {
                    configg.Health = config.Health - deflect.Item2;
                    GlobalUserAccounts.SaveAccounts();
                    response = $":dagger:  | **{context.User.Username}**, You hit and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{deflect.Item2}** damage was deflected back!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n **{config.OpponentName}** has **{configg.Health}** health left!";
                    dmg = deflect.Item2;
                }
                else
                {
                    configg.Health = configg.Health - dmg;
                    GlobalUserAccounts.SaveAccounts();
                    response = $":dagger:  | **{context.User.Username}**, You hit and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers)!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n**{config.OpponentName}** has **{configg.Health}** health left!";
                }
            }
            else
            {
                response = $":dash:  | **{context.User.Username}**, your attack missed!";
            }
            success = true;

            return await Task.FromResult(new Tuple<bool, string, int>(success, response, dmg));
        }

        public static async Task<Tuple<bool, string, int>> Absorb(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);

            string response = string.Empty;
            bool success = false;
            int hit = Global.Rng.Next(1, 3);

            if (hit == 1)
            {
                if (configg.armour == "reinforced")
                {
                    GlobalUserAccounts.SaveAccounts();
                    response = $":fire:  | **{configg.OpponentName}** used **Absorb** but {config.OpponentName}** has reinforced armour. Therefore it had no effect.";
                    success = true;
                    return new Tuple<bool, string, int>(success, response, 0);
                }

                int dmg = Global.Rng.Next(7, 15);

                var result = CheckModifiers.GetDMGModifiers(context.User, player2, dmg);
                dmg = result.Item1;
                int modifier = result.Item2;

                var block = CheckModifiers.CheckForBlock(context.User, player2, dmg);
                if (block.Item1 == true)
                {
                    configg.Health = config.Health - block.Item3;
                    config.Health = config.Health + block.Item3;
                    GlobalUserAccounts.SaveAccounts();
                    response = $":comet:  | **{context.User.Username}**,You absorbed **{dmg}** health and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{block.Item2}** damage was blocked!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n **{config.OpponentName}** has **{configg.Health}** health left!";
                    dmg = block.Item3;
                }
                var deflect = CheckModifiers.CheckForDeflect(context.User, player2, dmg);
                if (deflect.Item1 == true)
                {
                    configg.Health = config.Health - deflect.Item2;
                    GlobalUserAccounts.SaveAccounts();
                    response = $":comet:  | **{context.User.Username}**, You absorbed **{dmg}** health and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{deflect.Item2}** damage was deflected back!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n **{config.OpponentName}** has **{configg.Health}** health left!";
                    dmg = deflect.Item2;
                }

                config.Health = config.Health + dmg;
                configg.Health = configg.Health - dmg;
                GlobalUserAccounts.SaveAccounts();

                response = $":comet:  | **{context.User.Username}**, You absorbed **{dmg}** health and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers)!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n**{config.OpponentName}** has **{configg.Health}** health left!";
                success = true;
                return new Tuple<bool, string, int>(success, response, dmg);
            }
            else
            {
                int dmg = 0;
                var result = CheckModifiers.GetDMGModifiers(context.User, player2, dmg); //resets 1 turn buffs
                response = $":dash:  | **{context.User.Username}**, your attack missed!";
                success = true;
                return new Tuple<bool, string, int>(success, response, dmg);
            }

        }

        public static Tuple<bool, string, int> Deflect(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            string response = string.Empty;
            bool success = false;
            int dmg = 0;
            if (config.IsMeditate == true)
            {
                response = "You cannot deflect while meditation! Try Again!";
            }
            if (config.Blocking == true)
            {
                response = "You cannot deflect while in blocking formation! Try Again!";
            }
            if (config.Deflecting == true)
            {
                response = "You are already in deflecting formation! Try Again!";
            }
            config.Deflecting = true;
            GlobalUserAccounts.SaveAccounts();
            response = $":shield:  | **{config.OpponentName}**, You are now in deflecting formation!\n\n**{config.OpponentName}**'s shield will deflect 50% of the damage from the next attack";
            success = true;
            return new Tuple<bool, string, int>(success, response, dmg);
        }

        public static async Task<Tuple<bool, string, int>> Fireball(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            int hitOrMiss = Global.Rng.Next(1, 5);
            string response = string.Empty;
            bool success = false;
            int dmg = Global.Rng.Next(3,7);
            if (configg.armour == "reinforced")
            {
                GlobalUserAccounts.SaveAccounts();
                response = $":fire:  | **{configg.OpponentName}** used **Fireball** but {config.OpponentName}** has reinforced armour. Therefore it had no effect.";
                success = true;
                return new Tuple<bool, string, int>(success, response, 0);
            }
            if (hitOrMiss > 1)
            {
                var result = CheckModifiers.GetDMGModifiers(context.User, player2, dmg);
                dmg = result.Item1;
                var modifier = result.Item2;
                config.Health = config.Health - dmg;
                config.Burn = new Tuple<bool, int>(false, 0);

                GlobalUserAccounts.SaveAccounts();
                response = $":fire:  | **{configg.OpponentName}** used **Fireball** and did {dmg} damage (buffed by {modifier}% due to active damage modifiers)!\n\n**{config.OpponentName}** will now be *burning* for 4 turns";
                success = true;
                return new Tuple<bool, string, int>(success, response, dmg);
            }
            else
            {
                dmg = 0;
                var result = CheckModifiers.GetDMGModifiers(context.User, player2, dmg);
                response = $":dash:  | **{context.User.Username}**, your attack missed!";
                success = true;
                return new Tuple<bool, string, int>(success, response, dmg);
            }
        }
        public static async Task Burned(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);

            int dmg = Global.Rng.Next(2, 5);

            if (config.Burn.Item1 != true) return;
            if (config.Burn.Item2 == 4)
            {
                config.Burn = new Tuple<bool, int>(false, 0);
                GlobalUserAccounts.SaveAccounts();
                await context.Channel.SendMessageAsync($"{context.User} stopped burning!");
                return;
            }
            int newValue = config.Burn.Item2 + 1;
            config.Burn = new Tuple<bool,int>(true, newValue);
            config.Health = config.Health - dmg;
            GlobalUserAccounts.SaveAccounts();
            await context.Channel.SendMessageAsync($"{context.User} took {dmg} from being burned!");
            return;
        }
        public static async Task<Tuple<bool, string, int>> EarthShatter(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            string response = string.Empty;
            bool success = false;
            int dmg = Global.Rng.Next(15, 25);
            if (config.Blocking == true)
                config.Blocking = false;
            if (configg.Blocking == true)
                configg.Blocking = false;
            if (config.Deflecting == true)
                config.Deflecting = false;
            if (configg.Deflecting == true)
                configg.Deflecting = false;
            config.Health = config.Health - dmg;
            configg.Health = configg.Health - dmg;
            GlobalUserAccounts.SaveAccounts();
            response = $"<:shatter:532002647692148748>  | **{config.OpponentName}**, shattered the Earth! {dmg} damage was dealt to both players and Blocks/Deflects are canceled";
            success = true;

            return new Tuple<bool, string, int>(success, response, dmg);
        }
        public static async Task<Tuple<bool, string, int>> Bash(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            string response = string.Empty;
            bool success = false;
            int dmg = Global.Rng.Next(5, 10);
            int bash = Global.Rng.Next(1, 4);

            var result = CheckModifiers.GetDMGModifiers(context.User, player2, dmg);
            dmg = result.Item1;
            int modifier = result.Item2;

            var block = CheckModifiers.CheckForBlock(context.User, player2, dmg);
            if (block.Item1 == true)
            {
                configg.Health = config.Health - block.Item3;
                GlobalUserAccounts.SaveAccounts();
                response = $"**{context.User.Username}**, You bashed **{player2.Username}** and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{block.Item2}** damage was blocked!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n **{config.OpponentName}** has **{configg.Health}** health left!";
                dmg = block.Item3;
            }
            var deflect = CheckModifiers.CheckForDeflect(context.User, player2, dmg);
            if (deflect.Item1 == true)
            {
                configg.Health = config.Health - deflect.Item2;
                GlobalUserAccounts.SaveAccounts();
                response = $"**{context.User.Username}**, You bashed **{player2.Username}** and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{deflect.Item2}** damage was deflected back!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n **{config.OpponentName}** has **{configg.Health}** health left!";
                dmg = deflect.Item2;
            }

            configg.Health = configg.Health - dmg;
            GlobalUserAccounts.SaveAccounts();

            GlobalUserAccounts.SaveAccounts();

            response = $"**{context.User.Username}**, You bashed **{player2.Username}** and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers)!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n**{config.OpponentName}** has **{configg.Health}** health left!";
            success = true;

            if (bash == 1)
            {
                response = $"**{context.User.Username}**, You bashed **{player2.Username}** and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers)!**{config.OpponentName}** is now stunned (cannot attack for a turn)!\n\n**{configg.OpponentName}** has **{config.Health}** health left!\n**{config.OpponentName}** has **{configg.Health}** health left!";
                success = false;
            }

            return new Tuple<bool, string, int>(success, response, dmg);
        }
        public static Tuple<bool, string, int> Meditate(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            string response = string.Empty;
            bool success = false;
            int dmg = 0;
            if (config.IsMeditate == true)
            {
                response = "You have already meditated! Try Again!";
            }
            if (config.Blocking == true)
            {
                response = "You cannot meditate while in blocking formation! Try Again!";
            }
            if (config.Deflecting == true)
            {
                response = "You cannot meditate while in deflecting formation! Try Again!";
            }
            config.Blocking = true;
            GlobalUserAccounts.SaveAccounts();
            response = $":shield:  | **{config.OpponentName}**, You are now in blocking formation!\n\n**{config.OpponentName}**'s shield will absorb 75% of the damage from the next attack";
            success = true;
            return new Tuple<bool, string, int>(success, response, dmg);
        }
        public static async Task<Tuple<bool, string>> CheckDeath(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            string response = string.Empty;
            bool success = false;

            if (config.Health <= 0)
            {
                response = $"{context.User.Username} died. {config.OpponentName} wins!";
                configg.Wins += 1;
                config.Losses += 1;
                configg.WinStreak += 1;
                config.WinStreak = 0;
                success = true;
                await Reset(context);
            }
            if (configg.Health <= 0)
            {
                response = $"{player2.Username} died. {configg.OpponentName} wins!";
                config.Wins += 1;
                configg.Losses += 1;
                success = true;
                config.WinStreak += 1;
                configg.WinStreak = 0;
                await Reset(context);
            }
            if (configg.Health <= 0 && config.Health <= 0)
            {
                response = "Both players died. It is a draw.";
                config.Draws += 1;
                configg.Draws += 1;
                success = true;
                await Reset(context);
            }
            GlobalUserAccounts.SaveAccounts();

            return new Tuple<bool, string>(success, response);
        }

        public static Task Reset(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);

            config.Fighting = false;
            configg.Fighting = false;
            config.Health = 100;
            configg.Health = 100;
            config.OpponentId = 0;
            configg.OpponentId = 0;
            config.OpponentName = null;
            configg.OpponentName = null;
            config.WhosTurn = null;
            config.WhoWaits = null;
            config.placeHolder = null;
            configg.WhosTurn = null;
            configg.WhoWaits = null;
            configg.placeHolder = null;
            config.Meds = 6;
            configg.Meds = 6;
            config.Blocking = false;
            configg.Blocking = false;
            configg.Deflecting = false;
            config.Deflecting = false;
            config.HasSpeedPots = false;
            config.HasStrengthPots = 0;
            config.HasDebuffPots = 0;
            config.HasPoisonedWeapon = false;
            config.HasDivineShield = false;
            config.HasBasicTreatment = false;
            config.HasBookDR = false;
            config.HasBookPE = false;
            config.HasBookSD = false;
            config.HasBookWM = false;
            configg.HasSpeedPots = false;
            configg.HasStrengthPots = 0;
            configg.HasDebuffPots = 0;
            configg.HasPoisonedWeapon = false;
            configg.HasDivineShield = false;
            configg.HasBasicTreatment = false;
            configg.HasBookDR = false;
            configg.HasBookPE = false;
            configg.HasBookSD = false;
            configg.HasBookWM = false;

            GlobalUserAccounts.SaveAccounts();
            return Task.CompletedTask;
        }
    }
}

