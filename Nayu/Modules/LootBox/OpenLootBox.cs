using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Xml;
using Discord.Commands;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;

namespace Nayu.Modules.LootBox
{
    public class OpenLootBox : NayuModule
    {
        public static async Task OpenCommonBox(SocketUser user, ITextChannel channel)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            var configg = GlobalUserAccounts.GetUserAccount(user);

            var embed = new EmbedBuilder()
                .WithColor(176, 176, 160)
                .WithImageUrl("https://i.imgur.com/fH8wya5.png");
            embed.Title = "Common Lootbox";
            embed.Description = $"**{user.Username}** opened a **COMMON** Lootbox!";
            config.LootBoxCommon -= 1;
            uint taiyakies = (uint) Global.Rng.Next(50, 100);
            config.Taiyaki += taiyakies;
            embed.AddField("Taiyakis", taiyakies);

            configg.NormalCapsule += 1;
            embed.AddField("Chomusuke Item", "Normal Chomusuke Capsule (Open it with `n!openCapsule`!)");
            GlobalUserAccounts.SaveAccounts();
            await channel.SendMessageAsync("", embed: embed.Build());
        }

        public static async Task OpenUncommonBox(SocketUser user, ITextChannel channel)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);

            var embed = new EmbedBuilder()
                .WithColor(26, 252, 10)
                .WithImageUrl("https://i.imgur.com/MHpJetn.png");
            embed.Title = "Uncommon Lootbox";
            embed.Description = $"**{user.Username}** opened an **UNCOMMON** Lootbox!";
            config.LootBoxUncommon -= 1;
            ulong taiyakies = (ulong) Global.Rng.Next(150, 301);
            config.Taiyaki += taiyakies;
            embed.AddField("Taiyakis", taiyakies);

            byte chomusukeBool = (byte) Global.Rng.Next(1, 4);
            if (chomusukeBool == 1 || chomusukeBool == 2)
            {
                config.NormalCapsule += 1;
                embed.AddField("Chomusuke Item", "Normal Chomusuke Capsule (Open it with `n!openCapsule`!)");
            }
            
            int duelBool = Global.Rng.Next(1, 4);
            if (duelBool == 1 || duelBool == 2)
            {
                int duelValue = Global.Rng.Next(29, 57);
                string item = GetDuelItem(user, duelValue);
                embed.AddField("Duels Item", $"{item} (x1)");
            }
            GlobalUserAccounts.SaveAccounts();
            
            await channel.SendMessageAsync("", embed: embed.Build());
        }


        public static async Task OpenRareBox(SocketUser user, ITextChannel channel)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);

            var embed = new EmbedBuilder()
                .WithColor(10, 50, 252)
                .WithImageUrl("https://i.imgur.com/cpK5Aa8.png");
            embed.Title = "Rare Lootbox";
            embed.Description = $"**{user.Username}** opened a **RARE** Lootbox!";
            config.LootBoxRare -= 1;
            int taiyakies = Global.Rng.Next(320, 500);
            config.Taiyaki += (ulong) taiyakies;
            embed.AddField("Taiyakis", taiyakies);

            config.NormalCapsule += 1;
            embed.AddField("Chomusuke Item", "Normal Chomusuke Capsule (Open it with `n!openCapsule`!)");

            int duelBool = Global.Rng.Next(1, 4);
            if (duelBool == 1 || duelBool == 2)
            {
                int duelValue = Global.Rng.Next(26, 57);
                string item = GetDuelItem(user, duelValue);
                embed.AddField("Duels Item", $"{item} (x1)");
            }

            GlobalUserAccounts.SaveAccounts();
            await channel.SendMessageAsync("", embed: embed.Build());
        }

        public static async Task OpenEpicBox(SocketUser user, ITextChannel channel)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);

            var embed = new EmbedBuilder()
                .WithColor(131, 10, 252)
                .WithImageUrl("https://i.imgur.com/h3F7zeF.png");
            embed.Title = "Epic Lootbox";
            embed.Description = $"**{user.Username}** opened an **EPIC** Lootbox!";
            config.LootBoxEpic -= 1;
            int taiyakies = Global.Rng.Next(550, 750);
            config.Taiyaki += (ulong)taiyakies;
            embed.AddField("Taiyakis", taiyakies);

            int chomusukeBool = Global.Rng.Next(1, 4);
            if (chomusukeBool == 1)
            {
                config.ShinyCapsule += 1;
                    embed.AddField("Chomusuke Item", "Shiny Chomusuke Capsule (Open it with `n!openCapsule`!)");
                }
            else if (chomusukeBool == 2)
            {
                config.NormalCapsule += 1;
                embed.AddField("Chomusuke Item", "Normal Chomusuke Capsule (Open it with `n!openCapsule`!)");
            }
            else if (chomusukeBool == 3)
            {
                config.MythicalCapsule += 1;
                embed.AddField("Chomusuke Item", "Mythical Chomusuke Capsule (Open it with `n!openCapsule`!)");
            }


            int duelBool = Global.Rng.Next(1, 3);
            if (duelBool == 1 || duelBool == 2)
            {
                int duelValue = Global.Rng.Next(44, 57);
                string item = GetDuelItem(user, duelValue);
                embed.AddField("Duels Item", $"{item} (x1)");
            }
            GlobalUserAccounts.SaveAccounts();
            await channel.SendMessageAsync("", embed: embed.Build());
        }

        public static async Task OpenLegendaryBox(SocketUser user, ITextChannel channel)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            var configg = GlobalUserAccounts.GetUserAccount(user);

            var embed = new EmbedBuilder()
                .WithColor(252, 252, 10)
                .WithImageUrl("https://i.imgur.com/qi4sqoH.png");
            embed.Title = "Legendary Lootbox";
            embed.Description = $"**{user.Username}** opened a **LEGENDARY** Lootbox!";
            config.LootBoxLegendary -=  1;
            int taiyakies = Global.Rng.Next(800, 1200);
            config.Taiyaki += (ulong)taiyakies;
            embed.AddField("Taiyakis", taiyakies);

            int chomusukeBool = Global.Rng.Next(1, 3);
            if (chomusukeBool == 1 || chomusukeBool == 2)
            {
                configg.MythicalCapsule += 1;
                    embed.AddField("Chomusuke Item", "Mythical Chomusuke Capsule (Open it with `n!openCapsule`!)");
                
                GlobalUserAccounts.SaveAccounts();
            }
            int duelBool = Global.Rng.Next(1, 3);
            if (duelBool == 1 || duelBool == 2)
            {
                int duelValue = Global.Rng.Next(53, 57);
                string item = GetDuelItem(user, duelValue);
                embed.AddField("Duels Item", $"{item} (x1)");
            }
            await channel.SendMessageAsync("", embed: embed.Build());
        }

        public static string GetDuelItem(SocketUser user, int value)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            string item = string.Empty;
            switch (value)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    item = "Strength Potion";
                    if (config.Items.ContainsKey("Strength Potion")) config.Items["Strength Potion"] += 1;
                    else config.Items.Add("Strength Potion", 1);
                    break;
                case 13:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 14:
                    item = "Speed Potion";
                    if (config.Items.ContainsKey("Speed Potion")) config.Items["Speed Potion"] += 1;
                    else config.Items.Add("Speed Potion", 1);
                    break;
                case 19:
                case 20:
                case 15:
                case 16:
                case 17:
                case 18:
                case 21:
                    item = "Debuff Potion";
                    if (config.Items.ContainsKey("Debuff Potion")) config.Items["Debuff Potion"] += 1;
                    else config.Items.Add("Debuff Potion", 1);
                    break;
                case 25:
                case 26:
                case 27:
                case 22:
                case 23:
                case 24:
                case 28:
                    item = "Equalizer Potion";
                    if (config.Items.ContainsKey("Equalizer Potion")) config.Items["Equalizer Potion"] += 1;
                    else config.Items.Add("Equalizer Potion", 1);
                    break;
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                    item = "Vile of Poison";
                    if (config.Items.ContainsKey("Vile Of Poison")) config.Items["Vile Of Poison"] += 1;
                    else config.Items.Add("Vile Of Poison", 1);
                    break;
                case 29:
                case 30:
                case 31:
                case 32:
                case 33:
                    item = "Metallic Acid";
                    if (config.Items.ContainsKey("Metallic Acid")) config.Items["Metallic Acid"] += 1;
                    else config.Items.Add("Metallic Acid", 1);
                    break;
                case 34:
                case 35:
                case 36:
                case 37:
                case 38:
                    item = "Weapon Liquifier";
                    if (config.Items.ContainsKey("Weapon Liquifier")) config.Items["Weapon Liquifier"] += 1;
                    else config.Items.Add("Weapon Liquifier", 1);
                    break;
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                    item = "Basic Treatment";
                    if (config.Items.ContainsKey("Basic Treatment")) config.Items["Basic Treatment"] += 1;
                    else config.Items.Add("Basic Treatment", 1);
                    break;
                case 49:
                case 50:
                case 51:
                case 52:
                    item = "Divine Shield";
                    if (config.Items.ContainsKey("Divine Shield")) config.Items["Divine Shield"] += 1;
                    else config.Items.Add("Divine Shield", 1);
                    break;
                case 53:
                    if (config.blessingProtection != true)
                    {
                        item = "Blessing of Protection";
                        config.blessingProtection = true;
                    }
                    else
                    {
                        item = "Blessing of Protection [DUPLICATE] (+2000 Taiyakis)";
                        config.Taiyaki = config.Taiyaki + 2000;
                    }
                    break;
                case 54:
                    if (config.blessingSwiftness != true)
                    {
                        item = "Blessing of Swiftness";
                        config.blessingSwiftness = true;
                    }
                    else
                    {
                        item = "Blessing of Swiftness [DUPLICATE] (+2000 Taiyakis)";
                        config.Taiyaki = config.Taiyaki + 2000;
                    }
                    break;
                case 55:
                    if (config.blessingStrength != true)
                    {
                        item = "Blessing of Stength";
                        config.blessingStrength = true;
                    }
                    else
                    {
                        item = "Blessing of Stength [DUPLICATE] (+2000 Taiyakis)";
                        config.Taiyaki = config.Taiyaki + 2000;
                    }
                    break;
                case 56:
                    if (config.blessingWar != true)
                    {
                        item = "Blessing of War";
                        config.blessingWar = true;
                    }
                    else
                    {
                        item = "Blessing of War [DUPLICATE] (+2000 Taiyakis)";
                        config.Taiyaki = config.Taiyaki + 2000;
                    }
                    break;
            }
            GlobalUserAccounts.SaveAccounts();
            return item;
        } //1-56
    }
}
