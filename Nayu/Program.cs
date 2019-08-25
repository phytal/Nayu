

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
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Addons.Interactive;
using Microsoft.Extensions.DependencyInjection;
using Nayu.Core.LevelingSystem;

namespace Nayu
{
    class Program
    {
        public static DiscordShardedClient _client;
        private IServiceProvider _services;
        private readonly int[] _shardIds = { 0, 1 };

        private static void Main() => new Program().StartAsync().GetAwaiter().GetResult();

        private async Task StartAsync()
        {
            if (string.IsNullOrEmpty(Config.bot.token)) return;
            _client = new DiscordShardedClient(_shardIds, new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                MessageCacheSize = 100,
                TotalShards = 2
            });

            _client.Log += Logger.Log;
            _services = ConfigureServices();
            _services.GetRequiredService<DiscordEventHandler>().InitDiscordEvents();
            await _services.GetRequiredService<CommandHandler>().InitializeAsync();

            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();

            await _client.SetGameAsync(Config.bot.BotGameToSet, $"https://twitch.tv/{Config.bot.TwitchStreamer}", ActivityType.Streaming);
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
                .BuildServiceProvider();
        }
    }
}

