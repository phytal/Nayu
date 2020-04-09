using System;
using System.IO;
using Newtonsoft.Json;

namespace Nayu.Core.Configuration
{
    class DataStorage
    {
        private static readonly string resourcesFolder = Constants.ResourceFolder;

        static DataStorage()
        {
            if (!Directory.Exists(resourcesFolder))
            {
                Directory.CreateDirectory(resourcesFolder);
            }
        }

        internal static void StoreObject(object obj, string file, Formatting formatting)
        {
            string json = JsonConvert.SerializeObject(obj, formatting);
            string filePath = String.Concat(resourcesFolder, "/", file);
            File.WriteAllText(filePath, json);
        }

        internal static void StoreObject(object obj, string file, bool useIndentations)
        {
            var formatting = (useIndentations) ? Formatting.Indented : Formatting.None;
            StoreObject(obj, file, formatting);
        }

        internal static T RestoreObject<T>(string file)
        {
            string json = GetOrCreateFileContents(file);
            return JsonConvert.DeserializeObject<T>(json);
        }

        internal static bool LocalFileExists(string file)
        {
            string filePath = String.Concat(resourcesFolder, "/", file);
            return File.Exists(filePath);
        }

        private static string GetOrCreateFileContents(string file)
        {
            string filePath = String.Concat(resourcesFolder, "/", file);
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "");
                return "";
            }

            return File.ReadAllText(filePath);
        }
    }
}