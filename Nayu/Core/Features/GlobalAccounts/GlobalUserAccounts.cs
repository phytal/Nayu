using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord;
using MongoDB.Driver;
using Nayu.Core.Configuration;
using Nayu.Core.Entities;
using Nayu.Helpers;
using Nayu.Modules.Chomusuke;
using Type = Nayu.Modules.Chomusuke.Dueling.Enums.Type;

namespace Nayu.Core.Features.GlobalAccounts
{
    internal static class GlobalUserAccounts
    {
        private static readonly ConcurrentDictionary<ulong, GlobalUserAccount> UserAccounts = new ConcurrentDictionary<ulong, GlobalUserAccount>();

        static GlobalUserAccounts()
        {
            MongoHelper.ConnectToMongoService();
            MongoHelper.UserCollection = MongoHelper.Database.GetCollection<GlobalUserAccount>("Users");
            var filter = Builders<GlobalUserAccount>.Filter.Ne("_id", "");
            var results = MongoHelper.UserCollection.Find(filter);
            if (results.CountDocuments() < 1)
                UserAccounts = new ConcurrentDictionary<ulong, GlobalUserAccount>();
            else
            {
                foreach (var result in results.ToList())
                {
                    var user = DataStorage.RestoreObject<GlobalUserAccount>(CollectionType.User, result.Id);
                    UserAccounts.TryAdd(user.Id, user);
                }
            }
        }
        
         internal static GlobalUserAccount GetUserAccount(ulong id)
         {
             return UserAccounts.GetOrAdd(id, (key) =>
             {
                 var newAccount = new GlobalUserAccount { Id = id, Title = "Adventurer", Chomusuke1 = new Chomusuke(false, null, null, false, 0, 0, 0, 0, false, null, null, null, null, Type.None, 0, Trait.None, Trait.None, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, DateTime.MinValue,null, null, null), 
                     Chomusuke2 = new Chomusuke(false, null, null, false, 0, 0, 0, 0, false, null, null, null, null, Type.None, 0, Trait.None, Trait.None, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, DateTime.MinValue,null, null, null), 
                     Chomusuke3 = new Chomusuke(false, null, null, false, 0, 0, 0, 0, false, null, null, null, null, Type.None, 0, Trait.None, Trait.None, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, DateTime.MinValue, null, null, null)
             };
                 DataStorage.StoreObject(newAccount, CollectionType.User, id);
                 return newAccount;
             });
         }
         
         

        internal static GlobalUserAccount GetUserAccount(IUser user)
        {
            return GetUserAccount(user.Id);
        }

        internal static List<GlobalUserAccount> GetAllAccounts()
        {
            return UserAccounts.Values.ToList();
        }


        internal static List<GlobalUserAccount> GetFilteredAccounts(Func<GlobalUserAccount, bool> filter)
        {
            return UserAccounts.Values.Where(filter).ToList();
        }
        

        /// <summary>
        /// This rewrites ALL UserAccounts to the harddrive... Strongly recommend to use SaveAccounts(id1, id2, id3...) where possible instead
        /// </summary>
        internal static void SaveAllAccounts()
        {
            foreach (var id in UserAccounts.Keys)
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
                DataStorage.StoreObject(GetUserAccount(id), CollectionType.User, id);
            }
        }
    }
}
