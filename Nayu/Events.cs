using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nayu.Core.Configuration;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.LevelingSystem;
using Nayu.Modules;

namespace Nayu
{
    public class Events : NayuModule
    {
        private static readonly DiscordShardedClient _client = Program._client;

        public async Task Autorole(SocketGuildUser user)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(user.Guild.Id);
            if (config.Autorole != null || config.Autorole != "")
            {
                var targetRole = user.Guild.Roles.FirstOrDefault(r => r.Name == config.Autorole);
                await user.AddRoleAsync(targetRole);
            }
        }

        public async Task GuildUtils(SocketGuild s)
        {
            var config = GlobalGuildAccounts.GetGuildAccount(s.Id);

            var dmChannel = await s.Owner.GetOrCreateDMChannelAsync();
            var embed = new EmbedBuilder();
            embed.WithTitle($"Thanks for adding me to your server, {s.Owner.Username}!");
            embed.WithDescription("For quick information, use the `n!help` command! \nNeed quick help? Visit the my support server! https://discord.gg/z8TgwT!");
            embed.WithThumbnailUrl(s.IconUrl);
            embed.WithFooter("Found an issue in a command? Report it in the server linked above!");
            embed.WithColor(Global.NayuColor);

            config.GuildOwnerId = s.Owner.Id;

            await dmChannel.SendMessageAsync("", embed: embed.Build());
            GlobalGuildAccounts.SaveAccounts(s.Id);

            var client = Program._client;
            var guilds = client.Guilds.Count;
            var sclient = client.Shards;
            //var shard = sclient.
            await client.SetGameAsync($"n!help | in {guilds} servers!", $"https://twitch.tv/{Config.bot.twitchStreamer}", ActivityType.Streaming);
        }

        public async Task<string> UpdateServerCount(DiscordShardedClient client)
        {
            var webclient = new HttpClient();
            var content = new StringContent($"{{ \"server_count\": {client.Guilds.Count} }}", Encoding.UTF8, "application/json");
            webclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Config.bot.dblToken);
            var resp = await webclient.PostAsync($"https://discordbots.org/api/bots/417160957010116608/stats", content);
            return resp.Content.ReadAsStringAsync().ToString();
        }

        public async Task FilterChecks(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            var context = new ShardedCommandContext(_client, msg);
            var config = GlobalGuildAccounts.GetGuildAccount(context.Guild.Id);
            if (context.User.IsBot) return;
            if (msg == null) return;
            if (msg.Author.Id == config.GuildOwnerId) return;

            try
            {
                if (config.Antilink)
                {
                    if ((msg.Content.Contains("https://discord.gg") || msg.Content.Contains("https://discord.io")) && !config.AntilinkIgnoredChannels.Contains(context.Channel.Id))
                    {
                        await msg.DeleteAsync();
                        var embed = new EmbedBuilder();
                        embed.WithColor(Global.NayuColor);
                        embed.WithDescription($":warning:  | {context.User.Mention}, Don't post your filthy links here! (No links)");
                        await ReplyAndDeleteAsync("", embed: embed.Build());
                    }
                }
            }
            catch (NullReferenceException)
            {
                return;
            }

            string[] reactionTexts = new string[]
{
        "This is a Christian Minecraft Server!",
        "Watch your language buddy!",
        "I think you touched the stove on accident!",
        "You're starting to bug me..",
        "You're under-arrest by the Good Boy Cops",
        "Woah man, too far",
        "Do I really have to tape your mouth shut?",
        "Ok buddy you might get yourself into a problem..",
        "Now I know why you have no friends",
        "If you like to eat soap I have some right here..",
        "Honestly I can't stand you"
};
            Random rand = new Random();
            List<string> bannedWords = new List<string>
                {
                     "fuck", "fuk", "bitch", "pussy", "nigg","asshole", "c0ck", "cock", "dick", "cunt", "cnut", "d1ck", "blowjob", "b1tch"
                };
            try
            {
                if (config.Filter)
                {
                    if (bannedWords.Any(msg.Content.ToLower().Contains) 
                        || config.CustomFilter.Any(msg.Content.ToLower().Contains) && !config.NoFilterChannels.Contains(context.Channel.Id))
                    {
                        int randomIndex = rand.Next(reactionTexts.Length);
                        string text = reactionTexts[randomIndex];
                        await msg.DeleteAsync();
                        var embed = new EmbedBuilder();
                        embed.WithDescription($":warning:  |  {text} (Inappropriate language)");
                        embed.WithColor(Global.NayuColor);
                        await ReplyAndDeleteAsync("", embed: embed.Build());
                    }
                }
            }
            catch (NullReferenceException)
            {
                return;
            }

            if (config.MassPingChecks)
            {
                if (msg.Content.Contains("@everyone") || msg.Content.Contains("@here"))
                {
                    await msg.DeleteAsync();
                    await ReplyAndDeleteAsync($":warning:  | {msg.Author.Mention}, try not to mass ping.");
                }
            }
        }

        public async Task Unflip(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            var context = new ShardedCommandContext(_client, msg);
            var config = GlobalGuildAccounts.GetGuildAccount(context.Guild.Id);
            if (context.User.IsBot) return;
            if (msg == null) return;

            if (config.Unflip)
            {
                if (msg.Content.Contains("(╯°□°）╯︵ ┻━┻"))
                {
                    await SendMessage(Context, null, "┬─┬ ノ( ゜-゜ノ)");
                }
            }
        }
    }
}
