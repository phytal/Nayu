using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using Nayu.Features.Economy;
using System;
using Nayu.Features.GlobalAccounts;
using Nayu.Preconditions;
using Nayu.Core.Modules;

namespace Nayu.Modules
{
    public class Slots : NayuModule
    {
        [Command("newslot")]
        [Alias("newslots")]
        [Summary("Generates a new slot machine (duh)")]
        [Remarks("Ex: n!newslot")]
        [Cooldown(10)]
        public async Task NewSlot(int amount = 0)
        {
            Global.slot = new Slot(amount);
            await ReplyAsync(":white_check_mark:  | A new slotmachine got generated!");
        }

        [Command("slots")]
        [Alias("slot")]
        [Summary("Play a game of slots! Ex: n!slots <amount you bet on>")]
        [Remarks("n!slots <amount you want to gamble> Ex: n!slots 50")]
        [Cooldown(10)]
        public async Task SpinSlot(uint amount)
        {
            if (amount < 1)
            {
                await ReplyAsync($":x:  | You can't spin for that amount of Taiyakis.");
                return;
            }
            var account = GlobalUserAccounts.GetUserAccount(Context.User.Id);
            if (account.Taiyaki < amount)
            {
                await ReplyAsync($":hand_splayed:  | Sorry but it seems like you don't have enough Taiyakis... You only have {account.Taiyaki}.");
                return;
            }

            account.Taiyaki -= amount;
            GlobalUserAccounts.SaveAccounts();

            string slotEmojis = Global.slot.Spin();
            var payoutAndFlavour = Global.slot.GetPayoutAndFlavourText(amount);

            if (payoutAndFlavour.Item1 > 0)
            {
                account.Taiyaki += payoutAndFlavour.Item1;
                GlobalUserAccounts.SaveAccounts();
            }

            IUserMessage msg = await ReplyAsync(slotEmojis);
            await Task.Delay(1000);
            await ReplyAsync(payoutAndFlavour.Item2);

        }

        [Command("showslots")]
        [Alias("showslot")]
        [Summary("Shows the slots wheel (don't worry it gets randomized everytime :stuck_out_tongue: ")]
        [Remarks("Ex: n!showslots")]
        [Cooldown(10)]
        public async Task ShowSlot()
        {
            await ReplyAsync(String.Join("\n", Global.slot.GetCylinderEmojis(true)));
        }
    }
}
