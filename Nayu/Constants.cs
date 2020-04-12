using System;
using Discord;

namespace Nayu
{
    public static class Constants
    {
        internal static readonly string ResourceFolder = "resources";
        internal static readonly string MemeFolder = "memes";
        internal static readonly string UserAccountsFolder = "users";
        internal static readonly string ServerAccountsFolder = "servers";
        internal static readonly string ServerUserAccountsFolder = "serverUsers";
        internal static readonly string InvisibleString = "\u200b";
        public const ulong DailyTaiyakiGain = 100;
        public const int MessageRewardCooldown = 30;
        public const int MessageXpCooldown = 6;
        public const int MessageRewardMinLenght = 20;
        public static readonly Color DefaultColor = new Color(230, 230, 230);
        public static readonly Tuple<int, int> MessagRewardMinMax = Tuple.Create(1, 5);
    }
}