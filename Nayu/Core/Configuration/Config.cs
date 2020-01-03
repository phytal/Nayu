using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

namespace Nayu.Core.Configuration
{
    class Config
    {
        private const string configFolder = "resources";
        private const string configFile = "config.json";

        public static BotConfig bot;

        static Config()
        {
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

            if (!File.Exists(configFolder + "/" + configFile))
            {
                bot = new BotConfig{token = Environment.GetEnvironmentVariable("Token"), wolkeToken = Environment.GetEnvironmentVariable("WolkeToken"), cmdPrefix = "n!", twitchStreamer = "phytal"};
                bot.token = "";
                string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + configFile, json);
            }
            else
            {
                string json = File.ReadAllText(configFolder + "/" + configFile);
                bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
        }
    }

    public struct BotConfig
    {
        public string token;
        public string cmdPrefix;
        public string botGameToSet;
        public string twitchStreamer;
        public string version;
        public string wolkeToken;
        public string dblToken;
    }
}

