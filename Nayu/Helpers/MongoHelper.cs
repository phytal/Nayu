using System;
using MongoDB.Driver;
using Nayu.Core.Configuration;
using Nayu.Core.Entities;

namespace Nayu.Helpers
{
    public class MongoHelper
    {
        public static IMongoClient Client { get; set; }
        public static IMongoDatabase Database { get; private set; }
        
        public static IMongoCollection<GlobalUserAccount> UserCollection { get; set; }
        public static IMongoCollection<GlobalGuildAccount> GuildCollection { get; set; }
        public static IMongoCollection<GlobalGuildUserAccount> GuildUserCollection { get; set; }
        public static IMongoCollection<BotAccount> BotCollection { get; set; }
        
        internal static void ConnectToMongoService()
        {
            try
            {
                Client = new MongoClient(Config.bot.mongoConnection);
                Database = Client.GetDatabase("Nayu");
            }
            catch(MongoException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}