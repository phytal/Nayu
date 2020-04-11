using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord.WebSocket;
using MongoDB.Driver;
using Nayu.Core.Configuration;
using Nayu.Core.Entities;
using Nayu.Helpers;

namespace Nayu.Core.Features.GlobalAccounts
{
    internal static class GlobalGuildUserAccounts
    {
        private static SocketGuildUser user;
        private static readonly ConcurrentDictionary<string, GlobalGuildUserAccount> GuildUserAccounts = new ConcurrentDictionary<string, GlobalGuildUserAccount>();
        static GlobalGuildUserAccounts()
        {
            MongoHelper.ConnectToMongoService();
            MongoHelper.GuildUserCollection = MongoHelper.Database.GetCollection<GlobalGuildUserAccount>("GuildUsers");
            var filter = Builders<GlobalGuildUserAccount>.Filter.Ne("ID", "");
            var results = MongoHelper.GuildUserCollection.Find(filter).ToList();
            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    var guildUser = DataStorage.RestoreObject(CollectionType.GuildUser, result.Id) as GlobalGuildUserAccount;
                    GuildUserAccounts.TryAdd(guildUser.Id.ToString(), guildUser);
                }
            }
            else {
                GuildUserAccounts = new ConcurrentDictionary<string, GlobalGuildUserAccount>();
            }
        }

        internal static GlobalGuildUserAccount GetUserID(string id, ulong nid)
        {
            return GuildUserAccounts.GetOrAdd(id, (key) =>
            {
                var newAccount = new GlobalGuildUserAccount { UniqueId = id , Id = nid};
                DataStorage.StoreObject(newAccount, CollectionType.GuildUser, id);
                return newAccount;
            });
        }

        internal static GlobalGuildUserAccount GetUserID(SocketGuildUser user)
        {
            return GetUserID($"{user.Guild.Id}{user.Id}", user.Id);
        }


        internal static List<GlobalGuildUserAccount> GetAllAccounts()
        {
            return GuildUserAccounts.Values.ToList();
        }


        internal static List<GlobalGuildUserAccount> GetFilteredAccounts(Func<GlobalGuildUserAccount, bool> filter)
        {
            return GuildUserAccounts.Values.Where(filter).ToList();
        }

        internal static void SaveAccount(string uId, ulong id)
        {
            DataStorage.StoreObject(GetUserID(uId, id), CollectionType.GuildUser, uId);

        }
        /// <summary>
        /// This rewrites ALL UserAccounts to the harddrive... Strongly recommend to use SaveAccounts(id1, id2, id3...) where possible instead
        /// </summary>
        internal static void SaveAccounts()
        {
            foreach (var userAcc in GuildUserAccounts.Values)
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
                DataStorage.StoreObject(GetUserID(id, user.Id), CollectionType.GuildUser, id);
            }
        }
    }
}
