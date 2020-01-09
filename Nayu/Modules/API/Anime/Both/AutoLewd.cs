using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Modules.API.Anime.NekosLife;
using Nayu.Preconditions;
using Timer = System.Timers.Timer;
using WebRequest = Nayu.Modules.API.Anime.WeebDotSh.Helpers.WebRequest;

namespace Nayu.Modules.API.Anime.Both
{
    public class AutoLewd : NayuModule
    {
        [Command("autolewd")]
        [Summary("Loops images of lewd anime girls :3")]
        [Remarks("Usage: n!autolewd <on/off> Ex: n!autolewd on")]
        [Cooldown(5)]
        public async Task AutoLewdIMG(string arg)
        {
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var botAcc = BotAccounts.GetAccount();
            var result = ConvertBool.ConvertStringToBoolean(arg);
            if (!result.Item1)
            {
                await SendMessage(Context, null, $"Please say `n!autolewd <on/off>`");
                return;
            }
            if (result.Item2)
            {
                await SendMessage(Context, null, $"Started the AutoLewd loop :3");
                guildAcc.AutoLewdStatus = true;
                botAcc.AutoLewdGuilds.Add(guildAcc.Id);
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                BotAccounts.SaveAccounts();
            }
            if (!result.Item2)
            {
                guildAcc.AutoLewdStatus = false;
                botAcc.AutoLewdGuilds.Remove(guildAcc.Id);
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                BotAccounts.SaveAccounts();
                await SendMessage(Context, null, $"Stopped the AutoLewd loop :/");

            }
            
        }

        [Command("autolewdchannel")]
        [Summary("Loops images of lewd anime girls :3")]
        [Remarks("Usage: n!autolewdchannel <channel you want images being sent to> Ex: n!autolewdchannel #nsfw")]
        [Cooldown(5)]
        public async Task LewdIMGChannel(ITextChannel channel)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                string description = $"{Global.ENo} | You Need the Administrator Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
            }

            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            guildAcc.AutoLewdChannel = channel.Id;
            GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
            await ReplyAsync("The AutoLewd-Channel has been set to " + channel.Mention);
        }
    }

    public class AutoLewdTimer
    {
         private static Timer loopingtimer;

        internal Task StartTimer()
        {
            var miliSeconds = 15000;
            loopingtimer = new Timer()
            {
                Interval = miliSeconds,
                AutoReset = true,
                Enabled = true
            };
            loopingtimer.Elapsed += OnTimerTicked;

            Console.WriteLine("Started AutoLewd Loop");
            return Task.CompletedTask;
        }

        public async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            var config = BotAccounts.GetAccount();
            foreach (var guild in config.AutoLewdGuilds)
            {
                var guildAcc = GlobalGuildAccounts.GetGuildAccount(guild);
                string nekolink = NekosLifeHelper.GetNekoLink("lewd");
                string description = "Randomly generated lewd nekos just for you <3!";

                var embed = ImageEmbed.GetImageEmbed(nekolink, Source.NekosLife, description);
                await Program._client.GetGuild(guildAcc.Id).GetTextChannel(guildAcc.AutoLewdChannel)
                    .SendMessageAsync("", embed: embed);
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Successfully sent Autolewd");
        }
    }
}
