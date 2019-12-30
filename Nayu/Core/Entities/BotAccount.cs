using System.Collections.Generic;

namespace Nayu.Core.Entities
{
    public class BotAccount
    {
        public Dictionary<ulong, ulong> BlockedChannels = new Dictionary<ulong, ulong>(); //channel id, guild id
    }
}