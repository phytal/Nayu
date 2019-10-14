using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nayu.Entities
{
    public class GlobalGuildAccount : IGlobalAccount
    {
        public List<string> WelcomeMessages { get; set; } = new List<string> { };

        public List<string> LeaveMessages { get; set; } = new List<string> { };

        public List<ulong> AntilinkIgnoredChannels { get; set; } = new List<ulong> { };

        public List<ulong> NoFilterChannels { get; set; } = new List<ulong> { };

        public List<string> CustomFilter { get; set; } = new List<string> { };

        public List<string> SelfRoles { get; set; } = new List<string> { };

        public ulong Id { get; set; }

        public bool Filter { get; set; }

        public bool Antilink { get; set; }

        public bool Unflip { get; set; }

        public string LevelingMsgs { get; set; }

        public string CommandPrefix { get; set; }

        public bool Leveling { get; set; }

        public ulong WelcomeChannel { get; set; }

        public ulong LeaveChannel { get; set; }

        public bool MassPingChecks { get; set; }

        public ulong GuildOwnerId { get; set; }

        public string Autorole { get; set; }

        public ulong ServerLoggingChannel { get; set; }

        public bool IsServerLoggingEnabled { get; set; }

        public ulong SlowModeCooldown { get; set; }

        public bool IsSlowModeEnabled { get; set; }

        public string Currency { get; set; }

        public ulong AutoLewdChannel { get; set; }

        public bool AutoLewdStatus { get; set; }

        public Dictionary<string, string> CustomCommands { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();

    }
}