using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weeb.net.Data;

namespace Weeb.net
{   
    public class WeebClient
    {
        private WeebHttp _weebHttp;
        private string _userAgent;
        internal const string WeebNetVersion = "1.0.7";

        public WeebClient(string BotName = null, string BotVersion = null){
            //prevent spammy user-agents
            if(string.IsNullOrWhiteSpace(BotName)|| (BotName.Length > 70 || BotVersion.Length > 20)){
                _userAgent = $"Weeb.Net/{WeebNetVersion}";
            } else {
                if(string.IsNullOrWhiteSpace(BotVersion)){
                    _userAgent = $"{BotName}/1.0.0";
                } else {
                    _userAgent = $"{BotName}/{BotVersion}";
                }
            }
        }

        //https://docs.weeb.sh/

        /// <summary>
        /// Authenticate the client to the Api withrow shit at us like in GOTth your token also choosing which Type of token to use. This step must be done before attempting to use the api!
        /// </summary>
        /// <param name="token">Your Token</param>
        /// <param name="type">Type of Token Bearer/Wolke</param>
        public async Task Authenticate(string token, TokenType type)
        {
            _weebHttp = new WeebHttp(token, type, _userAgent);
            var welcome = await _weebHttp.Welcome();
            Console.WriteLine($"Connected to Weeb Api version: {welcome.Version}\nUsing Weeb.net wrapper {WeebNetVersion}");
        }

        
        /// <summary>
        /// Get an array of available tags
        /// </summary>
        /// <param name="hidden">If it is only visible for the creator of weeb.sh</param>
        /// <returns>All types, null if authentication fails</returns>
        public async Task<TypesData> GetTypesAsync(bool hidden = false)
        {
            return await _weebHttp.GetTypes(hidden);
        }

        /// <summary>
        /// Get an array of available tags
        /// </summary>
        /// <param name="hidden">If it is only visible for the creator of weeb.sh</param>
        /// <returns>All tags, null if authentication fails</returns>
        public async Task<TagsData> GetTagsAsync(bool hidden = false)
        {
            return await _weebHttp.GetTags(hidden);
        }
        
        /// <summary>
        /// Gets a random image with the specified tags and type. Either tag or type is mandatory
        /// </summary>
        /// <param name="type">type of image</param>
        /// <param name="tags">tags the image should contain</param>
        /// <param name="fileType">Specify which filetpye you want. Default to Any</param>
        /// <param name="hidden">Only visible to the creator of weeb.sh</param>
        /// <param name="nsfw">Is nsfw</param>
        /// <returns>Random image or null if none found or authentication fails</returns>
        public async Task<RandomData> GetRandomAsync(string type, IEnumerable<string> tags, FileType fileType = FileType.Any, bool hidden = false, NsfwSearch nsfw = NsfwSearch.False)
        {
            string finalTags = string.Join(",",tags);
           
            //Need at least type or tags
            if (string.IsNullOrWhiteSpace(type) && string.IsNullOrWhiteSpace(finalTags))
                return null;
            return await _weebHttp.GetRandomImage(type, finalTags, hidden, nsfw, fileType);
        }
    }
}
