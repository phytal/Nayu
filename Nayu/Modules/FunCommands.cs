using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.IO;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Preconditions;
using Nayu.Helpers;

namespace Nayu.Modules
{
    public class FunCommands : NayuModule
    {
        [Subject(Categories.Fun)]
        [Command("ping")]
        [Summary("Ping Pong!")]
        [Remarks("Ex: n!ping")]
        [Cooldown(5)]
        public async Task Ping()
        {
            var embed = EmbedHandler.CreateEmbed(Context, "Ping",
                $":ping_pong:  **|** Pong! {(Context.Client).Latency}ms", EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        string[] predictionsTexts =
        {
            ":8ball:  **|** It is certain",
            ":8ball:  **|** It is decidedly so",
            ":8ball:  **|** Without a doubt",
            ":8ball:  **|** Yes definitely",
            ":8ball:  **|** You may rely on it",
            ":8ball:  **|** As I see it, yes",
            ":8ball:  **|** Most likely",
            ":8ball:  **|** Outlook good",
            ":8ball:  **|** Yes",
            ":8ball:  **|** Signs point to yes",
            ":8ball:  **|** Reply hazy try again",
            ":8ball:  **|** Ask again later",
            ":8ball:  **|** Better not tell you now",
            ":8ball:  **|** Zzzzz...",
            ":8ball:  **|** Concentrate on my oppai and ask again!",
            ":8ball:  **|** Don't count on it",
            ":8ball:  **|** My reply is no",
            ":8ball:  **|** My sources say no",
            ":8ball:  **|** Outlook not so good",
            ":8ball:  **|** Very doubtful"
        };

        [Subject(Categories.Fun)]
        [Command("8ball")]
        [Alias("eightball")]
        [Summary("Gives a prediction")]
        [Remarks("n!8ball <your prediction> Ex: n!8ball am I loved?")]
        [Cooldown(5)]
        public async Task EightBall([Remainder] string input)
        {
            int randomIndex = Global.Rng.Next(predictionsTexts.Length);
            string text = predictionsTexts[randomIndex];
            var embed = EmbedHandler.CreateEmbed(Context, "8 Ball", $"{text}, {Context.User.Username}",
                EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(Categories.Fun)]
        [Command("echo")]
        [Summary("Make me say a message!")]
        [Remarks("n!echo <what you want the bot to say> Ex: n!echo I like to eat oreos")]
        [Cooldown(5)]
        public async Task Echo([Remainder] string message)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);

            if (config.MassPingChecks)
            {
                if (message.Contains("@everyone") || message.Contains("@here")) return;
            }

            var embed = EmbedHandler.CreateEmbed(Context, "Echo!", message, EmbedHandler.EmbedMessageType.Success);
            await SendMessage(Context, embed);
        }

        [Subject(Categories.Fun)]
        [Command("lmgtfy")]
        [Summary("Sends an Let me Google that for you link with what you inputed")]
        [Remarks("n!lmgtfy <what you want to search up> Ex: n!lmgtfy how to use discord")]
        [Cooldown(5)]
        public async Task Lmgtfy([Remainder] string link = "enter something")
        {
            link = link.Replace(' ', '+');
            var embed = EmbedHandler.CreateEmbed(Context, "", "https://lmgtfy.com/?q=" + link,
                EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(Categories.Fun)]
        [Command("lenny")]
        [Summary("Sends a lenny face ( ͡° ͜ʖ ͡°)")]
        [Remarks("Ex: n!lenny")]
        [Cooldown(5)]
        public async Task Lenny()
        {
            var embed = EmbedHandler.CreateEmbed(Context, "Lenny!", "( ͡° ͜ʖ ͡°)",
                EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(Categories.Information)]
        [Command("prefix")]
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

            var embed = EmbedHandler.CreateEmbed(Context, "Prefix", $"The prefix for this server is {prefix}.",
                EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(Categories.Fun)]
        [Command("ratewaifu")]
        [Summary("Rates your waifu :3")]
        [Remarks("n!ratewaifu <whoever (waifu) that you want to rate> Ex: n!ratewaifu Taiyakiman22")]
        [Cooldown(5)]
        public async Task RateWaifu([Remainder] string input)
        {
            Random rnd = new Random();
            int rating = rnd.Next(101);
            var embed = EmbedHandler.CreateEmbed(Context, "Rate Waifu", $"I'd rate {input} a **{rating} / 100**",
                EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(Categories.Fun)]
        [Command("bigletter")]
        [Alias("emoji", "emotion", "emotify")]
        [Remarks("n!bigletter <whatever you want to 'emotify'> Ex: n!bigletter hello how is your day")]
        [Cooldown(5)]
        public async Task Emotify([Remainder] string args)
        {
            string[] convertorArray = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
            args = args.ToLower();
            var convertedText = "";
            foreach (var c in args)
            {
                if (char.IsLetter(c)) convertedText += $":regional_indicator_{c}:";
                else if (char.IsDigit(c)) convertedText += $":{convertorArray[(int) char.GetNumericValue(c)]}:";
                else if (c == '.') convertedText += " ⏺ ";
                else if (c == '?') convertedText += "❓ ";
                else if (c == '!') convertedText += "❗ ";
                else convertedText += c;
                if (char.IsWhiteSpace(c)) convertedText += "  ";
            }

            await ReplyAsync(convertedText);
        }

        [Subject(Categories.Fun)]
        [Command("woop")]
        [Summary("Woop! <o/")]
        [Remarks("Ex: n!coop")]
        [Cooldown(5)]
        public async Task Woop()
        {
            await Context.Channel.SendFileAsync(@Path.Combine(Constants.ResourceFolder, Constants.MemeFolder,
                "woop.gif"));
        }

        [Subject(Categories.Fun)]
        [Command("rps")]
        [Summary("Rock, Paper Scissors!")]
        [Remarks("n!rps <rock/paper/scissors> Ex: n!rps rock")]
        [Cooldown(5)]
        public async Task Rps([Remainder] string play)
        {
            play = play.ToLower();
            int choice = Global.Rng.Next(4); //1= rock 2= paper 3= scissors
            if (play == "rock")
            {
                if (choice == 1)
                {
                    await SendMessage(Context, null, "I choose **Rock**! :punch: It's a tie!");
                }

                if (choice == 2)
                {
                    await SendMessage(Context, null, "I choose **Paper**! 🖐️**PAPER** wins!");
                }

                if (choice == 3)
                {
                    await SendMessage(Context, null, "I choose **Scissors**! 🖐️**ROCK** wins!");
                }
            }
            else if (play == "paper")
            {
                if (choice == 1)
                {
                    await SendMessage(Context, null, "I choose **Rock**! :punch:  **PAPER** wins!");
                }

                if (choice == 2)
                {
                    await SendMessage(Context, null, "I choose **Paper**! 🖐️It's a tie!");
                }

                if (choice == 3)
                {
                    await SendMessage(Context, null, "I choose **Scissors**! 🖐️**SCISSORS** wins!");
                }
            }
            else if (play == "scissors")
            {
                if (choice == 1)
                {
                    await SendMessage(Context, null, "I choose **Rock**! :punch: **ROCK** wins!");
                }

                if (choice == 2)
                {
                    await SendMessage(Context, null, "I choose **Paper**! 🖐️**SCISSORS** wins!");
                }

                if (choice == 3)
                {
                    await SendMessage(Context, null, "I choose **Scissors**! 🖐️It's a tie!");
                }
            }
            else
            {
                await SendMessage(Context, null, "Your response was invalid, use `n!rps <rock, paper, or scissors>`");
            }
        }
    }
}