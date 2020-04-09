using System;
using System.Threading.Tasks;
using Nayu.Core.Features.GlobalAccounts;

namespace Nayu.Modules.Chomusuke.Dueling
{
    using Chomusuke = Nayu.Core.Entities.Chomusuke;
    public class ActiveChomusuke
    {
        public static ChomusukeGroup GetActiveChomusuke(ulong user1,
            ulong user2)
        {
            var config = GlobalUserAccounts.GetUserAccount(user1);
            var configg = GlobalUserAccounts.GetUserAccount(user2);
            Chomusuke activeChomusuke = Global.NewChomusuke;
            Chomusuke activeChomusukee = Global.NewChomusuke;
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

            return new ChomusukeGroup{ChomusukeOne = activeChomusuke, ChomusukeTwo = activeChomusukee};
        }

        public static Chomusuke GetOneActiveChomusuke(ulong user)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            Chomusuke activeChomusuke = Global.NewChomusuke;
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

        /// <summary>
        /// matches 2 active chomusukes with its respective number
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <param name="activeChomusuke"></param>
        /// <param name="activeChomusukee"></param>
        /// <returns></returns>
        public static Task ConvertActiveVariable(ulong user1, ulong user2, Chomusuke activeChomusuke,
            Chomusuke activeChomusukee)
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

        /// <summary>
        /// matches 1 active chomusuke with its respective number
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <param name="activeChomusuke"></param>
        /// <param name="activeChomusukee"></param>
        /// <returns></returns>
        public static Task ConvertOneActiveVariable(ulong user, Chomusuke activeChomusuke)
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

    public class ChomusukeGroup
    {
        public Chomusuke ChomusukeOne { get; set; }
        public Chomusuke ChomusukeTwo { get; set; }
    }
}