using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord;
using Nayu.Core.Configuration;
using Nayu.Core.Entities;
using Nayu.Modules.Chomusuke;
using Type = Nayu.Modules.Chomusuke.Dueling.Enums.Type;

namespace Nayu.Core.Features.GlobalAccounts
{
    internal static class GlobalUserAccounts
    {
        private static readonly ConcurrentDictionary<ulong, GlobalUserAccount> userAccounts = new ConcurrentDictionary<ulong, GlobalUserAccount>();

        static GlobalUserAccounts()
        {
            var info = Directory.CreateDirectory(Path.Combine(Constants.ResourceFolder, Constants.UserAccountsFolder));
            var files = info.GetFiles("*.json");
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    var user = DataStorage.RestoreObject<GlobalUserAccount>(Path.Combine(file.Directory.Name, file.Name));
                    userAccounts.TryAdd(user.Id, user);
                }
            }
            else
            {
                userAccounts = new ConcurrentDictionary<ulong, GlobalUserAccount>();
            }
        }

         internal static GlobalUserAccount GetUserAccount(ulong id)
         {
             return userAccounts.GetOrAdd(id, (key) =>
             {
                 var newAccount = new GlobalUserAccount { Id = id, Title = "Adventurer", Chomusuke1 = new Chomusuke(false, null, null, false, 0, 0, 0, 0, false, null, null, null, null, Type.None, 0, Trait.None, Trait.None, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, DateTime.MinValue,null, null, null), Chomusuke2 = new Chomusuke(false, null, null, false, 0, 0, 0, 0, false, null, null, null, null, Type.None, 0, Trait.None, Trait.None, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, DateTime.MinValue,null, null, null), Chomusuke3 = new Chomusuke(false, null, null, false, 0, 0, 0, 0, false, null, null, null, null, Type.None, 0, Trait.None, Trait.None, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, DateTime.MinValue, null, null, null)
             };
                 Configuration.DataStorage.StoreObject(newAccount, Path.Combine(Constants.UserAccountsFolder, $"{id}.json"), useIndentations: true);
                 return newAccount;
             });
         }

        internal static GlobalUserAccount GetUserAccount(IUser user)
        {
            return GetUserAccount(user.Id);
        }

        internal static List<GlobalUserAccount> GetAllAccounts()
        {
            return userAccounts.Values.ToList();
        }


        internal static List<GlobalUserAccount> GetFilteredAccounts(Func<GlobalUserAccount, bool> filter)
        {
            return userAccounts.Values.Where(filter).ToList();
        }
        

        /// <summary>
        /// This rewrites ALL UserAccounts to the harddrive... Strongly recommend to use SaveAccounts(id1, id2, id3...) where possible instead
        /// </summary>
        internal static void SaveAllAccounts()
        {
            foreach (var id in userAccounts.Keys)
            {
                SaveAccounts(id);
            }
        }

        /// <summary>
        /// Saves one or multiple Accounts by provided Ids
        /// </summary>
        internal static void SaveAccounts(params ulong[] ids)
        {
            foreach (var id in ids)
            {
                DataStorage.StoreObject(GetUserAccount(id), Path.Combine(Constants.UserAccountsFolder, $"{id}.json"), useIndentations: true);
            }
        }
    }
}
