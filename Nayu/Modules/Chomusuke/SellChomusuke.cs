using Discord;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Chomusuke.Dueling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nayu.Modules.Chomusuke
{
    public class SellChomusuke : NayuModule
    {
        //TODO: finish
        [Command("sell")]
        public async Task SellChomusukeAsync()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            var activeChom = ActiveChomusuke.GetOneActiveChomusuke(Context.User.Id);
            string shoptext = $":department_store:  **| Are you sure you want to sell your Chomusuke, {activeChom.Name}? [y/n]";
            var shop = await Context.Channel.SendMessageAsync(shoptext);
            var response = await NextMessageAsync();
            if (response == null)
            {
                await shop.ModifyAsync(m => { m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity"; });
                return;
            }
            if (response.Content.Equals("y", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
            {
                var value = GetChomusukeValue(activeChom);
                await shop.ModifyAsync(m => { m.Content = $":feet:  |  **Your {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke is worth {value} Taiyakis, do you wish to sell it? (**900** {Emote.Parse("<:taiyaki:599774631984889857>")})**\n\nType `confirm` to continue or `cancel` to cancel.\n\n**Warning: this is irreversible!**"; });
                var newresponse = await NextMessageAsync();
                if (newresponse.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
                {
                    if (config.Taiyaki < 900)
                    {

                    }
                    config.NormalCapsule += 1;
                    config.Taiyaki += value;
                    GlobalUserAccounts.SaveAccounts(Context.User.Id);
                    await Context.Channel.SendMessageAsync($"You have successfully bought a {Emote.Parse("<:chomusuke:601183653657182280>")} Normal Chomusuke Capsule!");
                    return;
                }
                if (newresponse.Content.Equals("n", StringComparison.CurrentCultureIgnoreCase) && (response.Author.Equals(Context.User)))
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
        }

        public static ulong GetChomusukeValue(Core.Entities.Chomusuke chom)
        {
            ulong value = 0;
            return value;
        }
    }
}
