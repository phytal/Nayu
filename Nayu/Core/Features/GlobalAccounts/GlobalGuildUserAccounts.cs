using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord.WebSocket;
using Nayu.Core.Entities;

namespace Nayu.Core.Features.GlobalAccounts
{
    internal static class GlobalGuildUserAccounts
    {
        private static SocketGuildUser user;
        private static readonly ConcurrentDictionary<string, GlobalGuildUserAccount> userAccounts = new ConcurrentDictionary<string, GlobalGuildUserAccount>();
        static GlobalGuildUserAccounts()
        {
            var info = System.IO.Directory.CreateDirectory(Path.Combine(Constants.ResourceFolder, Constants.ServerUserAccountsFolder));
            var files = info.GetFiles("*.json");
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    var user = Configuration.DataStorage.RestoreObject<GlobalGuildUserAccount>(Path.Combine(file.Directory.Name, file.Name));
                    userAccounts.TryAdd(user.UniqueId, user);
                }
            }
            else {
                userAccounts = new ConcurrentDictionary<string, GlobalGuildUserAccount>();
            }
        }

        internal static GlobalGuildUserAccount GetUserID(string id, ulong nid)
        {
            return userAccounts.GetOrAdd(id, (key) =>
            {
                var newAccount = new GlobalGuildUserAccount { UniqueId = id , Id = nid};
                Configuration.DataStorage.StoreObject(newAccount, Path.Combine(Constants.ServerUserAccountsFolder, $"{id}.json"), useIndentations: true);
                return newAccount;
            });
        }

        internal static GlobalGuildUserAccount GetUserID(SocketGuildUser user)
        {
            return GetUserID($"{user.Guild.Id}{user.Id}", user.Id);
        }


        internal static List<GlobalGuildUserAccount> GetAllAccounts()
        {
            return userAccounts.Values.ToList();
        }


        internal static List<GlobalGuildUserAccount> GetFilteredAccounts(Func<GlobalGuildUserAccount, bool> filter)
        {
            return userAccounts.Values.Where(filter).ToList();
        }

        internal static void SaveAccount(string uId, ulong id)
        {
            Configuration.DataStorage.StoreObject(GetUserID(uId, id), Path.Combine(Constants.ServerUserAccountsFolder, $"{uId}.json"), useIndentations: true);

        }
        /// <summary>
        /// This rewrites ALL UserAccounts to the harddrive... Strongly recommend to use SaveAccounts(id1, id2, id3...) where possible instead
        /// </summary>
        internal static void SaveAccounts()
        {
            foreach (var userAcc in userAccounts.Values)
            {
                SaveAccount(userAcc.UniqueId, userAcc.Id);
            }
        }

        /// <summary>
        /// Saves one or multiple Accounts by provided Ids
        /// </summary>
        internal static void SaveAccounts(params string[] ids)
        {
            foreach (var id in ids)
            {
                Configuration.DataStorage.StoreObject(GetUserID(id, user.Id), Path.Combine(Constants.ServerUserAccountsFolder, $"{id}.json"), useIndentations: true);
            }
        }
    }
}
