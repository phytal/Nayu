using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Inbox;

namespace Nayu.Core.LevelingSystem
{
    public class LevelingRewards
    {
        public static Task CheckDuelRewards(SocketUser user)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            uint wins = config.Wins;

            switch (wins)
            {
                case (10):
                    config.Title = "Classic Adventurer";
                    break;
                case (20):
                    config.Title = "Courageous Explorer";
                    break;
                case (30):
                    config.Title = "Daring Knight";
                    break;
                case (40):
                    config.Title = "Monster Hunter";
                    break;
                case (50):
                    config.Title = ("Noble Swordsman");
                    break;
                case (60):
                    config.Title = ("Valiant Paladin");
                    break;
                case (70):
                    config.Title = ("Dragon Slayer");
                    break;
                case (80):
                    config.Title = ("Respectable Hero");
                    break;
                case (90):
                    config.Title = ("Holy Protectorate");
                    break;
                case (100):
                    config.Title = ("Saint");
                    break;
                case (120):
                    config.Title = ("Defender of Gods");
                    break;
                case (150):
                    config.Title = ("God Eater");
                    break;
            }

            GlobalUserAccounts.SaveAccounts(config.Id);
            return Task.CompletedTask;
        }
        public static async Task CheckDuelLootboxes(SocketUser user)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            var channel = await user.GetOrCreateDMChannelAsync();
            var wins = config.Wins;
            string msg = "";

            var c = wins % 3;
            var uc = wins % 10;
            var rare = wins % 20;
            var epic = wins % 35;
            var legendary = wins % 50;
            if (legendary == 0)
            {
                config.LootBoxLegendary += 1;
                msg = $"**{user.Username}**, you have received a **LEGENDARY** lootbox for reaching {config.Wins} wins!";
            }
            else if (epic == 0)
            {
                config.LootBoxEpic += 1;
                msg = $"**{user.Username}**, you have received a **EPIC** lootbox for reaching {config.Wins} wins!";
            }
            else if (rare == 0)
            {
                config.LootBoxRare += 1;
                msg = $"**{user.Username}**, you have received a **RARE** lootbox for reaching {config.Wins} wins!";
            }
            else if (uc == 0)
            {
                config.LootBoxUncommon += 1;
                msg = $"**{user.Username}**, you have received a **UNCOMMON** lootbox for reaching {config.Wins} wins!";
            }
            else if (c == 0)
            {
                config.LootBoxCommon += 1;
                msg = $"**{user.Username}**, you have received a **COMMON** lootbox for reaching {config.Wins} wins!";
            }
            GlobalUserAccounts.SaveAccounts(user.Id);
            await CreateMessage.CreateAndSendMessageAsync("Lootbox Reward", msg, DateTime.UtcNow, user);
        }

            public static async Task CheckLootBoxRewards(SocketUser user)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            var channel = await user.GetOrCreateDMChannelAsync();
            int level = (int)config.LevelNumber;

            string msg;
            
            int uc = level % 5;
            int rare = level % 10;
            int epic = level % 15;
            int legendary = level % 20;
            if (legendary == 0)
            {
                config.LootBoxLegendary += 1;
                msg = $"**{user.Username}**, you have received a **LEGENDARY** lootbox for reaching level {config.LevelNumber}";
            }
            else if (epic == 0)
            {
                config.LootBoxEpic += 1;
                msg = $"**{user.Username}**, you have received a **EPIC** lootbox for reaching level {config.LevelNumber}";
            }
            else if (rare == 0)
            {
                config.LootBoxRare += 1;
                msg = $"**{user.Username}**, you have received a **RARE** lootbox for reaching level {config.LevelNumber}";
            }
            else if (uc == 0)
            {
                config.LootBoxUncommon += 1;
                msg = $"**{user.Username}**, you have received a **UNCOMMON** lootbox for reaching level {config.LevelNumber}";
            }
            else
            {
                config.LootBoxCommon += 1;
                msg = $"**{user.Username}**, you have received a **COMMON** lootbox for reaching level {config.LevelNumber}";
            }
            GlobalUserAccounts.SaveAccounts(user.Id);
            await CreateMessage.CreateAndSendMessageAsync("Lootbox Reward", msg, DateTime.UtcNow, user);
        }
    }
}
