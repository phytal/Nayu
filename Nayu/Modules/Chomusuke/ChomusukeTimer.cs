using System;
using System.Threading.Tasks;
using System.Timers;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Inbox;

namespace Nayu.Modules.Chomusuke
{
    public class ChomusukeTimer
    {
        private static Timer loopingtimer;

        internal Task StartTimer()
        {
            var fourHoursInMiliSeconds = 28800000;
            loopingtimer = new Timer()
            {
                Interval = fourHoursInMiliSeconds,
                AutoReset = true,
                Enabled = true
            };
            loopingtimer.Elapsed += OnTimerTicked;

            Console.WriteLine("Initialized Mission - Cripple Chomusuke");
            return Task.CompletedTask;
        }
        //try to get something so that all pets will experience soemthing
        public async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            var config = GlobalUserAccounts.GetAllAccounts();
            foreach (var userAcc in config)
            {
                Core.Entities.Chomusuke activeChomusuke = Global.NewChomusuke;
                if (userAcc.ActiveChomusuke == 1)
                    activeChomusuke = userAcc.Chomusuke1;
                if (userAcc.ActiveChomusuke == 2)
                    activeChomusuke = userAcc.Chomusuke2;
                if (userAcc.ActiveChomusuke == 3)
                    activeChomusuke = userAcc.Chomusuke3;
                if (userAcc.Chomusuke1.Have)
                {
                    if (activeChomusuke.Hunger > 0)
                        activeChomusuke.Hunger -= 1;
                    else activeChomusuke.Hunger = 0;
                    if (activeChomusuke.Waste < 20)
                        activeChomusuke.Waste += 1;
                    else activeChomusuke.Waste = 20;

                    var user = Global.Client.GetUser(userAcc.Id);
                    if (activeChomusuke.Waste >= 15)
                    {
                        activeChomusuke.Sick = true;
                        await CreateMessage.CreateAndSendMessageAsync("Chomusuke Alert",$":exclamation:  | {user.Mention}, **{activeChomusuke.Name}** is sick! Treat it right with medicine with n!buy!", DateTime.Now, user);
                    }
                    if ((activeChomusuke.Waste == 20) && (activeChomusuke.Hunger <= 5))
                    {
                        activeChomusuke.Trust -= 1;
                        await CreateMessage.CreateAndSendMessageAsync("Chomusuke Alert",$":exclamation:  | {user.Mention}, **{activeChomusuke.Name}** is losing trust in you! The living conditions you provided were too low... Maybe try to pay more attention to your Chomusuke!", DateTime.Now, user);
                    }
                    if (userAcc.ActiveChomusuke == 1)
                        userAcc.Chomusuke1 = activeChomusuke;
                    if (userAcc.ActiveChomusuke == 2)
                        userAcc.Chomusuke2 = activeChomusuke;
                    if (userAcc.ActiveChomusuke == 3)
                        userAcc.Chomusuke3 = activeChomusuke;
                    GlobalUserAccounts.SaveAccounts(userAcc.Id);
                }
                else return;
            }
            Console.WriteLine("Successfully executed pet crippling effects.");
        }
    }
}
