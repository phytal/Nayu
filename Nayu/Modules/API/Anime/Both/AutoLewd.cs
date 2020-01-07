using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Helpers;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;
using Newtonsoft.Json;

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
            var result = ConvertBool.ConvertStringToBoolean(arg);
            if (result.Item1 == false)
            {
                await SendMessage(Context, null, $"Please say `n!autolewd <on/off>`");
                return;
            }
            if (result.Item2)
            {
                await SendMessage(Context, null, $"Started the AutoLewd loop :3");
                await LewdLoop(Context.Message);
                guildAcc.AutoLewdStatus = true;
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
            }
            if (!result.Item2)
            {
                guildAcc.AutoLewdStatus = false;
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
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
            if (guildUser.GuildPermissions.Administrator)
            {
                var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
                guildAcc.AutoLewdChannel = channel.Id;
                GlobalGuildAccounts.SaveAccounts(Context.Guild.Id);
                await ReplyAsync("The AutoLewd-Channel has been set to " + channel.Mention);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        private static readonly DiscordShardedClient _client = Program._client;
        public static async Task LewdLoop(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            var context = new ShardedCommandContext(_client, msg);
            var config = GlobalGuildAccounts.GetGuildAccount(context.Guild.Id);
            while (true)
            {
                var embed = new EmbedBuilder();
                int rand = Global.Rng.Next(1, 3);
                if (rand == 1)
                {
                    string json = "";
                    using (WebClient client = new WebClient())
                    {
                        json = client.DownloadString("https://nekos.life/api/v2/img/lewd");
                    }

                    var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                    string nekolink = dataObject.url.ToString();
                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Randomly generated lewd neko just for you <3!");
                    embed.WithImageUrl(nekolink);
                    embed.WithFooter($"Powered by nekos.life");
                }

                if (rand == 2)
                {
                    string[] tags = new[] {""};
                    weebDotSh.Helpers.WebRequest webReq = new weebDotSh.Helpers.WebRequest();
                    RandomData result = await webReq.GetTypesAsync("neko", tags, FileType.Any, NsfwSearch.Only, false);
                    string url = result.Url;
                    string id = result.Id;

                    embed.WithColor(37, 152, 255);
                    embed.WithTitle("Lewd!");
                    embed.WithDescription(
                        $"{context.User.Mention} here's some lewd anime girls at your disposal :3");
                    embed.WithImageUrl(url);
                    embed.WithFooter($"Powered by weeb.sh | ID: {id}");
                }

                if (config.AutoLewdStatus == false)
                {
                    break;
                }
                Thread.Sleep(5000);
                await _client.GetGuild(config.Id).GetTextChannel(config.AutoLewdChannel)
                    .SendMessageAsync("", false, embed.Build());
            }
        }

    }
}
