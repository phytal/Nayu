using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Configuration;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Modules.Inbox;

namespace Nayu.Modules.Admin
{
    public class Owner : NayuModule
    {

        public void KillProgram() =>
            Kill(); // DO. NOT. USE. THIS. This is only for deliberately causing a StackOverflowException to stop the program.

        public void Kill() =>
            KillProgram(); // DO. NOT. USE. THIS. This is only for deliberately causing a StackOverflowException to stop the program.
        
        [Subject(OwnerCategories.Owner)]
        [Command("Shutdown")]
        [Summary("Shuts down Nayu :((")]
        [RequireOwner]
        public async Task Shutdown()
        {
            var client = Program._client;

            await client.LogoutAsync();
            await client.StopAsync();
            KillProgram();
        }
        
        [Subject(OwnerCategories.Owner)]
        [Command("Stream")]
        [Summary("Sets what Nayu is streaming")]
        [RequireOwner]
        public async Task SetBotStream(string streamer, [Remainder] string streamName)
        {
            await Program._client.SetGameAsync(streamName, $"https://twitch.tv/{streamer}", ActivityType.Streaming);
            var embed = MiscHelpers.CreateEmbed(Context, "Set Bot Streaming",
                    $"Set the stream name to **{streamName}**, and set the streamer to <https://twitch.tv/{streamer}>!")
                .WithColor(Global.NayuColor);
            await SendMessage(Context, embed.Build());
        }

        [Subject(OwnerCategories.Owner)]
        [Command("Game")]
        [Summary("Sets the game Nayu is playing")]
        [RequireOwner]
        public async Task SetBotGame([Remainder] string game)
        {
            var client = Program._client;

            var embed = new EmbedBuilder();
            embed.WithDescription($"Set the bot's game to {game}");
            embed.WithColor(Global.NayuColor);
            await client.SetGameAsync(game);
            await ReplyAsync("", embed: embed.Build());
        }

        [Subject(OwnerCategories.Owner)]
        [Command("setVersion")]
        [Summary("Set Nayu's version")]
        [RequireOwner]
        public async Task SetBotVersion([Remainder] string version)
        {
            Config.bot.version = version;
            var embed = new EmbedBuilder();
            embed.WithDescription($"Set the bot's version to {version}");
            embed.WithColor(Global.NayuColor);
            await ReplyAsync("", embed: embed.Build());
        }

        [Subject(OwnerCategories.Owner)]
        [Command("setChangeLog"), Alias("scl")]
        [Summary("Set Nayu's changelog")]
        [RequireOwner]
        public async Task SetChangeLog([Remainder] string changeLog)
        {
            var config = BotAccounts.GetAccount();
            config.ChangeLog = changeLog;
            config.LastUpdate = DateTime.Today;
            var embed = EmbedHandler.CreateEmbed(Context, "Update Change Log",
                $"Set the bot's changeLog to ```{changeLog}```", EmbedHandler.EmbedMessageType.Success);
            await SendMessage(Context, embed);
        }
        
        [Subject(OwnerCategories.Owner)]
        [Command("status")]
        [Summary("Sets Nayu's user status")]
        [RequireOwner]
        public async Task SetBotStatus(string status)
        {
            var embed = new EmbedBuilder();
            embed.WithDescription($"Set the status to {status}.");

            embed.WithColor(Global.NayuColor);

            var client = Program._client;

            switch (status)
            {
                case "dnd":
                    await client.SetStatusAsync(UserStatus.DoNotDisturb);
                    break;
                case "idle":
                    await client.SetStatusAsync(UserStatus.Idle);
                    break;
                case "online":
                    await client.SetStatusAsync(UserStatus.Online);
                    break;
                case "offline":
                    await client.SetStatusAsync(UserStatus.Invisible);
                    break;
            }

            await ReplyAsync("", embed: embed.Build());
        }

        [Subject(OwnerCategories.Owner)]
        [Command("LeaveServer")]
        [Summary("Make's Nayu leave the server")]
        [RequireOwner]
        public async Task LeaveServer()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Global.NayuColor);
            await ReplyAsync("", embed: embed.Build());
            await Context.Guild.LeaveAsync();
        }

        [Subject(OwnerCategories.Owner)]
        [Command("ServerCount"), Alias("Sc")]
        [Summary("Sets Nayu's game/stream the number of guilds in")]
        [RequireOwner]
        public async Task ServerCountStream()
        {
            var client = Program._client;
            var guilds = client.Guilds.Count;
            var embed = new EmbedBuilder();
            embed.WithDescription($"Done. In {guilds}");
            embed.WithColor(Global.NayuColor);
            await ReplyAsync("", embed: embed.Build());
            await client.SetGameAsync($"n!help **|** in {guilds} servers!",
                $"https://twitch.tv/{Config.bot.twitchStreamer}", ActivityType.Streaming);

        }

        [Subject(OwnerCategories.Owner)]
        [Command("setAvatar"), Remarks("Sets the bots Avatar")]
        [RequireOwner]
        public async Task SetAvatar(string link)
        {
            try
            {
                var webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(link);

                var stream = new MemoryStream(imageBytes);

                var image = new Image(stream);
                await Context.Client.CurrentUser.ModifyAsync(k => k.Avatar = image);
            }
            catch (Exception)
            {
                var embed = EmbedHandler.CreateEmbed(Context,"Avatar", "Could not set the avatar!",
                    EmbedHandler.EmbedMessageType.Exception);
                await SendMessage(Context, embed);
            }
        }

        [Subject(OwnerCategories.Owner)]
        [Command("uptime")]
        [Alias("runtime")]
        public async Task UpTime()
        {
            var proc = Process.GetCurrentProcess();

            var mem = proc.WorkingSet64 / 1000000;
            var threads = proc.Threads;
            var time = DateTime.Now - proc.StartTime;
            var cpu = proc.TotalProcessorTime.TotalMilliseconds / proc.PrivilegedProcessorTime.TotalMilliseconds;


            var sw = Stopwatch.StartNew();
            sw.Stop();

            var embed = new EmbedBuilder();
            embed.WithColor(Global.NayuColor);
            embed.AddField("Bot Statistics:", $"Your ping: {(int) sw.Elapsed.TotalMilliseconds}ms\n" +
                                              $"Runtime: {time.Hours}h:{time.Minutes}m\n" +
                                              //$"CPU usage: {cpu:n0}\n" +
                                              $"Memory: {mem:n0}Mb\n" +
                                              $"Threads using: {threads.Count}\n");
            //$"Servers in: {Global.Client.Guilds.Count}\n");
            await ReplyAsync("", embed: embed.Build());
        }

        [Subject(OwnerCategories.Owner)]
        [Command("announceToEveryone"), Remarks("Sets the bots Avatar")]
        [RequireOwner]
        public async Task Announce([Remainder]string content)
        {
            var config = GlobalUserAccounts.GetAllAccounts();
            foreach (var userAcc in config)
            {
                SocketUser user = Global.Client.GetUser(userAcc.Id);
                await CreateMessage.CreateAndSendMessageAsync("Nayu Announcement!", content, DateTime.Now, user);
                GlobalUserAccounts.SaveAccounts(userAcc.Id);
            }

            await ReplyAsync($"Sent \n`{content}`\n to everyone");
        }
    }
}