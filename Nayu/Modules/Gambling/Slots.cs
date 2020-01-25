using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Features.Economy;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Helpers;
using Nayu.Modules.Chomusuke.Dueling.Enums;
using Nayu.Preconditions;

namespace Nayu.Modules.Gambling
{
    public class Slots : NayuModule
    {        
        [Subject(Categories.EconomyGambling)]
        [Command("newSlot")]
        [Alias("newSlots")]
        [Summary("Generates a new slot machine")]
        [Remarks("Ex: n!newSlot")]
        [Cooldown(5)]
        public async Task NewSlot(int amount = 0)
        {
            Global.slot = new Slot(amount);
            await ReplyAsync("✅  **|** A new slot machine got generated!");
        }
        
        [Subject(Categories.EconomyGambling)]
        [Command("slots")]
        [Alias("slot")]
        [Summary("Play a game of slots! Ex: n!slots <amount you bet on>")]
        [Remarks("n!slots <amount you want to gamble> Ex: n!slots 50")]
        [Cooldown(5)]
        public async Task SpinSlot(uint amount)
        {
            if (amount < 1)
            {
                await ReplyAsync($"{Global.ENo} **|** You can't spin for that amount of Taiyakis.");
                return;
            }
            var account = GlobalUserAccounts.GetUserAccount(Context.User.Id);
            if (account.Taiyaki < amount)
            {
                await ReplyAsync($"🖐️ **|** Sorry but it seems like you don't have enough Taiyakis... You only have {account.Taiyaki}.");
                return;
            }

            account.Taiyaki -= amount;
            GlobalUserAccounts.SaveAccounts(account.Id);

            string slotEmojis = Global.slot.Spin();
            var payoutAndFlavour = Global.slot.GetPayoutAndFlavourText(amount);

            if (payoutAndFlavour.Item1 > 0)
            {
                account.Taiyaki += payoutAndFlavour.Item1;
                GlobalUserAccounts.SaveAccounts(account.Id);
            }

            IUserMessage msg = await ReplyAsync(slotEmojis);
            await Task.Delay(1000);
            await ReplyAsync(payoutAndFlavour.Item2);

        }
        
        [Subject(Categories.EconomyGambling)]
        [Command("showSlots")]
        [Alias("showSlot")]
        [Summary("Shows the slots wheel (don't worry it gets randomized every time :stuck_out_tongue: ")]
        [Remarks("Ex: n!showSlots")]
        [Cooldown(5)]
        public async Task ShowSlot()
        {
            await ReplyAsync(String.Join("\n", Global.slot.GetCylinderEmojis(true)));
        }
    }
}
