using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Chomusuke.Dueling;
using System;
using System.Threading.Tasks;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.Chomusuke
{
    using Chomusuke = Nayu.Core.Entities.Chomusuke;
    public class SellChomusuke : NayuModule
    {
        [Subject(ChomusukeCategories.Chomusuke)]
        [Command("sell")]
        [Summary("Sells your active Chomusuke. Note: Make sure the Chomusuke you want to sell is in your active slot!")]
        [Remarks("Ex: n!sell")]
        [Cooldown(3)]
        public async Task SellChomusukeAsync()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            var activeChom = ActiveChomusuke.GetOneActiveChomusuke(Context.User.Id);
            if ((DateTime.Now - activeChom.BoughtDay).Days < 1)
                throw new Exception("You cannot sell a Chomusuke that's under a day old!");
            var shopText =
                $"🏬  **| Are you sure you want to sell your active Chomusuke, {activeChom.Name}? [y/n]";
            var shop = await Context.Channel.SendMessageAsync(shopText);
            var response = await NextMessageAsync();
            if (response == null)
            {
                await shop.ModifyAsync(m =>
                {
                    m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity";
                });
            }

            else if (response.Content.Equals("y", StringComparison.CurrentCultureIgnoreCase))
            {
                var value = GetChomusukeValue(activeChom);
                await shop.ModifyAsync(m =>
                {
                    m.Content =
                        $"🐾  |  **Your {Global.EChomusuke} Chomusuke is worth {value} Taiyakis, do you wish to sell it? (**900** {Global.ETaiyaki})**\n\nType `confirm` to continue or `cancel` to cancel.\n\n**Warning: this is irreversible!**";
                });
                var newResponse = await NextMessageAsync();
                if (newResponse.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase))
                {
                    await ActiveChomusuke.ConvertOneActiveVariable(Context.User.Id, Global.NewChomusuke);
                    config.ActiveChomusuke = 0;
                    config.Taiyaki += value;
                    GlobalUserAccounts.SaveAccounts(Context.User.Id);
                    await SendMessage(Context, null, $"You have successfully sold your Chomusuke {activeChom.Name}");
                }
                else if (response.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
                {
                    await shop.ModifyAsync(m =>
                    {
                        m.Content = $"🐾  **|**  **{Context.User.Username}**, action cancelled.";
                    });
                }
            }
            else if (response.Content.Equals("n", StringComparison.CurrentCultureIgnoreCase))
            {
                await shop.ModifyAsync(m =>
                {
                    m.Content = $"🐾  **|**  **{Context.User.Username}**, action cancelled.";
                });
            }
            else
            {
                await shop.ModifyAsync(m =>
                {
                    m.Content = $"{Global.ENo}  **|** That is an invalid response. Please try again.";
                });
            }
        }

        //TODO: better chomusuke value calculation
        private static ulong GetChomusukeValue(Chomusuke chom)
        {
            double value = 400;
            value *= chom.CP * .04;
            if (chom.Shiny) value += 1000;
            if (chom.Trait1 == Trait.Lucky) value += 1000;
            return (ulong) Math.Round(value);
        }
    }
}