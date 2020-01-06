using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Chomusuke.Dueling;


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
            var activeChomusuke = ActiveChomusuke.GetOneActiveChomusuke(user.Id);
            string shoptext = ":department_store:  **|  Chomusuke Shop** \n ```xl\nPlease select the purchase you would like to make.\n\n[1] Capsules\n[2] Boosts\n[3] Items\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.```";
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
                    if (config.Taiyaki < 900)
                    {
                        await shop.ModifyAsync(m => { m.Content = $"**<:no:453716729525174273>  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{900 - config.Taiyaki}** more Taiyakis!"; });
                        return;
                    }
                    config.NormalCapsule += 1;
                    config.Taiyaki -= 900;
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
            //TODO: add boosts
            if (response.Content.Equals("2", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $"```xl\n[1] Medicine - cures your Chomusuke's sickness [400 {Emote.Parse("<:taiyaki:599774631984889857>")}]\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.```"; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await ChooseChomusuke.ChooseActionAsync(user, "cure");
                    config.Taiyaki -= 400;
                    GlobalUserAccounts.SaveAccounts(config.Id);
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
                }
            }
            
            if (response.Content.Equals("3", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                await shop.ModifyAsync(m => { m.Content = $"```xl\n[1] Medicine - cures your Chomusuke's sickness [400 {Emote.Parse("<:taiyaki:599774631984889857>")}]\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.```"; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("1", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    await ChooseChomusuke.ChooseActionAsync(user, "cure");
                    config.Taiyaki -= 400;
                    GlobalUserAccounts.SaveAccounts(config.Id);
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
                }
            }
            else
            {
                await shop.ModifyAsync(m => { m.Content = "<:no:453716729525174273>  | That is an invalid response. Please try again."; });
            }
        }
    }
}
