using System;
using System.Threading.Tasks;
using System.Timers;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Chomusuke.Dueling;
using Nayu.Modules.Inbox;

namespace Nayu.Modules.Chomusuke
{
    public class ChomusukeTimer
    {
        private static Timer _loopingTimer;

        internal Task StartTimer()
        {
            const int fourHoursInMilliSeconds = 28800000;
            _loopingTimer = new Timer()
            {
                Interval = fourHoursInMilliSeconds,
                AutoReset = true,
                Enabled = true
            };
            _loopingTimer.Elapsed += OnTimerTicked;

            Console.WriteLine("Beginning Chomusuke Actions...");
            return Task.CompletedTask;
        }
        
        private async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            var config = GlobalUserAccounts.GetAllAccounts();
            foreach (var userAcc in config)
            {
                var activeChomusuke = ActiveChomusuke.GetOneActiveChomusuke(userAcc.Id);
                if (userAcc.ActiveChomusuke != 0)
                {
                    if ((DateTime.UtcNow - userAcc.MeguminDay).TotalDays < 7)
                    {
                        continue;
                    }
                    if (activeChomusuke.Hunger > 0)
                        activeChomusuke.Hunger -= 1;
                    if (activeChomusuke.Waste < 20)
                        activeChomusuke.Waste += 1;

                    var user = Global.Client.GetUser(userAcc.Id);
                    if (activeChomusuke.Waste >= 15)
                    {
                        activeChomusuke.Sick = true;
                        await CreateMessage.CreateAndSendMessageAsync("Chomusuke Alert",
                            $":exclamation:  | {user.Mention}, **{activeChomusuke.Name}** is sick! Treat it right with medicine with n!cBuy!",
                            DateTime.Now, user);
                    }

                    if (activeChomusuke.Waste == 20 && activeChomusuke.Hunger <= 5)
                    {
                        if (activeChomusuke.Trust > 0)
                            activeChomusuke.Trust -= 1;
                        await CreateMessage.CreateAndSendMessageAsync("Chomusuke Alert",
                            $":exclamation:  | {user.Mention}, **{activeChomusuke.Name}** is losing trust in you! The living conditions you provided are too low... Maybe try paying more attention to your Chomusuke!",
                            DateTime.Now, user);
                    }

                    await ActiveChomusuke.ConvertOneActiveVariable(userAcc.Id, activeChomusuke);
                    GlobalUserAccounts.SaveAccounts(userAcc.Id);
                }
                else return;
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Executed Chomusuke Actions");
        }
    }
}