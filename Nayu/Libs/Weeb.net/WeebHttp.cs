using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Weeb.net.Data;

namespace Weeb.net
{
    
    public enum FileType
    {
        Jpg,
        Png,
        Gif,
        Any
    };

    public enum NsfwSearch
    {
        False,
        True,
        Only
    }

    public enum TokenType
    {
        Bearer,
        Wolke
    }
    
    internal class WeebHttp
    {
        private const string BaseUrl = "https://api.weeb.sh/";
        private string _token;
        private readonly HttpClient _client;

        /// <summary>
        /// Initialize the Weeb.api wrapper using your API token
        /// </summary>
        /// <param name="token">Your weeb api token</param>
        public WeebHttp(string token, TokenType type, string UserAgent)
        {
            _token = token;
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = true
            };
            _client = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseUrl),
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            string tokenType ="Wolke";
            switch (type)
            {
                    case TokenType.Bearer:
                        tokenType = "Bearer";
                        break;
                    case TokenType.Wolke:
                        tokenType = "Wolke";
                        break;
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
        }

        internal async Task<WelcomeData> Welcome()
        {
            //get result
            var result = await _client.GetStringAsync(Endpoints.Images);
            //convert from json to usable c# objects
            return JsonConvert.DeserializeObject<WelcomeData>(result);
        }

        internal async Task<TypesData> GetTypes(bool hidden)
        {
            string result;
            try
            {
                result = await _client.GetStringAsync(Endpoints.Types+$"?hidden={hidden}");
            }
            catch (HttpRequestException)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<TypesData>(result);
        }

        internal async Task<TagsData> GetTags(bool hidden)
        {
            string result;
            try
            {
                result = await _client.GetStringAsync(Endpoints.Tags+$"?hidden={hidden}");
            }
            catch (HttpRequestException)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<TagsData>(result);
        }

        internal string GetNsfwOption(NsfwSearch search)
        {
            string nsfw = "";
            switch (search)
            {
                case NsfwSearch.False:
                    nsfw = "false";
                    break;
                case NsfwSearch.True:
                    nsfw = "true";
                    break;
                case NsfwSearch.Only:
                    nsfw = "only";
                    break;
            }
            return nsfw;
        }

        internal string GetFiletypeExtension(FileType type)
        {
            string fileExtension = "";
            switch (type)
            {
                case FileType.Gif:
                    fileExtension = "gif";
                    break;
                case FileType.Jpg:
                    fileExtension = "jpg";
                    break;
                case FileType.Png:
                    fileExtension = "png";
                    break;
                    default:
                        fileExtension = null;
                        break;
            }
            return fileExtension;
        }

        internal async Task<RandomData> GetRandomImage(string type, string tags, bool hidden, NsfwSearch nsfw, FileType fileType)
        {
            string query = "";
            if (!string.IsNullOrWhiteSpace(type))
                query += $"&type={type}";
            if (!string.IsNullOrWhiteSpace(tags))
                query += $"&tags={tags}";
            string nsfwS = GetNsfwOption(nsfw);
            query += $"&hidden={hidden.ToString().ToLower()}&nsfw={nsfwS}";
            if (fileType != FileType.Any)
            {
                string fileExtension = GetFiletypeExtension(fileType);
                if (!string.IsNullOrWhiteSpace(fileExtension))
                    query += $"&filetype={fileExtension}";
            }
            query = query.Substring(1);
            query = "?" + query;
            string result;
            try
            {
                result = await _client.GetStringAsync(Endpoints.Random + query);
            }
            catch (HttpRequestException)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<RandomData>(result);
        }
    }
}