using System;
using System.Collections.Generic;

namespace Nayu
{
    class Timeouts
    {
        internal static Dictionary<string, Dictionary<ulong, DateTime>> timeouts = new Dictionary<string, Dictionary<ulong, DateTime>>();

        internal static bool HasCommandTimeout(ulong userId, string command, double seconds = -1)
        {
            seconds = (seconds == -1) ? 10 : Math.Abs(seconds);
            var cmdTimeouts = GetOrCreateCommandTimeouts(command);
            if (!cmdTimeouts.ContainsKey(userId))
            {
                cmdTimeouts.Add(userId, DateTime.Now);
                return false;
            }
            DateTime lastRequest = cmdTimeouts[userId];
            TimeSpan difference = DateTime.Now - lastRequest;
            if (difference.TotalSeconds >= seconds)
            {
                cmdTimeouts[userId] = DateTime.Now;
                return false;
            }
            return true;
        }

        private static Dictionary<ulong, DateTime> GetOrCreateCommandTimeouts(string command)
        {
            if (!timeouts.ContainsKey(command)) timeouts.Add(command, new Dictionary<ulong, DateTime>());
            return timeouts[command];
        }
    }
}