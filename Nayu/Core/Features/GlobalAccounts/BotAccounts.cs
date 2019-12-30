using System.Collections.Concurrent;
using System.IO;
using Nayu.Core.Configuration;
using Nayu.Core.Entities;

namespace Nayu.Core.Features.GlobalAccounts
{
    internal static class BotAccounts
    {
        private static readonly ConcurrentDictionary<string, BotAccount> botAccounts = new ConcurrentDictionary<string, BotAccount>();

        static BotAccounts()
        {
            var info = Directory.CreateDirectory(Constants.ResourceFolder);
            var files = info.GetFiles("Nayu.json");
            if (files.Length == 1)
            {
                var bot = DataStorage.RestoreObject<BotAccount>("Nayu.json");
                botAccounts.TryAdd("Nayu", bot);
            }
            else
            {
                botAccounts = new ConcurrentDictionary<string, BotAccount>();
            }
        }

         internal static BotAccount GetAccount()
         {
             return botAccounts.GetOrAdd("Nayu", (key) =>
             {
                 var newAccount = new BotAccount { };
                 DataStorage.StoreObject(newAccount, "Nayu.json", useIndentations: true);
                 return newAccount;
             });
         }

        internal static void SaveAccounts()
        {
            DataStorage.StoreObject(GetAccount(), "Nayu.json", useIndentations: true);
        }
    }
}
