using System;
using System.Collections.Generic;

namespace Nayu.Entities
{
    public class GlobalGuildUserAccount
    {
        public string UniqueId { get; set; }

        public ulong Id { get; set; }

        public uint Reputation { get; set; }

        public DateTime LastRep { get; set; } = DateTime.UtcNow.AddDays(-2);

        public uint NumberOfWarnings { get; set; }

        public List<string> Warnings { get; private set; } = new List<string>();
    }
}