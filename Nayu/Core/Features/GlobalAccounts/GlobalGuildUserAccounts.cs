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
        private static readonly ConcurrentDictionary<string, GlobalGuildUserAccount> GuildUserAccounts = new ConcurrentDictionary<string, GlobalGuildUserAccount>();
        static GlobalGuildUserAccounts()
        {
            MongoHelper.ConnectToMongoService();
            MongoHelper.GuildUserCollection = MongoHelper.Database.GetCollection<GlobalGuildUserAccount>("GuildUsers");
            var filter = Builders<GlobalGuildUserAccount>.Filter.Ne("_id", "");
            var results = MongoHelper.GuildUserCollection.Find(filter);
            if (results.CountDocuments() < 1)
                GuildUserAccounts = new ConcurrentDictionary<string, GlobalGuildUserAccount>();
            else
            {
                foreach (var result in results.ToList())
                {
                    var guildUser =
                        DataStorage.RestoreObject<GlobalGuildUserAccount>(CollectionType.GuildUser, result.Id);
                    GuildUserAccounts.TryAdd(guildUser.Id, guildUser);
                }
            }
        }

        private static GlobalGuildUserAccount GetUserId(string id, ulong nid)
        {
            return GuildUserAccounts.GetOrAdd(id, (key) =>
            {
                var newAccount = new GlobalGuildUserAccount { Id = id , UserId = nid};
                DataStorage.StoreObject(newAccount, CollectionType.GuildUser, id);
                return newAccount;
            });
        }

        internal static GlobalGuildUserAccount GetUserId(SocketGuildUser user)
        {
            return GetUserId($"{user.Guild.Id}{user.Id}", user.Id);
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
            DataStorage.StoreObject(GetUserId(uId, id), CollectionType.GuildUser, uId);

        }
        /// <summary>
        /// This rewrites ALL UserAccounts to the harddrive... Strongly recommend to use SaveAccounts(id1, id2, id3...) where possible instead
        /// </summary>
        internal static void SaveAccounts()
        {
            foreach (var userAcc in GuildUserAccounts.Values)
            {
                SaveAccount(userAcc.Id, userAcc.UserId);
            }
        }

        /// <summary>
        /// Saves one or multiple Accounts by provided Ids
        /// </summary>
        internal static void SaveAccounts(params SocketGuildUser[] users)
        {
            foreach (var user in users)
            {
                DataStorage.StoreObject(GetUserId(user), CollectionType.GuildUser, $"{user.Guild.Id}{user.Id}");
            }
        }
    }
}
