using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Nayu.Core.LevelingSystem;
using Nayu.Features.GlobalAccounts;
using Discord;
using Nayu.Modules;
using Weeb.net;

namespace Nayu
{
    public class CommandHandler
    {
        private DiscordShardedClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        public CommandHandler(IServiceProvider services, CommandService commands,
    DiscordShardedClient client)
        {
            _commands = commands;
            _services = services;
            _client = client;

        }
        WeebClient weebClient = new WeebClient("Nayu", Config.bot.Version);

        public async Task InitializeAsync()
        {
            Global.Client = _client;

            _commands = new CommandService();
            var cmdConfig = new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async
            };
            await _commands.AddModulesAsync(
                Assembly.GetEntryAssembly(),
                _services);

            //Will print current weeb.sh API version and Weeb.net wrapper version
            await weebClient.Authenticate(Config.bot.wolkeToken, Weeb.net.TokenType.Wolke);

        }


        public static WeebClient _weebClient;

        public async Task HandleCommandAsync(SocketMessage s)
        {
            _ = Modules.Management.SlowMode.Slowmode(s);

            if (!(s is SocketUserMessage msg)) return;
            if (msg.Channel is SocketDMChannel) return;

            var context = new ShardedCommandContext(_client, msg);
            if (context.User.IsBot) return;

            var config = GlobalGuildAccounts.GetGuildAccount(context.Guild.Id);
            var prefix = config.CommandPrefix ?? Config.bot.cmdPrefix;

            var argPos = 0;
            if (msg.HasStringPrefix(prefix, ref argPos) && (context.Guild == null || context.Guild.Id != 264445053596991498 || context.Guild.Id != 396440418507816960) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos) && (context.Guild == null || context.Guild.Id != 264445053596991498 || context.Guild.Id != 396440418507816960))
            {
                foreach (var command in config.CustomCommands)
                    if (msg.HasStringPrefix($"{config.CommandPrefix}{command.Key}", ref argPos))
                    {
                        await context.Channel.SendMessageAsync(command.Value);
                    }

                var cmdSearchResult = _commands.Search(context, argPos);
                if (cmdSearchResult.Commands.Count == 0) await context.Channel.SendMessageAsync($"{context.User.Mention}, that is not a valid command");

                var executionTask = _commands.ExecuteAsync(context, argPos, _services);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                executionTask.ContinueWith(task =>
                {
                    if (task.Result.IsSuccess || task.Result.Error == CommandError.UnknownCommand) return;
                    const string errTemplate = "{0}, Error: {1}";
                    var errMessage = string.Format(errTemplate, context.User.Mention, task.Result.ErrorReason);
                    var embed = new EmbedBuilder()
                    .WithColor(37, 152, 255)
                    .WithTitle("Error!")
                    .WithDescription(errMessage)
                    .WithFooter("Did you use the command correctly? If so, please report this to our discord server https://discord.gg/NuUdx4h");
                    context.Channel.SendMessageAsync("", embed: embed.Build());
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            }
        }

        public async Task _UserJoined(SocketGuildUser user)
        {
            _ = UserJoined(user);
        }
        private async Task UserJoined(SocketGuildUser user)
        {
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(user.Guild.Id);
            if (guildAcc.WelcomeChannel == 0) return;
            if (!(_client.GetChannel(guildAcc.WelcomeChannel) is SocketTextChannel channel)) return;
            var possibleMessages = guildAcc.WelcomeMessages;
            var messageString = possibleMessages[Global.Rng.Next(possibleMessages.Count)];
            messageString = messageString.ReplacePlacehoderStrings(user);
            if (string.IsNullOrEmpty(messageString)) return;
            await channel.SendMessageAsync(messageString);
        }

        public async Task _UserLeft(SocketGuildUser user)
        {
            _ = UserLeft(user);
        }

        private async Task UserLeft(SocketGuildUser user)
        {
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(user.Guild.Id);
            if (guildAcc.LeaveChannel == 0) return;
            if (!(_client.GetChannel(guildAcc.LeaveChannel) is SocketTextChannel channel)) return;
            var possibleMessages = guildAcc.LeaveMessages;
            var messageString = possibleMessages[Global.Rng.Next(possibleMessages.Count)];
            messageString = messageString.ReplacePlacehoderStrings(user);
            if (string.IsNullOrEmpty(messageString)) return;
            await channel.SendMessageAsync(messageString);
        }


        public async Task OnReactionAddedDuelRequest(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (!reaction.User.Value.IsBot)
            {
                var context = new ShardedCommandContext(Global.Client, (SocketUserMessage)reaction.Message);
                if (!PendingDuelProvider.UserIsPlaying(reaction.UserId)) return;
                var emote = Emote.Parse("<:no:453716729525174273>");
                var user = (SocketGuildUser)reaction.User;
                var req = PendingDuelProvider.RequestUser(reaction.UserId);
                var requester = Global.Client.GetUser(req);
                if (reaction.Emote.Name == emote.Name)
                {
                    var game = PendingDuelProvider.games.FirstOrDefault(g => g.PlayerId == reaction.UserId);
                    PendingDuelProvider.games.Remove(game);
                    await context.Message.DeleteAsync();
                    await channel.SendMessageAsync($"**{user.Username}** has declined **{requester.Username}**'s duel request!");
                }
                if (reaction.Emote.Name == "✅")
                {
                    var game = PendingDuelProvider.games.FirstOrDefault(g => g.PlayerId == reaction.UserId);
                    PendingDuelProvider.games.Remove(game);
                    await context.Message.DeleteAsync();
                    await channel.SendMessageAsync($"**{user.Username}** has accepted **{requester.Username}**'s duel request!");
                    await Duel.StartDuel(channel, user, requester);
                }
                else
                {
                    return;
                }
            }
        }
    }
}
