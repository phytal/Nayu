using System.Collections.Concurrent;
using System.IO;
using MongoDB.Driver;
using Nayu.Core.Configuration;
using Nayu.Core.Entities;
using Nayu.Helpers;

namespace Nayu.Core.Features.GlobalAccounts
{
    internal static class BotAccounts
    {
        private static readonly ConcurrentDictionary<ulong, BotAccount> BotAccount = new ConcurrentDictionary<ulong, BotAccount>();
        private static readonly ulong NayuId = 598335076689772554;
        static BotAccounts()
        {
            MongoHelper.ConnectToMongoService();
            MongoHelper.BotCollection = MongoHelper.Database.GetCollection<BotAccount>("Bot");
            var filter = Builders<BotAccount>.Filter.Eq("Id", NayuId);
            var result = MongoHelper.BotCollection.Find(filter).FirstOrDefault();
            if (result != null)
            {
                var bot = DataStorage.RestoreObject(CollectionType.Bot, NayuId) as BotAccount;
                BotAccount.TryAdd(bot.Id, bot);
            }
            else
            {
                BotAccount = new ConcurrentDictionary<ulong, BotAccount>();
            }
        }

         internal static BotAccount GetAccount()
         {
             return BotAccount.GetOrAdd(NayuId, (key) =>
             {
                 var newAccount = new BotAccount { Id = NayuId };
                 DataStorage.StoreObject(newAccount, CollectionType.Bot, NayuId);
                 return newAccount;
             });
         }

        internal static void SaveAccounts()
        {
            DataStorage.StoreObject(GetAccount(), CollectionType.Bot, NayuId);
        }
    }
}
