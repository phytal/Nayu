using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;
using Discord.WebSocket;


namespace Nayu.Modules.Chomusuke
{
    class Shop : NayuModule
    {
        [Command("chomusuke buy"), Alias("w shop", "w buy")]
        [Summary("Opens the Chomusuke shop menu!")]
        [Remarks("Ex: n!c shop")]
        public async Task ChomusukeBuy()
        {
            var user = Context.User as SocketGuildUser;
            var config = GlobalUserAccounts.GetUserAccount(user);
            var configg = GlobalUserAccounts.GetUserAccount(user);
            string shoptext = ":department_store:  **|  Chomusuke Shop** \n ```xl\nPlease select the purchase you would like to make.\n\n[1] Capsules\n[2] Room Upgrades\n[3] Room Downgrade\n[4] Boosts + Items\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.```";
            var shop = await Context.Channel.SendMessageAsync(shoptext);
            var response = await NextMessageAsync();
            if (response == null)
            {
                await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                return;
            }
            if (response.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $":feet:  |  **Are you sure you want to purchase a {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke? (**900** {Emote.Parse("<:taiyaki:599774631984889857>")})**\n\nType `confirm` to continue or `cancel` to cancel.\n\n**Warning: this will replace your current Chomusuke!**"; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (configg.Taiyaki < config.RoomCost)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{900 - configg.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.Have = true;
                    configg.Taiyaki -= config.RoomCost;
                    config.BoughtSince = DateTime.UtcNow;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await Context.Channel.SendMessageAsync($"You have successfully bought a {Emote.Parse("<:chomusuke:601183653657182280>")} Normal Chomusuke Capsule!");
                    return;
                }
                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":feet:  |  **{Context.User.Username}**, purchase cancelled."; });
                    return;
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
            if (response.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $":feet:  |  **{Context.User.Username}**, purchase cancelled."; });
                return;
            }

            if (response.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $":house:  |  Your current room is: **{GetRooms(config.rLvl)}**. Are you sure you want to upgrade to **{GetRooms(config.rLvl + 1)}**? (**{config.RoomCost}** {Emote.Parse("<:taiyaki:599774631984889857>")}) \n\nType `confirm` to continue or `cancel` to cancel."; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (configg.Taiyaki < config.RoomCost)
                    {
                        await Context.Channel.SendMessageAsync($"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{config.RoomCost - configg.Taiyaki}** more Taiyakis!");
                        return;
                    }
                    else
                    {
                        configg.Taiyaki -= config.RoomCost;
                        config.rLvl += 1;
                        GlobalUserAccounts.SaveAccounts(user.Id);
                        await Context.Channel.SendMessageAsync($":house:  |  **{Context.User.Username}**, your room has been upgraded to **{GetRooms(config.rLvl)}**");
                        return;
                    }
                }
                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":feet:  |  **{Context.User.Username}**, purchase cancelled."; });
                    return;
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

            if (response.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                if (config.RoomCost != 0)
                {
                    await shop.ModifyAsync(m => { m.Content = $":house:  |  **Your current room is: {GetRooms(config.rLvl)}. Are you sure you want to downgrade to {GetRooms(config.rLvl - 1)}? This will not refund your Taiyakis!** \n\nType `confirm` to continue or `cancel` to cancel."; });
                    var newresponse = await NextMessageAsync();
                    if (newresponse.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        config.rLvl -= 1;
                        GlobalUserAccounts.SaveAccounts(user.Id);
                        await shop.ModifyAsync(m => { m.Content = $":house:  |  **{Context.User.Username}**, your room has been downgraded to **{GetRooms(config.rLvl)}**"; });
                        return;
                    }
                    if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                    {
                        await shop.ModifyAsync(m => { m.Content = $":feet:  |  **{Context.User.Username}**, purchase cancelled."; });
                        return;
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
                else
                {
                    await Context.Channel.SendMessageAsync(":octagonal_sign:  | You cannot downgrade your room any further, as you have the loewst possible room");
                }

            }
            if (response.Content.Equals("4", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $"```xl\n[1] Medicine - cures your Chomusuke's sickness [200 {Emote.Parse("<:taiyaki:599774631984889857>")}]\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.```"; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    var chooseChomusuke = await NextMessageAsync();

                    config.Sick = false;
                    config.Waste = 0;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await shop.ModifyAsync(m => { m.Content = $":pill:  |  **{Context.User.Username}**, your {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke has been cured of it's sickness! Make sure to keep looking after it!"; });
                    return;
                }
                if (newresponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await shop.ModifyAsync(m => { m.Content = $":feet:  |  **{Context.User.Username}**, purchase cancelled."; });
                    return;
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
            else
            {
                await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
                return;
            }
        }
    }
}
