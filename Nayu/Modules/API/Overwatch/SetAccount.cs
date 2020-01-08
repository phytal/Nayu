using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Preconditions;

namespace Nayu.Modules.API.Overwatch
{
    public class SetAccount : NayuModule
    {
        [Command("owaccount")]
        [Summary("Set your Overwatch username, platform and region")]
        [Remarks("n!owaccount <username> <platform> <region> Ex: n!owaccount Username#1234 pc us")]
        [Cooldown(10)]
        public async Task OwAccount(string user, string platform, string region)
        {
            user = user.Replace('#', '-');
            var config = GlobalUserAccounts.GetUserAccount(Context.User);

            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle("Overwatch Credentials");
            embed.AddField("Username", user);
            embed.AddField("Platform", platform);
            embed.AddField("Region", region);
            embed.WithDescription($"Successfully set your default Battle.net credentials.");

            config.OverwatchID = user;
            config.OverwatchPlatform = platform;
            config.OverwatchRegion = region;
            GlobalUserAccounts.SaveAccounts(config.Id);


            await SendMessage(Context, embed.Build());
        }

        [Command("owaccount")]
        [Summary("View your Overwatch information")]
        [Remarks("n!owaccountinfo")]
        [Cooldown(10)]
        public async Task GetOwAccount()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (config.OverwatchPlatform == null && config.OverwatchRegion == null && config.OverwatchID == null)
            {
                await SendMessage(Context, null, 
                    "**Make sure you set your account information first!**\n n!owaccount <username> <platform> <region> Ex: n!owaccount Username#1234 pc us ");
                return;
            }

            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle("Here are your Overwatch credentials");
            embed.AddField("Username", config.OverwatchID);
            embed.AddField("Region", config.OverwatchRegion);
            embed.AddField("Platform", config.OverwatchPlatform);

            await SendMessage(Context, embed.Build());
        }
    }
}
