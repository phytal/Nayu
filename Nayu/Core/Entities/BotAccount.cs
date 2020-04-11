using System;
using System.Collections.Generic;

namespace Nayu.Core.Entities
{
    public class BotAccount
    {
        public ulong Id { get; set; }
        public int ShardCount
        {
            get { return Global.Client.Guilds.Count / 1500 + 1; }
        }
        public string ChangeLog { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<ulong> AutoLewdGuilds { get; set; } = new List<ulong>();
        public Dictionary<ulong, ulong> BlockedChannels { get; set; } = new Dictionary<ulong, ulong>(); //channel id, guild id
    }
}