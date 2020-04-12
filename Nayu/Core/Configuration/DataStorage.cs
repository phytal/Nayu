using System;
using System.IO;
using System.Linq;
using MongoDB.Driver;
using Nayu.Core.Entities;
using Nayu.Helpers;
using Newtonsoft.Json;

namespace Nayu.Core.Configuration
{
    class DataStorage
    {
        private static readonly string ResourcesFolder = Constants.ResourceFolder;

        internal static void StoreObject(object obj, CollectionType type, object id)
        {
            MongoHelper.ConnectToMongoService();
            UpdateLocalCollection(type);
            InsertCollection(type, obj);
            UpdateMongoCollection(type, obj, id);
        }

        /// <summary>
        /// Retrieves .type. json from MongoDB.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static T RestoreObject<T>(CollectionType type, object id)
        {
            return GetOrCreateFileContents(type, id) as dynamic;
        }

        internal static bool LocalFileExists(string file)
        {
            string filePath = String.Concat(ResourcesFolder, "/", file);
            return File.Exists(filePath);
        }

        private static object GetOrCreateFileContents(CollectionType type, object id)
        {
            MongoHelper.ConnectToMongoService();
            UpdateLocalCollection(type);
            object result = type switch
            {
                CollectionType.User =>
                MongoHelper.UserCollection.Find(Builders<GlobalUserAccount>.Filter.Eq("Id", id)).ToList()
                    .FirstOrDefault(),
                CollectionType.Guild =>
                MongoHelper.GuildCollection.Find(Builders<GlobalGuildAccount>.Filter.Eq("Id", id)).ToList()
                    .FirstOrDefault(),
                CollectionType.GuildUser =>
                MongoHelper.GuildUserCollection.Find(Builders<GlobalGuildUserAccount>.Filter.Eq("Id", id)).ToList()
                    .FirstOrDefault(),
                CollectionType.Bot =>
                MongoHelper.BotCollection.Find(Builders<BotAccount>.Filter.Eq("Id", id)).ToList().FirstOrDefault(),
                _ => throw new Exception("Data Storage - Invalid Collection Type or Id was entered.")
            };

            if (result != null) return result;

            switch (type)
            {
                case CollectionType.User:
                    MongoHelper.UserCollection.InsertOneAsync(null);
                    break;
                case CollectionType.Guild:
                    MongoHelper.GuildCollection.InsertOneAsync(null);
                    break;
                case CollectionType.GuildUser:
                    MongoHelper.GuildUserCollection.InsertOneAsync(null);
                    break;
                case CollectionType.Bot:
                    MongoHelper.BotCollection.InsertOneAsync(null);
                    break;
                default:
                    throw new Exception("Data Storage - Invalid Collection Type was entered.");
            }

            return "";
        }

        private static void InsertCollection(CollectionType type, object obj)
        {
            switch (type)
            {
                case CollectionType.User:
                    MongoHelper.UserCollection.InsertOneAsync(obj as GlobalUserAccount);
                    break;
                case CollectionType.Guild:
                    MongoHelper.GuildCollection.InsertOneAsync(obj as GlobalGuildAccount);
                    break;
                case CollectionType.GuildUser:
                    MongoHelper.GuildUserCollection.InsertOneAsync(obj as GlobalGuildUserAccount);
                    break;
                case CollectionType.Bot:
                    MongoHelper.BotCollection.InsertOneAsync(obj as BotAccount);
                    break;
                default:
                    throw new Exception("Data Storage - Invalid Collection Type or object was entered.");
            }
        }

        private static void UpdateLocalCollection(CollectionType type)
        {
            switch (type)
            {
                case CollectionType.User:
                    MongoHelper.UserCollection = MongoHelper.Database.GetCollection<GlobalUserAccount>("Users");
                    break;
                case CollectionType.Guild:
                    MongoHelper.GuildCollection = MongoHelper.Database.GetCollection<GlobalGuildAccount>("Guilds");
                    break;
                case CollectionType.GuildUser:
                    MongoHelper.GuildUserCollection =
                        MongoHelper.Database.GetCollection<GlobalGuildUserAccount>("GuildUsers");
                    break;
                case CollectionType.Bot:
                    MongoHelper.BotCollection = MongoHelper.Database.GetCollection<BotAccount>("Bot");
                    break;
                default:
                    throw new Exception("Data Storage - Invalid Collection Type was entered.");
            }
        }

        private static void UpdateMongoCollection(CollectionType type, object obj, object id)
        {
            switch (type)
            {
                case CollectionType.User:
                    MongoHelper.UserCollection.ReplaceOneAsync(Builders<GlobalUserAccount>.Filter.Eq("Id", id),
                        obj as GlobalUserAccount);
                    break;
                case CollectionType.Guild:
                    MongoHelper.GuildCollection.ReplaceOneAsync(Builders<GlobalGuildAccount>.Filter.Eq("Id", id),
                        obj as GlobalGuildAccount);
                    break;
                case CollectionType.GuildUser:
                    MongoHelper.GuildUserCollection.ReplaceOneAsync(
                        Builders<GlobalGuildUserAccount>.Filter.Eq("Id", id), obj as GlobalGuildUserAccount);
                    break;
                case CollectionType.Bot:
                    MongoHelper.BotCollection.ReplaceOneAsync(Builders<BotAccount>.Filter.Eq("Id", id),
                        obj as BotAccount);
                    break;
                default:
                    throw new Exception("Data Storage - Invalid Collection Type or object was entered.");
            }
        }
    }

    public enum CollectionType
    {
        User,
        Guild,
        GuildUser,
        Bot
    }
}