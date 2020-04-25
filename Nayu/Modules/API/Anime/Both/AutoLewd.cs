using System;
using System.ComponentModel;
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
        [Subject(AdminCategories.NSFW)]
        [Command("autoLewd")]
        [Summary("Loops images of lewd anime girls :3")]
        [Remarks("Usage: n!autoLewd <on/off> Ex: n!autoLewd on")]
        [Cooldown(5)]
        public async Task AutoLewdIMG(string arg)
        {
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var botAcc = BotAccounts.GetAccount();
            var result = ConvertBool.ConvertStringToBoolean(arg);
            if (!result.Item1)
            {
                await SendMessage(Context, null, $"Please say `n!autolewd <on/off>`");
            }

            else if (result.Item2)
            {
                await SendMessage(Context, null, $"Started the AutoLewd loop :3");
                guildAcc.AutoLewdStatus = Enabled;
                botAcc.AutoLewdGuilds.Add(guildAcc.Id);
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                BotAccounts.SaveAccounts();
            }

            else if (!result.Item2)
            {
                guildAcc.AutoLewdStatus = Disabled;
                botAcc.AutoLewdGuilds.Remove(guildAcc.Id);
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                BotAccounts.SaveAccounts();
                await SendMessage(Context, null, $"Stopped the AutoLewd loop :/");
            }
        }
        
        [Subject(AdminCategories.NSFW)]
        [Command("autoLewdChannel"), Alias("alc")]
        [Summary("Loops images of lewd anime girls :3")]
        [Remarks("Usage: n!alc <channel you want images being sent to> Ex: n!alc #nsfw")]
        [Cooldown(5)]
        public async Task LewdImgChannel(ITextChannel channel)
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                var description =
                    $"{Global.ENo} **|** You Need the Administrator Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            guildAcc.AutoLewdChannel = channel.Id;
            GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
            await ReplyAsync("The AutoLewd-Channel has been set to " + channel.Mention);
        }
    }

    public class AutoLewdTimer
    {
        private static Timer _loopingTimer;

        internal Task StartTimer()
        {
            const int milliSeconds = 15000;
            _loopingTimer = new Timer()
            {
                Interval = milliSeconds,
                AutoReset = true,
                Enabled = true
            };
            _loopingTimer.Elapsed += OnTimerTicked;

            Console.WriteLine("Started AutoLewd Loop");
            return Task.CompletedTask;
        }

        private async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            var config = BotAccounts.GetAccount();
            foreach (var guild in config.AutoLewdGuilds)
            {
                Embed embed = null;
                var rand = Global.Rng.Next(1, 3);
                if (rand == 1)
                {
                    var nekoLink = NekosLifeHelper.GetNekoLink("lewd");

                    embed = ImageEmbed.GetImageEmbed(nekoLink, Source.NekosLife);
                }

                else if (rand == 2)
                {
                    string[] tags = {""};
                    var webReq = new WebRequest();
                    var result = await webReq.GetTypesAsync("neko", tags, FileType.Any, NsfwSearch.Only, false);
                    var url = result.Url;
                    embed = ImageEmbed.GetImageEmbed(url, Source.WeebDotSh);
                }

                var guildAcc = GlobalGuildAccounts.GetGuildAccount(guild);
                await Program._client.GetGuild(guildAcc.Id).GetTextChannel(guildAcc.AutoLewdChannel)
                    .SendMessageAsync("", embed: embed);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Successfully sent Autolewd");
        }
    }
}