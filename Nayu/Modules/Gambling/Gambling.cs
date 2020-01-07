using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Preconditions;

namespace Nayu.Modules.Gambling
{
    public class Gambling : NayuModule
    {
        [Command("coinflip")]
        [Summary("Flips a coin and if you win you earn x2 of the Taiyakis you betted. If lost you lose your Taiyakis.")]
        [Alias("Coin", "flip", "cf")]
        [Remarks("n!cf <side (heads/tails)> <amount of Taiyakis you want to flip for(you will earn nothing if left empty)> Ex: n!cf tails 20")]
        [Cooldown(5)]
        public async Task CoinFlip(string side, uint amount = 0)
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (config.Taiyaki < amount)
            {
                await SendMessage(Context, null, "Do you think you can trick me, you liar? (Not enough Taiyaki)");
                return;
            }
            config.Taiyaki = config.Taiyaki - amount;
            Random rand = new Random();
            string sid = side.ToLower();
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle($"<:coin:459944364546981909> Coin Flip for {amount} Taiyakis on the Side of {sid}.");
            if (sid == "tails")
            {
                int randomNumber = rand.Next(1, 4);//you know gambling is dangerous right lol you have a low chance of winning :)
                if (randomNumber == 1)//win
                {
                    config.Taiyaki = config.Taiyaki + ((ulong)amount * 2);
                    embed.WithDescription($"I guess heads. And the coin landed on tails! Alright, you beat me, for *now*. Here's **{amount * 2}** Taiyakis!");
                }
                else
                {
                    embed.WithDescription($"I guess heads. And the coin landed on heads! Sorry **{Context.User.Username}**, but your **{amount}** Taiyakis are mine non!");
                }
            }
            if (sid == "heads")
            {
                int randomNumber = rand.Next(1, 4);//you know gambling is dangerous right lol
                if (randomNumber == 1)//win
                {
                    config.Taiyaki = config.Taiyaki + ((ulong)amount * 2);
                    embed.WithDescription($"I guess tails. And the coin landed on heads! Alright, you beat me, for *now*. Here's **{amount * 2}** Taiyakis!");
                }
                else
                {
                    embed.WithDescription($"I guess tails. And the coin landed on tails! Sorry **{Context.User.Username}**, but your **{amount}** Taiyakis are mine non!");
                }
            }
            GlobalUserAccounts.SaveAccounts(config.Id);
            await SendMessage(Context, embed);
        }

        [Command("roll")]
        [Summary("Rolls a Dice")]
        [Alias("dice", "dice roll")]
        [Remarks("Ex: n!roll")]
        [Cooldown(5)]
        public async Task RollDice(uint amountBetted)
        {
            //this game basically is that if you roll 2 numbers that are above 50, you earn money, but if below you lose. and ofc if it is 50 then you get all your money
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (config.Taiyaki < amountBetted)
            {
                await SendMessage(Context, null, "Do you think you can trick me, you liar? (Not enough Taiyaki)");
                return;
            }
            int randomNumber1 = Global.Rng.Next(1, 11);
            int randomNumber2 = Global.Rng.Next(1, 11);
            int amountGained = 0;
            int product = randomNumber1 * randomNumber2;

            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            if (product > 50)
            {
                _ = amountGained == amountBetted * (product / 20);
                embed.WithTitle($":game_die:  | You Rolled **{randomNumber1}** and **{randomNumber2}** ({product}). Alright you win this time, here are your Taiyakis..");
            }
            else if(product < 50)
            {
                _ = amountGained == amountBetted*-1;
                embed.WithTitle($":game_die:  | You Rolled **{randomNumber1}** and **{randomNumber2}** ({ product}). HAH YOUR TAIYAKIS ARE MINEEEE");
            }
            else if (product == 50)
            {
                _ = amountGained == amountBetted;
                embed.WithTitle($":game_die:  | You Rolled **{randomNumber1}** and **{randomNumber2}** ({product}). You know what, I'll be nice and give back your Taiyakis");
            }

            bool isNegative = amountGained > 0;
            _ = isNegative ? config.Taiyaki += (ulong)amountGained : config.Taiyaki -= (ulong)amountGained;
            await SendMessage(Context, embed);
        }
    }
}
