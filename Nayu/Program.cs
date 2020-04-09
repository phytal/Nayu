/////////////////////////////////////////
//    _   _                            //  
//   | \ | |                           //
//   |  \| |   __ _   _   _   _   _    // 
//   | . ` |  / _` | | | | | | | | |   //
//   | |\  | | (_| | | |_| | | |_| |   //
//   |_| \_|  \__,_|  \__, |  \__,_|   //
//                     __/ |           //
//                    |___/            //
//                                     //
/////////////////////////////////////////


using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nayu.Core.Configuration;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Core.LevelingSystem;
using Nayu.Libs.CustomLibraries.Discord.Addons.Interactive;
using Nayu.Modules.API.Anime.Both;
using Nayu.Modules.Chomusuke;
using Nayu.Modules.Music;
using Sentry;
using Victoria;

namespace Nayu
{
    internal class Program
    {
        public static DiscordShardedClient _client;
        private IServiceProvider _services;
        private readonly int[] _shardIds = {0};

        private static void Main()
        {
            SentrySdk.Init(Config.bot.sentryLink);
            // All required tasks that need to run simultaneously
            var botLaunchers = new List<Task>
            {
                Task.Run(() => { LaunchLavalink(); }), // Lavalink launcher
                Task.Run(() => { LaunchBotSetup(); }) // Bot launcher
            };

            // Run all required tasks
            Task.WaitAll(botLaunchers.ToArray());
        }

        private async Task StartAsync()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n _|_|_|      _|_|_|  _|    _|  _|    _|\n" +
                              " _|    _|  _|    _|  _|    _|  _|    _|\n" +
                              " _|    _|  _|    _|  _|    _|  _|    _|\n" +
                              " _|    _|    _|_|_|    _|_|_|    _|_|_|\n" +
                              "                           _|         \n" +
                              "                         _|_|   \n");
            if (string.IsNullOrEmpty(Config.bot.token)) return;
            _client = new DiscordShardedClient(_shardIds, new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                MessageCacheSize = 100,
                TotalShards = 1
            });
            _client.Log += Logger.Log;
            _services = ConfigureServices();
            _services.GetRequiredService<DiscordEventHandler>().InitDiscordEvents();
            await _services.GetRequiredService<CommandHandler>().InitializeAsync();

            try
            {
                await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            }
            catch (Exception)
            {
                return;
            }

            await _client.StartAsync();

            await _client.SetGameAsync(Config.bot.botGameToSet, $"https://twitch.tv/{Config.bot.twitchStreamer}",
                ActivityType.Streaming);
            await _client.SetStatusAsync(UserStatus.Online);

            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton<CommandHandler>()
                .AddSingleton<DiscordEventHandler>()
                .AddSingleton<CommandService>()
                .AddSingleton<InteractiveService>()
                .AddSingleton<ChomusukeTimer>()
                .AddSingleton<AutoLewdTimer>()
                .AddSingleton<Events>()
                .AddSingleton<Leveling>()
                .AddSingleton<LavaConfig>()
                .AddSingleton<LavaNode>()
                .AddSingleton<MusicManager>()
                .AddSingleton<Logger>()
                .BuildServiceProvider();
        }

        private static void LaunchBotSetup()
        {
            new Program().StartAsync().GetAwaiter().GetResult();
        }

        private static void LaunchLavalink()
        {
            var psi = new ProcessStartInfo();
            var directory = string.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                directory =
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())))) +
                    @"/Lavalink";
                psi = new ProcessStartInfo("/bin/bash", $"-c {directory}/lava.sh");
                //var psi = new ProcessStartInfo("/bin/bash", $"java -jar {directory.Substring(1)}/Lavalink.jar");
            }

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                directory =
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())))) +
                    @"\Lavalink";
                Console.WriteLine(directory);
                psi = new ProcessStartInfo("cmd.exe", $"/c java -jar {directory}" + @"\Lavalink.jar");
            }

            psi.WorkingDirectory = directory;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            Process.Start(psi);
        }
    }
}