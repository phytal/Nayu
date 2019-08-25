using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;

namespace Nayu.Modules
{
    public class DuelsShop : NayuModule
    {
        [Command("duelsBuy"), Alias("duels shop", "duel buy", "duel shop")]
        [Summary("Opens the duels shop menu!")]
        [Remarks("Ex: n!duels shop")]
        public async Task DuelsArmoury()
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalUserAccounts.GetUserAccount(user);
            if (config.Fighting == true)
            {
                await Context.Channel.SendMessageAsync("You can't go to the duels shop in the middle of a duel!");
                return;
            }
            string shoptext = ":crossed_swords:   **|  Duels Armoury** \n ```xl\nPlease select the purchase you would like to make.\n\n[1] Potions\n[2] Books\n[3] Weapons\n[4] Armour\n[5] Items\n[6] Blessings\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.```";
            var shop = await Context.Channel.SendMessageAsync(shoptext);
            var response = await NextMessageAsync();

            if (response.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $"```xl\n[1] Strength Potion - 5% more damage dealt [50 Taiyakis]\n[2] Speed Potion - 50% chance of canceling blocks and deflects [25 Taiyakis]\n[3] Debuff Potion - 5% more damage received [50 Taiyakis]\n[4] Equilizer Potion - Cancels all potion effects [75 Taiyakis]\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.\n```"; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 50)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{50 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    if (config.Items.ContainsKey("Strength Potion")) config.Items["Strength Potion"] += 1;
                    else config.Items.Add("Strength Potion", 1);
                    config.Taiyaki -= 50;

                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought a Strength Potion!");
                    return;
                }
                if (newresponse.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 25)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{25 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    if (config.Items.ContainsKey("Speed Potion")) config.Items["Speed Potion"] += 1;
                    else config.Items.Add("Speed Potion", 1);
                    config.Taiyaki -= 25;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought a Speed Potion!");
                    return;
                }
                if (newresponse.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 50)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{50 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.Taiyaki -= 50;
                    if (config.Items.ContainsKey("Debuff Potion")) config.Items["Debuff Potion"] += 1;
                    else config.Items.Add("Debuff Potion", 1);

                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought a Debuff Potion!");
                    return;
                }
                if (newresponse.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 75)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{75 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.Taiyaki -= 75;
                    if (config.Items.ContainsKey("Equalizer Potion")) config.Items["Equalizer Potion"] += 1;
                    else config.Items.Add("Equalizer Potion", 1);

                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought an Equalizer Potion!");
                    return;
                }
                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                    return;
                }
                if (newresponse == null)
                {
                    await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                    return;
                }
                else
                {
                    await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                    return;
                }
            }
            if (response.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                return;
            }
            if (response.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $"```xl\n[1] Weapon Mastery - 10% more damage dealt [500 Taiyakis]\n[2] Efficient Brewing - 5% increased potion effectiveness [500 Taiyakis]\n[3] Mage Mastery - 5% more spell damage delt [500 Taiyakis]\n[4] Durable Armour - 5% less damage received [500 Taiyakis]\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.\n```"; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 500)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{500 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.bookWM = true;
                    config.Taiyaki -= 500;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought the book, Weapon Mastery!");
                    return;
                }
                if (newresponse.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 500)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{500 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.bookPE = true;
                    config.Taiyaki -= 500;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought the book, Efficient Brewing!");
                    return;
                }
                if (newresponse.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 500)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{500 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.bookSD = true;
                    config.Taiyaki -= 500;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought the book, Mage Mastery!");
                    return;
                }
                if (newresponse.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 500)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{500 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.bookDR = true;
                    config.Taiyaki -= 500;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought the book, Durable Armour!");
                    return;
                }
                if (newresponse.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 500)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{500 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.bookDR = true;
                    config.Taiyaki -= 500;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought the book, Blessing of Protection!");
                    return;
                }
                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                    return;
                }
                if (newresponse == null)
                {
                    await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                    return;
                }
                else
                {
                    await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                    return;
                }
            }
            if (response.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $"```xl\n[1] Bronze Sword - 5% more damage dealt [150 Taiyakis]\n[2] Steel Sword - 10% more damage dealt [300 Taiyakis]\n[3] Gold Sword - 15% more damage dealt [500 Taiyakis]\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.\n```"; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":feet:  |  **Are you sure you want to purchase a Bronze Sword? (**150** {Emote.Parse("<:taiyaki:599774631984889857>")})**\n\nType `confirm` to continue or `cancel` to cancel.\n\n**Warning: this will replace your current weapon!**"; });
                    var newresponsee = await NextMessageAsync();
                    if (newresponsee.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        if (config.Taiyaki < 150)
                        {
                            await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{150 - config.Taiyaki}** more Taiyakis!"; });
                            return;
                        }
                        config.weapon = "bronze";
                        config.Taiyaki -= 150;
                        GlobalUserAccounts.SaveAccounts(user.Id);
                        await Context.Channel.SendMessageAsync("You have successfully bought a Bronze Sword!");
                        return;
                    }
                    if (newresponsee == null)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                        return;
                    }
                    if (newresponsee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                        return;
                    }
                }
                if (newresponse.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":feet:  |  **Are you sure you want to purchase a Steel Sword? (**300** {Emote.Parse("<:taiyaki:599774631984889857>")})**\n\nType `confirm` to continue or `cancel` to cancel.\n\n**Warning: this will replace your current weapon!**"; });
                    var newresponsee = await NextMessageAsync();
                    if (newresponsee.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        if (config.Taiyaki < 300)
                        {
                            await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{300 - config.Taiyaki}** more Taiyakis!"; });
                            return;
                        }
                        config.weapon = "steel";
                        config.Taiyaki -= 300;
                        GlobalUserAccounts.SaveAccounts(user.Id);
                        await Context.Channel.SendMessageAsync("You have successfully bought a Steel Sword!");
                        return;
                    }
                    if (newresponsee == null)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                        return;
                    }
                    if (newresponsee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                        return;
                    }
                }
                if (newresponse.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":feet:  |  **Are you sure you want to purchase a Gold Sword? (**500** {Emote.Parse("<:taiyaki:599774631984889857>")})**\n\nType `confirm` to continue or `cancel` to cancel.\n\n**Warning: this will replace your current weapon!**"; });
                    var newresponsee = await NextMessageAsync();
                    if (newresponsee.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        if (config.Taiyaki < 500)
                        {
                            await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{500 - config.Taiyaki}** more Taiyakis!"; });
                            return;
                        }
                        config.weapon = "gold";
                        config.Taiyaki -= 500;
                        GlobalUserAccounts.SaveAccounts(user.Id);
                        await Context.Channel.SendMessageAsync("You have successfully bought a Gold Sword!");
                        return;
                    }
                    if (newresponsee == null)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                        return;
                    }
                    if (newresponsee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                        return;
                    }
                }
                if (newresponse == null)
                {
                    await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                    return;
                }
                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                    return;
                }
                else
                {
                    await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                    return;
                }
            }
            if (response.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                {
                    await shop.ModifyAsync(m => { m.Content = $"```xl\n[1] Bronze Armour - 5% less damage received [150 Taiyakis]\n[2] Steel Armour - 10% less damage received [300 Taiyakis]\n[3] Gold Armour - 15% less damage received [500 Taiyakis]\n[4] Platinum Armour - 20% less damage received [1000 Taiyakis]\n[5] Reinforced Armour - Your opponent's spells have no effect [1000 Taiyakis]\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.\n```"; });
                    var newresponse = await NextMessageAsync();
                    if (newresponse.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await shop.ModifyAsync(m => { m.Content = $":feet:  |  **Are you sure you want to purchase a Bronze Armour Set? (**150** {Emote.Parse("<:taiyaki:599774631984889857>")})**\n\nType `confirm` to continue or `cancel` to cancel.\n\n**Warning: this will replace your current armour set!**"; });
                        var newresponsee = await NextMessageAsync();
                        if (newresponsee.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                        {
                            if (config.Taiyaki < 150)
                            {
                                await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{150 - config.Taiyaki}** more Taiyakis!"; });
                                return;
                            }
                            config.armour = "bronze";
                            config.Taiyaki -= 150;
                            GlobalUserAccounts.SaveAccounts(user.Id);
                            await Context.Channel.SendMessageAsync("You have successfully bought a Bronze Armour Set!");
                            return;
                        }
                        if (newresponsee == null)
                        {
                            await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                            return;
                        }
                        if (newresponsee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                        {
                            await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                            return;
                        }
                    }
                    if (newresponse.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await shop.ModifyAsync(m => { m.Content = $":feet:  |  **Are you sure you want to purchase a Steel Armour Set? (**300** {Emote.Parse("<:taiyaki:599774631984889857>")})**\n\nType `confirm` to continue or `cancel` to cancel.\n\n**Warning: this will replace your current armour set!**"; });
                        var newresponsee = await NextMessageAsync();
                        if (newresponsee.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                        {
                            if (config.Taiyaki < 300)
                            {
                                await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{300 - config.Taiyaki}** more Taiyakis!"; });
                                return;
                            }
                            config.armour = "steel";
                            config.Taiyaki -= 300;
                            GlobalUserAccounts.SaveAccounts(user.Id);
                            await Context.Channel.SendMessageAsync("You have successfully bought a Steel Armour Set!");
                            return;
                        }
                        if (newresponsee == null)
                        {
                            await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                            return;
                        }
                        if (newresponsee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                        {
                            await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                            return;
                        }
                    }
                    if (newresponse.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await shop.ModifyAsync(m => { m.Content = $":feet:  |  **Are you sure you want to purchase a Gold Armour Set? (**500** {Emote.Parse("<:taiyaki:599774631984889857>")})**\n\nType `confirm` to continue or `cancel` to cancel.\n\n**Warning: this will replace your current armour set!**"; });
                        var newresponsee = await NextMessageAsync();
                        if (newresponsee.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                        {
                            if (config.Taiyaki < 500)
                            {
                                await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{500 - config.Taiyaki}** more Taiyakis!"; });
                                return;
                            }
                            config.armour = "gold";
                            config.Taiyaki -= 500;
                            GlobalUserAccounts.SaveAccounts(user.Id);
                            await Context.Channel.SendMessageAsync("You have successfully bought a Gold Armour Set!");
                            return;
                        }
                        if (newresponsee == null)
                        {
                            await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                            return;
                        }
                        if (newresponsee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                        {
                            await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                            return;
                        }
                    }
                    if (newresponse.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await shop.ModifyAsync(m => { m.Content = $":feet:  |  **Are you sure you want to purchase a Platinum Armour Set? (**1000** {Emote.Parse("<:taiyaki:599774631984889857>")})**\n\nType `confirm` to continue or `cancel` to cancel.\n\n**Warning: this will replace your current armour set!**"; });
                        var newresponsee = await NextMessageAsync();
                        if (newresponsee.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                        {
                            if (config.Taiyaki < 1000)
                            {
                                await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{1000 - config.Taiyaki}** more Taiyakis!"; });
                                return;
                            }
                            config.armour = "platinum";
                            config.Taiyaki -= 1000;
                            GlobalUserAccounts.SaveAccounts(user.Id);
                            await Context.Channel.SendMessageAsync("You have successfully bought a Platinum Armour Set!");
                            return;
                        }
                        if (newresponsee == null)
                        {
                            await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                            return;
                        }
                        if (newresponsee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                        {
                            await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                            return;
                        }
                    }
                    if (newresponse.Content.Equals("5", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await shop.ModifyAsync(m => { m.Content = $":feet:  |  **Are you sure you want to purchase a Reinforced Armour Set? (**1000** {Emote.Parse("<:taiyaki:599774631984889857>")})**\n\nType `confirm` to continue or `cancel` to cancel.\n\n**Warning: this will replace your current armour set!**"; });
                        var newresponsee = await NextMessageAsync();
                        if (newresponsee.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                        {
                            if (config.Taiyaki < 1000)
                            {
                                await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{1000 - config.Taiyaki}** more Taiyakis!"; });
                                return;
                            }
                            config.armour = "reinforced";
                            config.Taiyaki -= 1000;
                            GlobalUserAccounts.SaveAccounts(user.Id);
                            await Context.Channel.SendMessageAsync("You have successfully bought a Reinforced Armour Set!");
                            return;
                        }
                        if (newresponsee == null)
                        {
                            await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                            return;
                        }
                        if (newresponsee.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                        {
                            await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                            return;
                        }
                    }
                    if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                        return;
                    }
                    if (newresponse == null)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                        return;
                    }
                    else
                    {
                        await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                        return;
                    }
                }
            }
            if (response.Content.Equals("5", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $"```xl\n[1] Metallic Acid - Destorys your opponent's armour set [500 Taiyakis]\n[2] Weapon Liquifier - Destroys your opponent's weapon [500 Taiyakis]\n[3] Basic Treatment (Single Time Use) - Immune to **Metallic Acid** and **Weapon Liquifier** [600 Taiyakis]\n[4] Divine Shield (Active Throughout Duel) - Immune to **Metallic Acid**, **Weapon Liquifier**, and a **Poisonous Weapon** [800 Taiyakis]\n[5] Vile Of Poison - Make your weapon poisonous (+15% more damage) [200 Taiyakis]\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.\n```"; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 500)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{500 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    if (config.Items.ContainsKey("Metallic Acid")) config.Items["Metallic Acid"] += 1;
                    else config.Items.Add("Metallic Acid", 1);
                    config.Taiyaki -= 500;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought **Metallic Acid x1**!");
                    return;
                }
                if (newresponse.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 500)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{500 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    if (config.Items.ContainsKey("Weapon Liquifier")) config.Items["Weapon Liquifier"] += 1;
                    else config.Items.Add("Weapon Liquifier", 1);
                    config.Taiyaki -= 500;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought **Weapon Liquifier x1**!");
                    return;
                }
                if (newresponse.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 600)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{600 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    if (config.Items.ContainsKey("Basic Treatment")) config.Items["Basic Treatment"] += 1;
                    else config.Items.Add("Basic Treatment", 1);
                    config.Taiyaki -= 600;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought **Basic Treatment x1**!");
                    return;
                }
                if (newresponse.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 800)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{800 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    if (config.Items.ContainsKey("Divine Shield")) config.Items["Divine Shield"] += 1;
                    else config.Items.Add("Divine Shield", 1);
                    config.Taiyaki -= 800;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought **Divine Shield x1**!");
                    return;
                }
                if (newresponse.Content.Equals("5", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 200)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{200 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    if (config.Items.ContainsKey("Vile Of Poison")) config.Items["Vile Of Poison"] += 1;
                    else config.Items.Add("Vile Of Poison", 1);
                    config.Taiyaki -= 200;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought **Vile Of Poison x1**!");
                    return;
                }
                if (newresponse == null)
                {
                    await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                    return;
                }
                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                    return;
                }
                else
                {
                    await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                    return;
                }
            }
            if (response.Content.Equals("6", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $"```xl\n[1] Blessing of Protection - Grants a free **Basic Treatment** at the beginning of each duel [7500 Taiyakis]\n[2] Blessing of Swiftness - Small chance to attack twice each turn [7500 Taiyakis]\n[3] Blessing of War - 10% more damage dealt [7500 Taiyakis]\n[4] Blessing of Strength - Start off with 25 more health [7500 Taiyakis]\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.\n```"; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 7500)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{7500 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.blessingProtection = true;
                    config.Taiyaki -= 7500;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought the Blessing of Protection!");
                    return;
                }
                if (newresponse.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 7500)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{77500 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.blessingSwiftness = true;
                    config.Taiyaki -= 7500;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought the Blessing of Swiftness!");
                    return;
                }
                if (newresponse.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 7500)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{7500 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.blessingWar = true;
                    config.Taiyaki -= 7500;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought the Blessing of War!");
                    return;
                }
                if (newresponse.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 7500)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{7500 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.blessingStrength = true;
                    config.Taiyaki -= 7500;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought the Blessing of Strength!");
                    return;
                }
                if (newresponse.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 7500)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{7500 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.bookDR = true;
                    config.Taiyaki -= 7500;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync("You have successfully bought the book, Blessing of Protection!");
                    return;
                }
                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":shield:   |  **{Context.User.Username}**, purchase cancelled."; });
                    return;
                }
                else
                {
                    await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                    return;
                }
            }
            if (response == null)
            {
                await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                return;
            }
            else
            {
                await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                return;
            }
        }
         
        [Command("inventory")]
        [Summary("View your inventory for duels, or mention someone to see their inventory")]
        [Remarks("Usage: n!inventory @user Ex: n!inventory @Phytal")]
        public async Task DuelsInventory([Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var account = GlobalUserAccounts.GetUserAccount(target);
            var embed = new EmbedBuilder();
            embed.WithTitle($"{target.Username}'s Inventory");
            embed.AddField("Potions", potions);
            embed.AddField("Books", books);
            embed.AddField("Items", items);
            embed.AddField("Armour", armour);
            embed.AddField("Weapon", weapon);
            embed.AddField("Blessings", blessings);
            embed.AddField("Active Blessings", activeBlessing);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("attacks")]
        [Summary("View your learned attacks for duels")]
        [Remarks("Usage: n!attacks @user (or leave @user blank to see your own) Ex: n!attacks @Phytal")]
        public async Task LearnedAttacks([Remainder]string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var account = GlobalUserAccounts.GetUserAccount(target);
            var embed = new EmbedBuilder();
            embed.WithTitle($"{target.Username}'s Learned Attacks");
            string attacklist = string.Empty;
            if (account.Attacks?.Any() != true)
            {
                account.Attacks.Add("Slash");
                account.Attacks.Add("Absorb");
                account.Attacks.Add("Block");
                account.Attacks.Add("Deflect");
                account.Attack1 = "Slash";
                account.Attack2 = "Absorb";
                account.Attack3 = "Block";
                account.Attack4 = "Deflect";
            }
            GlobalUserAccounts.SaveAccounts();
            foreach (var attack in account.Attacks)
            {
                attacklist += $"\n**{attack}**";
            }
            embed.AddField("Learned Attacks", attacklist);
            embed.AddField("Current Attack 1", account.Attack1);
            embed.AddField("Current Attack 2", account.Attack2);
            embed.AddField("Current Attack 3", account.Attack3);
            embed.AddField("Current Attack 4", account.Attack4);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("replaceattack")]
        [Summary("Replace one of your learned attacks with another one for duels")]
        [Remarks("Usage: n!replaceattack <attack # (can be checked with n!attacks)> <attack you want to replace with (must have learned it)> Ex: n!replaceattack 1 Slash")]
        public async Task ReplaceAttack(int attackNum, [Remainder]string attackName = "")
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (config.Attacks.Contains(attackName))
            {
                if (attackNum == 1)
                {
                    string oldAttack = config.Attack1;
                    config.Attack1 = attackName;
                    GlobalUserAccounts.SaveAccounts();
                    await Context.Channel.SendMessageAsync($":white_check_mark:  | Successfully replaced {oldAttack} with {attackName}");
                }
                if (attackNum == 2)
                {
                    string oldAttack = config.Attack2;
                    config.Attack2 = attackName;
                    GlobalUserAccounts.SaveAccounts();
                    await Context.Channel.SendMessageAsync($":white_check_mark:  | Successfully replaced {oldAttack} with {attackName}");
                }
                if (attackNum == 3)
                {
                    string oldAttack = config.Attack3;
                    config.Attack3 = attackName;
                    GlobalUserAccounts.SaveAccounts(); GlobalUserAccounts.SaveAccounts();
                    await Context.Channel.SendMessageAsync($":white_check_mark:  | Successfully replaced {oldAttack} with {attackName}");
                }
                if (attackNum == 4)
                {
                    string oldAttack = config.Attack4;
                    config.Attack4 = attackName;
                    GlobalUserAccounts.SaveAccounts();
                    await Context.Channel.SendMessageAsync($":white_check_mark:  | Successfully replaced {oldAttack} with {attackName}");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync($"You haven't learned {attackName} yet! \n*Make sure you typed your desired attack's name correctly (psst, It's case-sensitive!).*");
            }
        }
    }
}

