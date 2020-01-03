

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
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nayu.Core.Configuration;
using Nayu.Core.Handlers;
using Nayu.Core.LevelingSystem;
using Nayu.Libs.CustomLibraries.Discord.Addons.Interactive;
using Nayu.Modules.Chomusuke;
using Nayu.Modules.Music;
using Victoria;
      
      namespace Nayu
{
    class Program
    {
        public static DiscordShardedClient _client;
        private IServiceProvider _services;
        private readonly int[] _shardIds = { 0 };

        private static void Main()
        {
            // All required tasks that need to run simultaneously
            var botLaunchers = new List<Task>
            {
                //Task.Run(() => { LaunchLavalink(); }), // Lavalink launcher
                Task.Run(() => { LaunchBotSetup(); })  // Bot launcher
            };

            // Run all required tasks
            Task.WaitAll(botLaunchers.ToArray());
        }

        private async Task StartAsync()
        {
            //if (string.IsNullOrEmpty(Config.bot.token)) return;
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
                await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("Token"));
            }
            catch (Exception e)
            {
                return;
            }

            await _client.StartAsync();

            await _client.SetGameAsync(Config.bot.botGameToSet, $"https://twitch.tv/{Config.bot.twitchStreamer}", ActivityType.Streaming);
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
                .AddSingleton<Events>()
                .AddSingleton<Leveling>()
                .AddSingleton<LavaConfig>()
                .AddSingleton<LavaNode>()
                .AddSingleton<MusicManager>()
                .AddSingleton<Logger>()
                .BuildServiceProvider();
        }
        
        private static void LaunchBotSetup()
            => new Program().StartAsync().GetAwaiter().GetResult();
        
        private static void LaunchLavalink()
        {
            var directory = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())))) + @"\Lavalink";
            var psi = new ProcessStartInfo("cmd.exe", $"/c java -jar {directory}" + @"\Lavalink.jar");
            psi.WorkingDirectory = directory;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            Process.Start(psi);
        }
    }
}

