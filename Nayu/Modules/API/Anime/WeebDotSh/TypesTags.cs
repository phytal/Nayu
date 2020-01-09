using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Configuration;
using Nayu.Libs.Weeb.net;
using Nayu.Libs.Weeb.net.Data;
using Nayu.Preconditions;
using TokenType = Nayu.Libs.Weeb.net.TokenType;

namespace Nayu.Modules.API.Anime.WeebDotSh
{
    public class TypesTags : NayuModule
    {
        WeebClient weebClient = new WeebClient("Nayu", Config.bot.version);
        public async Task<TagsData> GetTagsAsync(bool hidden)
        {
            return await weebClient.GetTagsAsync(hidden); //hidden is always defaulted to false
        }

        public async Task<TypesData> GetTypessAsync(bool hidden)
        {
            return await weebClient.GetTypesAsync(hidden); //hidden is always defaulted to false
        }

        [Command("weebtags")]
        [Summary("Displays all possible tags for weeb.sh")]
        [Remarks("Ex: n!ceebtags")]
        [Cooldown(5)]
        public async Task WeebTags(IGuildUser user = null)
        {
            await weebClient.Authenticate(Config.bot.wolkeToken, TokenType.Wolke);
            var tags = await GetTagsAsync(false);
            List<string> tagList = tags.Tags;
            await SendMessage(Context, null, String.Join(", ", tagList));

        }

        [Command("weebtypes")]
        [Summary("Displays all possible types for weeb.sh")]
        [Remarks("Ex: n!ceebtypes")]
        [Cooldown(5)]
        public async Task WeebTypes(IGuildUser user = null)
        {
            await weebClient.Authenticate(Config.bot.wolkeToken, TokenType.Wolke);
            var tags = await GetTypessAsync(false);
            List<string> tagList = tags.Types;
            await SendMessage(Context, null, String.Join(", ", tagList));

        }
    }
}
