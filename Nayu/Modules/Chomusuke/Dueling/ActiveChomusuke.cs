using System;
using System.Threading.Tasks;
using Nayu.Core.Features.GlobalAccounts;

namespace Nayu.Modules.Chomusuke.Dueling
{
    public class ActiveChomusuke
    {
        public static Tuple<Core.Entities.Chomusuke, Core.Entities.Chomusuke> GetActiveChomusuke(ulong user1, ulong user2)
        {
            var config = GlobalUserAccounts.GetUserAccount(user1);
            var configg = GlobalUserAccounts.GetUserAccount(user2);
            Core.Entities.Chomusuke activeChomusuke = Global.NewChomusuke;
            Core.Entities.Chomusuke activeChomusukee = Global.NewChomusuke;
            switch (config.ActiveChomusuke)
            {
                case 1:
                    activeChomusuke = config.Chomusuke1;
                    break;
                case 2:
                    activeChomusuke = config.Chomusuke2;
                    break;
                case 3:
                    activeChomusuke = config.Chomusuke3;
                    break;
            }

            switch (configg.ActiveChomusuke)
            {
                case 1:
                    activeChomusukee = configg.Chomusuke1;
                    break;
                case 2:
                    activeChomusukee = configg.Chomusuke2;
                    break;
                case 3:
                    activeChomusukee = configg.Chomusuke3;
                    break;
            }
            return new Tuple<Core.Entities.Chomusuke, Core.Entities.Chomusuke>(activeChomusuke, activeChomusukee);
        }
        public static Core.Entities.Chomusuke GetOneActiveChomusuke(ulong user)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            Core.Entities.Chomusuke activeChomusuke = Global.NewChomusuke;
            switch (config.ActiveChomusuke)
            {
                case 1:
                    activeChomusuke = config.Chomusuke1;
                    break;
                case 2:
                    activeChomusuke = config.Chomusuke2;
                    break;
                case 3:
                    activeChomusuke = config.Chomusuke3;
                    break;
            }
            return activeChomusuke;
        }

        public static Task ConvertActiveVariable(ulong user1, ulong user2, Core.Entities.Chomusuke activeChomusuke,
            Core.Entities.Chomusuke activeChomusukee)
        {
            var config = GlobalUserAccounts.GetUserAccount(user1);
            var configg = GlobalUserAccounts.GetUserAccount(user2);
            switch (config.ActiveChomusuke)
            {
                case 1:
                    config.Chomusuke1 = activeChomusuke;
                    break;
                case 2:
                    config.Chomusuke2 = activeChomusuke;
                    break;
                case 3:
                    config.Chomusuke3 = activeChomusuke;
                    break;
            }

            switch (configg.ActiveChomusuke)
            {
                case 1:
                    configg.Chomusuke1 = activeChomusukee;
                    break;
                case 2:
                    configg.Chomusuke2 = activeChomusukee;
                    break;
                case 3:
                    configg.Chomusuke3 = activeChomusukee;
                    break;
            }

            return Task.CompletedTask;
        }

        public static Task ConvertOneActiveVariable(ulong user, Core.Entities.Chomusuke activeChomusuke)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            switch (config.ActiveChomusuke)
            {
                case 1:
                    config.Chomusuke1 = activeChomusuke;
                    break;
                case 2:
                    config.Chomusuke2 = activeChomusuke;
                    break;
                case 3:
                    config.Chomusuke3 = activeChomusuke;
                    break;
            }
            GlobalUserAccounts.SaveAccounts(user);
            return Task.CompletedTask;
        }
    }
}