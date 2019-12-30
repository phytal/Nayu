using System;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;

namespace Nayu.Core.Features.Economy
{
    public static class Daily
    {
        public struct DailyResult
        {
            public bool Success;
            public TimeSpan RefreshTimeSpan;
        }

        public static DailyResult GetDaily(ulong userId)
        {
            var account = GlobalUserAccounts.GetUserAccount(userId);
            var difference = DateTime.UtcNow - account.LastDaily.AddDays(1);

            if (difference.TotalHours < 0) return new DailyResult { Success = false, RefreshTimeSpan = difference };

            account.Taiyaki += Constants.DailyTaiyakiGain;
            account.LastDaily = DateTime.UtcNow;
            GlobalUserAccounts.SaveAccounts(userId);
            return new DailyResult { Success = true };
        }

        public static DailyResult GetRep(SocketGuildUser user)
        {
            var account = GlobalGuildUserAccounts.GetUserID(user);
            var difference = DateTime.UtcNow - account.LastRep.AddDays(1);

            if (difference.TotalHours < 0) return new DailyResult { Success = false, RefreshTimeSpan = difference };

            account.LastRep = DateTime.UtcNow;
            GlobalGuildUserAccounts.SaveAccounts();
            return new DailyResult { Success = true };
        }
    }
}
