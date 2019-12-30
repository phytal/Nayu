using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Nayu.Core.Features.GlobalAccounts;

namespace Nayu.Core.LevelingSystem
{
    public class Leveling
    {
        public async Task NayuLevelandMessageReward(SocketMessage s)
        {
            if (s == null) return;
            if (s.Channel == s.Author.GetOrCreateDMChannelAsync()) return;
            if (s.Author.IsBot) return;

            var userAccount = GlobalUserAccounts.GetUserAccount(s.Author.Id);
            DateTime now = DateTime.UtcNow;

            // Check if message is long enough and if the coolown of the reward is up - if not return
            if (now < userAccount.LastMessage.AddSeconds(Constants.MessageRewardCooldown) || s.Content.Length < Constants.MessageRewardMinLenght)
            {
                return; // This Message is not eligible for a reward
            }

            // Generate a randomized reward in the configured boundries
            uint moneyGained = (uint)Global.Rng.Next(Constants.MessagRewardMinMax.Item1, Constants.MessagRewardMinMax.Item2 + 1);
            userAccount.Taiyaki += moneyGained;
            userAccount.TaiyakiFromMessages += moneyGained;
            userAccount.LastMessage = now;

            if (now < userAccount.LastXPMessage.AddSeconds(Constants.MessageXPCooldown))
            {
                return;
            }

            userAccount.LastXPMessage = now;

            uint oldLevel = userAccount.LevelNumber;
            userAccount.XP += 7;
            GlobalUserAccounts.SaveAccounts(userAccount.Id);
            uint newLevel = userAccount.LevelNumber;
            if (oldLevel != newLevel)
            {
                await LevelingRewards.CheckLootBoxRewards(s.Author);
            }
        }
    }
}
