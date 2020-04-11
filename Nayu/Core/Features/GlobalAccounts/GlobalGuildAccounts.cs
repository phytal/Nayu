using System.Collections.Concurrent;
using System.IO;
using Discord;
using MongoDB.Driver;
using Nayu.Core.Configuration;
using Nayu.Core.Entities;
using Nayu.Helpers;

namespace Nayu.Core.Features.GlobalAccounts
{
    internal static class GlobalGuildAccounts
    {
        private static readonly ConcurrentDictionary<ulong, GlobalGuildAccount> GuildAccounts = new ConcurrentDictionary<ulong, GlobalGuildAccount>();

        static GlobalGuildAccounts()
        {
            MongoHelper.ConnectToMongoService();
            MongoHelper.GuildCollection = MongoHelper.Database.GetCollection<GlobalGuildAccount>("Guilds");
            var filter = Builders<GlobalGuildAccount>.Filter.Ne("ID", "");
            var results = MongoHelper.GuildCollection.Find(filter).ToList();
            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    var guild = DataStorage.RestoreObject(CollectionType.Guild, result.Id) as GlobalGuildAccount;
                    GuildAccounts.TryAdd(guild.Id, guild);
                }
            }
            else
            {
                GuildAccounts = new ConcurrentDictionary<ulong, GlobalGuildAccount>();
            }
        }

        internal static GlobalGuildAccount GetGuildAccount(ulong id)
        {
            return GuildAccounts.GetOrAdd(id, (key) =>
            {
                var newAccount = new GlobalGuildAccount { Id = id, LevelingMsgs = "server", Currency = "Taiyakis"};
                DataStorage.StoreObject(newAccount, CollectionType.Guild, id);
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
            foreach (var id in GuildAccounts.Keys)
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
                DataStorage.StoreObject(GetGuildAccount(id), CollectionType.Guild, id);
            }
        }
    }
}
