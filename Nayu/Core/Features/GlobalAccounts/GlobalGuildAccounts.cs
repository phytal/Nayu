using System.Collections.Concurrent;
using System.IO;
using Discord;
using Nayu.Core.Entities;

namespace Nayu.Core.Features.GlobalAccounts
{
    internal static class GlobalGuildAccounts
    {
        private static readonly ConcurrentDictionary<ulong, GlobalGuildAccount> serverAccounts = new ConcurrentDictionary<ulong, GlobalGuildAccount>();

        static GlobalGuildAccounts()
        {
            var info = Directory.CreateDirectory(Path.Combine(Constants.ResourceFolder, Constants.ServerAccountsFolder));
            var files = info.GetFiles("*.json");
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    var server = Configuration.DataStorage.RestoreObject<GlobalGuildAccount>(Path.Combine(file.Directory.Name, file.Name));
                    serverAccounts.TryAdd(server.Id, server);
                }
            }
            else
            {
                serverAccounts = new ConcurrentDictionary<ulong, GlobalGuildAccount>();
            }
        }
        //TODO: change all instances of currency to custom currency name
        internal static GlobalGuildAccount GetGuildAccount(ulong id)
        {
            return serverAccounts.GetOrAdd(id, (key) =>
            {
                var newAccount = new GlobalGuildAccount { Id = id, LevelingMsgs = "server", Currency = "Taiyakis"};
                Configuration.DataStorage.StoreObject(newAccount, Path.Combine(Constants.ServerAccountsFolder, $"{id}.json"), useIndentations: true);
                return newAccount;
            });
        }

        internal static GlobalGuildAccount GetGuildAccount(IGuild guild)
        {
            return GetGuildAccount(guild.Id);
        }

        /// <summary>
        /// This rewrites ALL ServerAccounts to the harddrive... Strongly recommend to use SaveAccounts(id1, id2, id3...) where possible instead
        /// </summary>
        internal static void SaveAllAccounts()
        {
            foreach (var id in serverAccounts.Keys)
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
                Configuration.DataStorage.StoreObject(GetGuildAccount(id), Path.Combine(Constants.ServerAccountsFolder, $"{id}.json"), useIndentations: true);
            }
        }
    }
}
