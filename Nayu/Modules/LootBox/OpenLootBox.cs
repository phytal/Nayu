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
            byte taiyakies = (byte) Global.Rng.Next(50, 100);
            config.Taiyaki += taiyakies;
            embed.AddField("Taiyakis", taiyakies);

            configg.NormalCapsule += 1;
            embed.AddField("Chomusuke Item", "Normal Chomusuke Capsule (Open it with `n!openCapsule`!)");
            int duelBool = Global.Rng.Next(1, 4);//2/3 chance
            if (duelBool == 1 || duelBool == 2)
            {
                string item = ItemProbability.DuelsItemProbabiliy(user, 'c');
                embed.AddField("Duels Item", $"{item} (x1)");
            }

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
            ushort taiyakies = (ushort) Global.Rng.Next(150, 301);
            config.Taiyaki += taiyakies;
            embed.AddField("Taiyakis", taiyakies);

            byte chomusukeBool = (byte) Global.Rng.Next(1, 4);
            if (chomusukeBool == 1 || chomusukeBool == 2)
            {
                config.NormalCapsule += 1;
                embed.AddField("Chomusuke Item", "Normal Chomusuke Capsule (Open it with `n!openCapsule`!)");
            }


            string item = ItemProbability.DuelsItemProbabiliy(user, 'u');
            embed.AddField("Duels Item", $"{item} (x1)");

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
            ushort taiyakies = (ushort)Global.Rng.Next(320, 500);
            config.Taiyaki += taiyakies;
            embed.AddField("Taiyakis", taiyakies);

            config.NormalCapsule += 1;
            embed.AddField("Chomusuke Item", "Normal Chomusuke Capsule (Open it with `n!openCapsule`!)");

            string item = ItemProbability.DuelsItemProbabiliy(user, 'r');
            embed.AddField("Duels Item", $"{item} (x1)");


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
            ushort taiyakies = (ushort)Global.Rng.Next(550, 750);
            config.Taiyaki += taiyakies;
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



            string item = ItemProbability.DuelsItemProbabiliy(user, 'e');
            embed.AddField("Duels Item", $"{item} (x1)");

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
            config.LootBoxLegendary -= 1;
            ushort taiyakies = (ushort)Global.Rng.Next(800, 1200);
            config.Taiyaki += taiyakies;
            embed.AddField("Taiyakis", taiyakies);

            int chomusukeBool = Global.Rng.Next(1, 3);
            if (chomusukeBool == 1 || chomusukeBool == 2)
            {
                configg.MythicalCapsule += 1;
                embed.AddField("Chomusuke Item", "Mythical Chomusuke Capsule (Open it with `n!openCapsule`!)");

                GlobalUserAccounts.SaveAccounts();

                string item = ItemProbability.DuelsItemProbabiliy(user, 'l');
                embed.AddField("Duels Item", $"{item} (x1)");

                await channel.SendMessageAsync("", embed: embed.Build());
            }

        }
    }
}
