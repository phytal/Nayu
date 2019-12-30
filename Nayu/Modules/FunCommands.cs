using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Preconditions;
using Nayu.Helpers;

namespace Nayu.Modules
{
    public class FunCommands : NayuModule
    {
        [Command("ping")]
        [Summary("Ping Pong!")]
        [Remarks("Ex: n!ping")]
        [Cooldown(5)]
        public async Task Ping()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle($":ping_pong:  | Pong! {(Context.Client as DiscordShardedClient).Latency}ms");
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        string[] predictionsTexts = new string[]
            {
                ":8ball:  | It is certain",
                ":8ball:  | It is decidedly so",
                ":8ball:  | Without a doubt",
                ":8ball:  | Yes definitely",
                ":8ball:  | You may rely on it",
                ":8ball:  | As I see it, yes",
                ":8ball:  | Most likely",
                ":8ball:  | Outlook good",
                ":8ball:  | Yes",
                ":8ball:  | Signs point to yes",
                ":8ball:  | Reply hazy try again",
                ":8ball:  | Ask again later",
                ":8ball:  | Better not tell you now",
                ":8ball:  | Zzzzz...",
                ":8ball:  | Concentrate on my oppai and ask again!",
                ":8ball:  | Don't count on it",
                ":8ball:  | My reply is no",
                ":8ball:  | My sources say no",
                ":8ball:  | Outlook not so good",
                ":8ball:  | Very doubtful"
            };
        Random rand = new Random();

        [Command("8ball")]
        [Alias("eightball")]
        [Summary("Gives a prediction")]
        [Remarks("n!8ball <your prediction> Ex: n!8ball am I loved?")]
        [Cooldown(5)]
        public async Task EightBall([Remainder] string input)
        {
            int randomIndex = rand.Next(predictionsTexts.Length);
            string text = predictionsTexts[randomIndex];
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle(text + ", " + Context.User.Username);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }


        [Command("echo")]
        [Summary("Make me say a message!")]
        [Remarks("n!echo <what you want the bot to say> Ex: n!echo I like to eat oreos")]
        [Cooldown(5)]
        public async Task Echo([Remainder] string message)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var LocalTime = DateTime.Now;
            var Sender = Context.Message.Author;

            var embed = new EmbedBuilder();
            embed.WithTitle(message);
            embed.WithFooter(LocalTime + " Message from " + Sender);
            embed.WithColor(37, 152, 255);

            await Context.Channel.SendMessageAsync("", embed: embed.Build());
            if (config.MassPingChecks == true)
            {
                if (message.Contains("@everyone") || message.Contains("@here")) return;
            }
            await Context.Message.DeleteAsync();
        }


        [Command("lmgtfy")]
        [Summary("Sends an Let me Google that for you link with what you inputed")]
        [Remarks("n!lmgtfy <what you want to search up> Ex: n!lmgtfy how to use discord")]
        [Cooldown(10)]
        public async Task lmgtfy([Remainder]string link = "enter something")
        {
            link = link.Replace(' ', '+');
            await ReplyAsync("https://lmgtfy.com/?q=" + link);
        }


        [Command("Lenny")]
        [Summary("Sends a lenny face ( ͡° ͜ʖ ͡°)")]
        [Remarks("Ex: n!lenny")]
        [Cooldown(5)]
        public async Task Lenny()
        {
            await Context.Channel.SendMessageAsync("( ͡° ͜ʖ ͡°)");
        }

        [Command("Prefix")]
        [Summary("Show's you the server prefix")]
        [Remarks("Ex: n!prefix")]
        [Cooldown(5)]
        public async Task GetPrefixForServer()
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            string prefix;
            switch (config)
            {
                case null:
                    prefix = "n!";
                    break;
                default:
                    prefix = config.CommandPrefix;
                    break;
            }

            await Context.Channel.SendMessageAsync($"The prefix for this server is {prefix}.");
        }

        [Command("ratewaifu")]
        [Summary("Rates your waifu :3")]
        [Remarks("n!ratewaifu <whoever (waifu) that you want to rate> Ex: n!ratewaifu Taiyakiman22")]
        [Cooldown(5)]
        public async Task RateWaifu([Remainder]string input)
        {
            Random rnd = new Random();
            int rating = rnd.Next(101);
            await Context.Channel.SendMessageAsync($"I'd rate {input} a **{rating} / 100**");
        }

        [Command("bigletter")]
        [Alias("emoji", "emotion", "emotify")]
        [Remarks("n!bigletter <whatever you want to 'emotify'> Ex: n!bigletter hello how is your day")]
        [Cooldown(5)]
        public async Task Emotify([Remainder] string args)
        {
            string[] convertorArray = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            args = args.ToLower();
            var convertedText = "";
            foreach (var c in args)
            {
                if (char.IsLetter(c)) convertedText += $":regional_indicator_{c}:";
                else if (char.IsDigit(c)) convertedText += $":{convertorArray[(int)char.GetNumericValue(c)]}:";
                else if (c == '.') convertedText += " ⏺ ";
                else if (c == '?') convertedText += "❓ ";
                else if (c == '!') convertedText += "❗ ";
                else convertedText += c;
                if (char.IsWhiteSpace(c)) convertedText += "  ";
            }
            await ReplyAsync(convertedText);
        }

        [Command("woop")]
        [Summary("Woop! <o/")]
        [Remarks("Ex: n!coop")]
        [Cooldown(5)]
        public async Task Woop()
        {
            await Context.Channel.SendFileAsync(@Path.Combine(Constants.ResourceFolder, Constants.MemeFolder, "woop.gif"));
        }

        [Command("rps")]
        [Summary("Rock, Paper Scissors!")]
        [Remarks("n!rps <rock/paper/scissors> Ex: n!rps rock")]
        [Cooldown(5)]
        public async Task rps([Remainder] string play)
        {
            play = play.ToLower();
            Random rnd = new Random();
            int choice = rnd.Next(4); //1= rock 2= paper 3= scissors
            if (play == "rock")
            {
                if (choice == 1)
                {
                    await Context.Channel.SendMessageAsync("I choose **Rock**! :punch: It's a tie!");
                }
                if (choice == 2)
                {
                    await Context.Channel.SendMessageAsync("I choose **Paper**! :hand_splayed: **PAPER** wins!");
                }
                if (choice == 3)
                {
                    await Context.Channel.SendMessageAsync("I choose **Scissors**! :hand_splayed: **ROCK** wins!");
                }
                return;
            }
            if (play == "paper")
            {
                if (choice == 1)
                {
                    await Context.Channel.SendMessageAsync("I choose **Rock**! :punch:  **PAPER** wins!");
                }
                if (choice == 2)
                {
                    await Context.Channel.SendMessageAsync("I choose **Paper**! :hand_splayed: It's a tie!");
                }
                if (choice == 3)
                {
                    await Context.Channel.SendMessageAsync("I choose **Scissors**! :hand_splayed: **SCISSORS** wins!");
                }
                return;
            }
            if (play == "scissors")
            {
                if (choice == 1)
                {
                    await Context.Channel.SendMessageAsync("I choose **Rock**! :punch: **ROCK** wins!");
                }
                if (choice == 2)
                {
                    await Context.Channel.SendMessageAsync("I choose **Paper**! :hand_splayed: **SCISSORS** wins!");
                }
                if (choice == 3)
                {
                    await Context.Channel.SendMessageAsync("I choose **Scissors**! :hand_splayed: It's a tie!");
                }
                return;
            }
            else
            {
                await Context.Channel.SendMessageAsync("Your response was invalid, use `n!rps <rock, paper, or scissors>`");
                return;
            }
        }
    }
}
