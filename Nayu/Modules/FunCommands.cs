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

            var embed = EmbedHandler.CreateEmbed(Context, "Ping", $":ping_pong:  | Pong! {(Context.Client).Latency}ms", EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        string[] predictionsTexts = 
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

        [Command("8ball")]
        [Alias("eightball")]
        [Summary("Gives a prediction")]
        [Remarks("n!8ball <your prediction> Ex: n!8ball am I loved?")]
        [Cooldown(5)]
        public async Task EightBall([Remainder] string input)
        {
            int randomIndex = Global.Rng.Next(predictionsTexts.Length);
            string text = predictionsTexts[randomIndex];
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle(text + ", " + Context.User.Username);
            await SendMessage(Context, embed.Build());
        }


        [Command("echo")]
        [Summary("Make me say a message!")]
        [Remarks("n!echo <what you want the bot to say> Ex: n!echo I like to eat oreos")]
        [Cooldown(5)]
        public async Task Echo([Remainder] string message)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var localTime = DateTime.Now;
            var Sender = Context.Message.Author;

            var embed = new EmbedBuilder();
            embed.WithTitle(message);
            embed.WithFooter(localTime + " Message from " + Sender);
            embed.WithColor(37, 152, 255);

            await SendMessage(Context, embed.Build());
            if (config.MassPingChecks)
            {
                if (message.Contains("@everyone") || message.Contains("@here")) return;
            }
            await Context.Message.DeleteAsync();
        }


        [Command("lmgtfy")]
        [Summary("Sends an Let me Google that for you link with what you inputed")]
        [Remarks("n!lmgtfy <what you want to search up> Ex: n!lmgtfy how to use discord")]
        [Cooldown(10)]
        public async Task Lmgtfy([Remainder]string link = "enter something")
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
            await SendMessage(Context, null, "( ͡° ͜ʖ ͡°)");
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

            await SendMessage(Context, null, $"The prefix for this server is {prefix}.");
        }

        [Command("ratewaifu")]
        [Summary("Rates your waifu :3")]
        [Remarks("n!ratewaifu <whoever (waifu) that you want to rate> Ex: n!ratewaifu Taiyakiman22")]
        [Cooldown(5)]
        public async Task RateWaifu([Remainder]string input)
        {
            Random rnd = new Random();
            int rating = rnd.Next(101);
            await SendMessage(Context, null, $"I'd rate {input} a **{rating} / 100**");
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
                return;
            }
            if (play == "paper")
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
                return;
            }
            if (play == "scissors")
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
                return;
            }
            else
            {
                await SendMessage(Context, null, "Your response was invalid, use `n!rps <rock, paper, or scissors>`");
                return;
            }
        }
    }
}
