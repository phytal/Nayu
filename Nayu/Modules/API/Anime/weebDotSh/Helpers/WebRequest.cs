using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nayu.Core.Configuration;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;

namespace Nayu.Modules.API.Anime.weebDotSh.Helpers
{
    public class WebRequest
    {
        WeebClient weebClient = new WeebClient("Nayu", Config.bot.version);

        public async Task<RandomData> GetTypesAsync(string type, IEnumerable<string> tags, FileType fileType,
            NsfwSearch nsfw, bool hidden)
        {
            await weebClient.Authenticate(Config.bot.wolkeToken, TokenType.Wolke);
            var result =
                await weebClient.GetRandomAsync(type, tags, fileType, hidden,
                    nsfw); //hidden and nsfw are always defaulted to false

            if (result == null)
            {
                return null;
            }

            return result;


        }
    }
}
