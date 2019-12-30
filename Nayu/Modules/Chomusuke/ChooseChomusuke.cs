using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Nayu.Modules.Chomusuke.Dueling;

namespace Nayu.Modules.Chomusuke
{
    public class ChooseChomusuke : NayuModule
    {
        public static async Task ChooseActionAsync(SocketUser user, string action)
        {
            var activeChomusuke = ActiveChomusuke.GetOneActiveChomusuke(user.Id);
            if (action == "cure")
            {
                activeChomusuke.Sick = false;
                activeChomusuke.Waste = 0;
                await ActiveChomusuke.ConvertOneActiveVariable(user.Id, activeChomusuke);
            }
        }
    }
}
