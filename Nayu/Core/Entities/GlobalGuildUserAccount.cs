using System;
using System.Collections.Generic;

namespace Nayu.Core.Entities
{
    public class GlobalGuildUserAccount
    {
        public string Id { get; set; } //guildId+userId

        public ulong UserId { get; set; }

        public uint Reputation { get; set; }

        public DateTime LastRep { get; set; } = DateTime.UtcNow.AddDays(-2);

        public ushort NumberOfWarnings { get; set; }

        public List<string> Warnings { get;  set; } = new List<string>();
    }
}