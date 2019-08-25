using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Discord.WebSocket;
using Nayu.Features;
using Nayu.Features.GlobalAccounts;

namespace Nayu
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

            Console.WriteLine("Initialized Mission - Cripple Chomusuke"); //lmao such cringe
            return Task.CompletedTask;
        }
        //try to get something so that all pets will experience soemthing
        public async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            var config = GlobalUserAccounts.GetAllAccounts();
            foreach (var userAcc in config)
            {/*
                if (userAcc.Have == true)
                {
                    if (userAcc.Hunger > 0)
                        userAcc.Hunger = userAcc.Hunger - 1;
                    else userAcc.Hunger = 0;
                    if (userAcc.Waste < 20)
                        userAcc.Waste = userAcc.Waste + 1;
                    else userAcc.Waste = 20;
                    if (userAcc.Attention > 0)
                        userAcc.Attention = userAcc.Attention - 1;
                    else userAcc.Attention = 0;
                    GlobalUserAccounts.SaveAccounts();

                    var user = Global.Client.GetUser(userAcc.Id);
                    Console.WriteLine($"{userAcc.Id}");
                    var message = await user.GetOrCreateDMChannelAsync();
                    if (userAcc.Waste >= 15)
                    {
                        userAcc.Sick = true;
                        GlobalUserAccounts.SaveAccounts();
                        await message.SendMessageAsync($":exclamation:  | {user.Mention}, **{userAcc.Name}** is sick! Treat her right with medicine with n!buy! ");
                    }
                    if ((userAcc.Waste == 20) && (userAcc.Hunger <= 5) && (userAcc.Attention <= 5))
                    {
                        userAcc.XP = 0;
                        userAcc.Name = null;
                        userAcc.Have = false;
                        userAcc.pfp = null;
                        userAcc.RanAway = true;
                        await message.SendMessageAsync($":exclamation:  | {user.Mention}, **{userAcc.Name}** ran away! The living conditions you provided were too low... Maybe try to pay more attention to your Chomusuke next time! ");
                        GlobalUserAccounts.SaveAccounts();
                    }
                    GlobalUserAccounts.SaveAccounts();
                }
                else return;*/
            }
            Console.WriteLine("Successfully executed pet crippling effects.");
        }
    }
}
