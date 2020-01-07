using System.Collections.Generic;

namespace Nayu.Core.Entities
{
    public class BotAccount
    {
        public int ShardCount
        {
            get { return Global.Client.Guilds.Count / 1500 + 1; }
        }
        public Dictionary<ulong, ulong> BlockedChannels = new Dictionary<ulong, ulong>(); //channel id, guild id
    }
}