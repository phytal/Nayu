using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;

namespace Nayu.Modules.Chomusuke.Dueling
{
    public class Engage : NayuModule
    {/*
        [Command("engage"), Alias("attack", "item", "giveup")]
        [Summary("Opens the duels engagment GUI")]
        [Remarks("Ex: n!engage")]
        public async Task EngageCommand()
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalUserAccounts.GetUserAccount((SocketGuildUser)Context.User);
            
            var choms = ActiveChomusuke.GetActiveChomusuke(Context.User.Id, config.OpponentId);
            var activeChomusuke = choms.Item1;
            var activeChomusukee = choms.Item2;
            
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

            if (response.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) &&
                (response.Author.Equals(Context.User)))
            {
                var embeddd = new EmbedBuilder()
                    .WithColor(37, 152, 255)
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
                    await Context.Channel.SendMessageAsync(
                        $"{Context.User.Mention},The interface has closed due to inactivity");
                }

                else if (newresponse.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) &&
                    (response.Author.Equals(Context.User)))
                {
                    var result = new Tuple<bool, string, int>(false, null, 0);
                    switch (activeChomusuke.Attack1)
                    {
                        case "Slash":
                            await Slash(Context);
                            break;
                        case "Block":
                            Block(Context);
                            break;
                        case "Deflect":
                            Deflect(Context);
                            break;
                        case "Absorb":
                            await Absorb(Context);
                            break;
                        case "Bash":
                            await Absorb(Context);
                            break;
                        case "Fireball":
                            await Fireball(Context);
                            break;
                        case "EarthShatter":
                            await EarthShatter(Context);
                            break;
                        case "Meditate":
                            Meditate(Context, activeChomusuke);
                            break;
                    }

                    if (result.Item1 != true)
                    {
                        await Context.Channel.SendMessageAsync(result.Item2);
                        return;
                    }

                    Tuple<bool, string> death = await CheckDeath(Context, activeChomusuke, activeChomusukee);
                    if (death.Item1)
                    {
                        await Context.Channel.SendMessageAsync(death.Item2);
                    }

                    var turnmsg = await NextTurn(Context);
                    await Context.Channel.SendMessageAsync(result.Item2);
                    await Context.Channel.SendMessageAsync(turnmsg);
                }

                else if (newresponse.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) &&
                    (response.Author.Equals(Context.User)))
                {
                    var result = new Tuple<bool, string, int>(false, null, 0);
                    switch (activeChomusuke.Attack2)
                    {
                        case "Slash":
                            await Slash(Context);
                            break;
                        case "Block":
                            Block(Context);
                            break;
                        case "Deflect":
                            Deflect(Context);
                            break;
                        case "Absorb":
                            await Absorb(Context);
                            break;
                        case "Bash":
                            await Absorb(Context);
                            break;
                        case "Fireball":
                            await Fireball(Context);
                            break;
                        case "EarthShatter":
                            await EarthShatter(Context);
                            break;
                        case "Meditate":
                            Meditate(Context, activeChomusuke);
                            break;
                    }

                    Tuple<bool, string> death = await CheckDeath(Context, activeChomusuke, activeChomusukee);
                    if (death.Item1)
                    {
                        await Context.Channel.SendMessageAsync(death.Item2);
                    }

                    if (result.Item1 != true)
                    {
                        await Context.Channel.SendMessageAsync(result.Item2);
                        return;
                    }

                    var turnmsg = await NextTurn(Context);
                    await Context.Channel.SendMessageAsync(result.Item2);
                    await Context.Channel.SendMessageAsync(turnmsg);
                }

                else if (newresponse.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) &&
                    (response.Author.Equals(Context.User)))
                {
                    var result = new Tuple<bool, string, int>(false, null, 0);
                    switch (activeChomusuke.Attack3)
                    {
                        case "Slash":
                            await Slash(Context);
                            break;
                        case "Block":
                            Block(Context);
                            break;
                        case "Deflect":
                            Deflect(Context);
                            break;
                        case "Absorb":
                            await Absorb(Context);
                            break;
                        case "Bash":
                            await Absorb(Context);
                            break;
                        case "Fireball":
                            await Fireball(Context);
                            break;
                        case "EarthShatter":
                            await EarthShatter(Context);
                            break;
                        case "Meditate":
                            Meditate(Context, activeChomusuke);
                            break;
                    }

                    Tuple<bool, string> death = await CheckDeath(Context, activeChomusuke, activeChomusukee);
                    if (death.Item1)
                    {
                        await Context.Channel.SendMessageAsync(death.Item2);
                    }

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

                else if (newresponse.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) &&
                    (response.Author.Equals(Context.User)))
                {
                    var result = new Tuple<bool, string, int>(false, null, 0);
                    switch (activeChomusuke.Attack4)
                    {
                        case "Slash":
                            await Slash(Context);
                            break;
                        case "Block":
                            Block(Context);
                            break;
                        case "Deflect":
                            Deflect(Context);
                            break;
                        case "Absorb":
                            await Absorb(Context);
                            break;
                        case "Bash":
                            await Absorb(Context);
                            break;
                        case "Fireball":
                            await Fireball(Context);
                            break;
                        case "EarthShatter":
                            await EarthShatter(Context);
                            break;
                        case "Meditate":
                            Meditate(Context,activeChomusuke);
                            break;
                    }

                    Tuple<bool, string> death = await CheckDeath(Context, activeChomusuke, activeChomusukee);
                    if (death.Item1)
                    {
                        await Context.Channel.SendMessageAsync(death.Item2);
                    }

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

                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) &&
                    (response.Author.Equals(Context.User)))
                {
                    await gui.ModifyAsync(m =>
                    {
                        m.Content = $":shield:   |  **{Context.User.Username}**, engagement cancelled.";
                    });
                    return;
                }

                await gui.ModifyAsync(m =>
                {
                    m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again.";
                });
                return;
            }

            if (response.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) &&
                (response.Author.Equals(Context.User)))
            {
                var info = new Tuple<string, string>(null, null);
                var values = 0;
                var embedd = new EmbedBuilder()
                    .WithColor(37, 152, 255)
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
                    values = GetValueFromKey(keys, user);

                    int newnum = GetNextElement(num, numkeys);
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
                    await Context.Channel.SendMessageAsync(
                        $"{Context.User.Mention},The interface has closed due to inactivity");
                    return;
                }

                if (newresponsee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) &&
                    (response.Author.Equals(Context.User)))
                {
                    await gui.DeleteAsync();
                    await newGui.DeleteAsync();
                    await Context.Channel.SendMessageAsync(
                        $":shield:   |  **{Context.User.Username}**, engagement cancelled.");
                    return;
                }

                await Context.Channel.SendMessageAsync("Who do you want to use this item on? (@mention them)");
                var newresponseee = await NextMessageAsync();
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

                //carrys out item action
                foreach (var keys in config.Items.Keys)
                {
                    int newnum = GetNextElement(num, numkeys2);
                    numkeys2 = numkeys2 + 1;
                    string newnumtostring = newnum.ToString();

                    info = new Tuple<string, string>(newnumtostring, keys);
                    if (newresponseee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) &&
                        (response.Author.Equals(Context.User)))
                    {
                        await gui.ModifyAsync(m =>
                        {
                            m.Content = $":shield:   |  **{Context.User.Username}**, engagement cancelled.";
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

                    if (!newresponseee.Content.Equals(info.Item1, StringComparison.CurrentCultureIgnoreCase) &&
                        (response.Author.Equals(Context.User)))
                    {
                        await gui.ModifyAsync(m =>
                        {
                            m.Content =
                                "<:no:453716729525174273>  | That is an invalid response. Please try again.";
                        });
                        return;
                    }

                    if (newresponsee.Content.Equals(info.Item1, StringComparison.CurrentCultureIgnoreCase) &&
                        (response.Author.Equals(Context.User)))
                    {
                        string name = info.Item2.Replace(" ", "");

                        MethodInfo theMethod = GetType().GetMethod(name);
                        var result =
                            (Tuple<bool, Core.Entities.Chomusuke, Core.Entities.Chomusuke, string>) theMethod.Invoke(this,
                                new object[] {Context, activeChomusuke, activeChomusukee, target});
                        if (result.Item1)
                        {
                            await Context.Channel.SendMessageAsync(result.Item4);
                            var turnmsg = await NextTurn(Context);
                            await Context.Channel.SendMessageAsync(turnmsg);
                            return;
                        }
                        else
                        {
                            await Context.Channel.SendMessageAsync(result.Item4);
                            return;
                        }
                    }
                }

                await Context.Channel.SendMessageAsync("", embed: embedd.Build());

                var newresponse = await NextMessageAsync();

                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) &&
                    (response.Author.Equals(Context.User)))
                {
                    await gui.ModifyAsync(m =>
                    {
                        m.Content = $":shield:   |  **{Context.User.Username}**, engagement cancelled.";
                    });
                    return;
                }

                if (newresponse == null)
                {
                    await gui.ModifyAsync(m =>
                    {
                        m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity";
                    });
                    return;
                }

                await gui.ModifyAsync(m =>
                {
                    m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again.";
                });
                return;

            }

            if (response.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                if (config.Fighting == true || configg.Fighting == true)
                {
                    await ReplyAsync(":flag_white:  | " + Context.User.Mention + " gave up. The fight stopped.");
                    config.Wins += 1;
                    configg.Losses += 1;
                    await Reset(Context, activeChomusuke, activeChomusukee);
                    
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

            if (index == strArray.Length - 1)
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

        public static Tuple<bool, string> StrengthPotion(ShardedCommandContext context, Core.Entities.Chomusuke chom1,
            Core.Entities.Chomusuke chom2, string target)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            string response = string.Empty;
            if (chom1.PotionEffects["Strength"] <= 25 ||
                !chom1.PotionEffects.Keys.Contains("Strength") && chom2.PotionEffects["Strength"] <= 25 ||
                !chom2.PotionEffects.Keys.Contains("Strength"))
            {

                config.Items["Strength Potion"] -= 1;
                if (target == context.User.Mention)
                {
                    chom1.PotionEffects.Add("Strength", 0);
                    chom1.PotionEffects["Strength"] += 5;
                    response = $"**{context.User.Mention}** used a **Strength Potion** on {chom1.Name}!";
                }

                if (target == player2.Mention)
                {
                    chom1.PotionEffects.Add("Strength", 0);
                    chom2.PotionEffects["Strength"] += 5;
                    response = $"**{context.User.Mention}** used a **Strength Potion** on {chom2.Name}!";
                }

                GlobalUserAccounts.SaveAccounts();

                return new Tuple<bool, string>(true, response);
            }

            response =
                $"**{context.User.Mention}**, you cant use a **Strength Potion** because both players are already at the max strength level (25%)!";
            return new Tuple<bool, string>(false, response);
        }

        public static Tuple<bool, string> DebuffPotion(ShardedCommandContext context, Core.Entities.Chomusuke chom1,
            Core.Entities.Chomusuke chom2, string target)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            string response = string.Empty;
            if (chom1.PotionEffects["Debuff"] <= 25 ||
                !chom1.PotionEffects.Keys.Contains("Debuff") && chom2.PotionEffects["Debuff"] <= 25 ||
                !chom2.PotionEffects.Keys.Contains("Debuff"))
            {
                config.Items["Debuff Potion"] -= 1;
                if (target == context.User.Mention)
                {
                    chom1.PotionEffects.Add("Debuff", 0);
                    chom1.PotionEffects["Debuff"] += 5;
                    response = $"**{context.User.Mention}** used a **Debuff Potion** on {chom1.Name}!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }

                if (target == player2.Mention)
                {
                    chom1.PotionEffects.Add("Debuff", 0);
                    chom2.PotionEffects["Debuff"] += 5;
                    response = $"**{context.User.Mention}** used a **Debuff Potion** on {chom2.Name}!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }
            }

            response =
                $"**{context.User.Mention}**, you cant use a **Debuff Potion** because both players are already at the max debuff level (25%)!";
            return new Tuple<bool, string>(false, response);
        }

        public static Tuple<bool, string> SpeedPotion(ShardedCommandContext context, Core.Entities.Chomusuke chom1,
            Core.Entities.Chomusuke chom2, string target)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            string response = string.Empty;
            if (!chom1.PotionEffects.Keys.Contains("Speed") && !chom2.PotionEffects.Keys.Contains("Speed"))
            {
                config.Items["Speed Potion"] -= 1;
                if (target == context.User.Mention)
                {
                    chom1.PotionEffects.Add("Speed", 0); //going to use its existence to determine true/false
                    response = $"**{context.User.Mention}** used a **Speed Potion** on {chom1.Name}!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }

                if (target == player2.Mention)
                {
                    chom2.PotionEffects.Add("Speed", 0);
                    response = $"**{context.User.Mention}** used a **Speed Potion** on {chom2.Name}!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }
            }

            response =
                $"**{context.User.Mention}**, you cant use a **Speed Potion** because both players already have the effect!";
            return new Tuple<bool, string>(false, response);
        }

        public static Tuple<bool, string> EqualizerPotion(ShardedCommandContext context, Core.Entities.Chomusuke chom1,
            Core.Entities.Chomusuke chom2, string target)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            string response = string.Empty;
            config.Items["Equalizer Potion"] -= 1;
            if (chom1.PotionEffects == null && chom2.PotionEffects == null)
            {
                if (target == context.User.Mention)
                {
                    chom1.PotionEffects.Clear();
                    response = $"**{context.User.Mention}** used a **Equalizer Potion** on {chom1.Name}!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }

                if (target == player2.Mention)
                {
                    chom2.PotionEffects.Clear();
                    response = $"**{context.User.Mention}** used a **Equalizer Potion** on {chom2.Name}!";
                    GlobalUserAccounts.SaveAccounts();
                    return new Tuple<bool, string>(true, response);
                }
            }

            //never going to happen but whatever
            GlobalUserAccounts.SaveAccounts();
            return new Tuple<bool, string>(true, response);
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

            await Burned(context);
            GlobalUserAccounts.SaveAccounts();
            return $"{config.WhosTurn}, your turn!";
        }
        //TODO: add attacks?
        /*public static Tuple<bool, string, int> Block(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            string response = string.Empty;
            bool success = false;
            int dmg = 0;
            if (config.Blocking == true)
            {
                response = "You are already in blocking formation! Try again!";
            }
            if (config.Deflecting == true)
            {
                response = "You cannot block while already in deflecting formation! Try again!";
            }
            config.Blocking = true;
            GlobalUserAccounts.SaveAccounts();
            response = $":shield:  | **{config.OpponentName}**, You are now in blocking formation!\n\n**{config.OpponentName}**'s shield will absorb 75% of the damage from the next attack";
            success = true;
            return new Tuple<bool, string, int>(success, response, dmg);
        }

        public static async Task<AttackCommand> Slash(ShardedCommandContext context)
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

            return await Task.FromResult(new AttackCommand{success, response, dmg});
        }

        public static async Task<AttackCommand> Absorb(ShardedCommandContext context)
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

        public static AttackCommand Deflect(ShardedCommandContext context)
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

        public static async Task<AttackCommand> Fireball(ShardedCommandContext context)
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
        public static async Task<AttackCommand> EarthShatter(ShardedCommandContext context)
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
        public static AttackCommand Meditate(ShardedCommandContext context, Core.Entities.Chomusuke chom)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            string response = string.Empty;
            bool success = false;
            int dmg = 0;
            if (chom.Meditating)
            {
                response = "You have already meditated! Try Again!";
            }
            if (chom.Blocking)
            {
                response = "You cannot meditate while in blocking formation! Try Again!";
            }
            if (chom.Deflecting)
            {
                response = "You cannot meditate while in deflecting formation! Try Again!";
            }
            chom.Meditating = true;
            GlobalUserAccounts.SaveAccounts();
            response = $":shield:  | **{config.OpponentName}**, {chom.Name} just Meditated! \n\n**{config.OpponentName}**'s next attack will deal 30% more damage";
            success = true;
            return new Tuple<bool, string, int>(success, response, dmg);
        }
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
            GlobalUserAccounts.SaveAccounts();

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
            chom1.Blocking = false;
            chom2.Blocking = false;
            chom1.Deflecting = false;
            chom2.Deflecting = false;
            chom1.PotionEffects.Clear();
            chom2.PotionEffects.Clear();
            await ActiveChomusuke.ConvertActiveVariable(context.User.Id, config.OpponentId, chom1, chom2);

            GlobalUserAccounts.SaveAccounts();
        }*/
    }
}

