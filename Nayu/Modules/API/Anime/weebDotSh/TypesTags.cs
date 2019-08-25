using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weeb.net;
using Weeb.net.Data;
using Nayu.Core.Modules;
using Nayu.Preconditions;

namespace Nayu.Modules.API.weebDotSh
{
    public class TypesTags : NayuModule
    {
        WeebClient weebClient = new WeebClient("Nayu", Config.bot.Version);
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
            await weebClient.Authenticate(Config.bot.wolkeToken, Weeb.net.TokenType.Wolke);
            var tags = await GetTagsAsync(false);
            List<string> tagList = tags.Tags;
            await Context.Channel.SendMessageAsync(String.Join(", ", tagList));

        }

        [Command("weebtypes")]
        [Summary("Displays all possible types for weeb.sh")]
        [Remarks("Ex: n!ceebtypes")]
        [Cooldown(5)]
        public async Task WeebTypes(IGuildUser user = null)
        {
            await weebClient.Authenticate(Config.bot.wolkeToken, Weeb.net.TokenType.Wolke);
            var tags = await GetTypessAsync(false);
            List<string> tagList = tags.Types;
            await Context.Channel.SendMessageAsync(String.Join(", ", tagList));

        }
    }
}
