using Discord;
using Discord.Commands;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;
using System;
using System.Threading.Tasks;

namespace Nayu.Modules.Chomusuke
{
    public class SetActive : NayuModule
    {
        [Command("active")]
        [Summary("Replace your active chomusuke with another one")]
        [Remarks("Usage: n!active Ex: n!active")]
        public async Task ReplaceBlessing()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (config.Chomusuke1.Have == false)
            {
                await Context.Channel.SendMessageAsync("You don't have a chomusuke! You can buy one with `n!shop`!");
                return;
            }

            if (config.Chomusuke1.Have && config.Chomusuke2.Have == false && config.Chomusuke3.Have == false)
            {
                config.ActiveChomusuke = 1;
                GlobalUserAccounts.SaveAccounts();
                await Context.Channel.SendMessageAsync(
                    $":white_check_mark:  | Successfully made your only Chomusuke your active Chomusuke.");
                return;
            }

            var embed = new EmbedBuilder();
            embed.WithTitle("Set active Chomusuke");
            embed.WithDescription(
                "Type the number correlating to the chomusukes shown below that you want to set to your active Chomusuke.");
            embed.AddField("1",
                $"Name: **{config.Chomusuke1.Name}**\nZodiac: **{config.Chomusuke1.Zodiac}**\nType: **{config.Chomusuke1.Name}**\nLevel: **{config.Chomusuke1.LevelNumber}**\nCombat Power: **{config.Chomusuke1.CP}**\nHealth: **{config.Chomusuke1.HealthCapacity}**\nShield: **{config.Chomusuke1.ShieldCapacity}**\nControl: **{config.Chomusuke1.Control}**\nTrait 1: **{config.Chomusuke1.Trait1}**\nTrait2: **{config.Chomusuke1.Trait2}**");
            if (config.Chomusuke2.Have)
                embed.AddField("2",
                    $"Name: **{config.Chomusuke2.Name}**\nZodiac: **{config.Chomusuke2.Zodiac}**\nType: **{config.Chomusuke2.Name}**\nLevel: **{config.Chomusuke2.LevelNumber}**\nCombat Power: **{config.Chomusuke2.CP}**\nHealth: **{config.Chomusuke2.HealthCapacity}**\nShield: **{config.Chomusuke2.ShieldCapacity}**\nControl: **{config.Chomusuke2.Control}**\nTrait 1: **{config.Chomusuke2.Trait1}**\nTrait2: **{config.Chomusuke2.Trait2}**");
            if (config.Chomusuke3.Have)
                embed.AddField("3",
                    $"Name: **{config.Chomusuke3.Name}**\nZodiac: **{config.Chomusuke3.Zodiac}**\nType: **{config.Chomusuke3.Name}**\nLevel: **{config.Chomusuke3.LevelNumber}**\nCombat Power: **{config.Chomusuke3.CP}**\nHealth: **{config.Chomusuke3.HealthCapacity}**\nShield: **{config.Chomusuke3.ShieldCapacity}**\nControl: **{config.Chomusuke3.Control}**\nTrait 1: **{config.Chomusuke3.Trait1}**\nTrait2: **{config.Chomusuke3.Trait2}**");
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
            var response = await NextMessageAsync();
            if (response.Content.Equals("1") && response.Author.Equals(Context.User) && config.Chomusuke1.Have) config.ActiveChomusuke = 1;

            else if (response.Content.Equals("2") && response.Author.Equals(Context.User) && config.Chomusuke2.Have) config.ActiveChomusuke = 2;

            else if (response.Content.Equals("3") && response.Author.Equals(Context.User) && config.Chomusuke3.Have)
                config.ActiveChomusuke = 3;
            else
                throw new ArgumentException(
                    $"You don't have that Chomusuke!\n*Make sure you typed the number correctly!*");


            GlobalUserAccounts.SaveAccounts();
            await Context.Channel.SendMessageAsync(
                $":white_check_mark:  | Successfully made Chomusuke #{response} your active Chomusuke.");
        }
    }
}

