using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using Nayu.Core.Entities;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules
{
    public class ServerTags : NayuModule
    {
        [Subject(AdminCategories.ServerTags)]
        [Command("tag"), Summary("Let the bot send a message with the content of the named tag on the server")]
        [Remarks("n!tag <tag name> Ex: n!tag door")]
        [Cooldown(5)]
        public async Task ShowTag(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                
                var description = "You need to use this with some more input...\n" +
                                 "Try the `n!command tag` command to get more information on how to use this command.";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error!", description, EmbedHandler.EmbedMessageType.Exception, false);
                await SendMessage(Context, errorEmbed);
                return;
            }
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var response = TagFunctions.GetTag(tagName, guildAcc);
            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(AdminCategories.ServerTags)]
        [Command("tagNew"), Alias("add"), Summary("Adds a new (not yet existing) tag to the server")]
        [Remarks("n!tagNew <tag content> Ex: n!tag :door: Get out.")]
        [Cooldown(5)]
        public async Task AddTag(string tagName, [Remainder] string tagContent)
        {
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            //tagContent.Replace("@everyone", "@\u200beveryone").Replace("@here", "@\u200bhere");
            var response = TagFunctions.AddTag(tagName, tagContent, guildAcc);
            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(AdminCategories.ServerTags)]
        [Command("tagUpdate"), Summary("Updates the content of an existing tag of the server")]
        [Remarks("n!tagUpdate <tag name> <tag content> Ex: n!tag update door :door: Get in.")]
        [Cooldown(5)]
        public async Task UpdateTag(string tagName, [Remainder] string tagContent)
        {
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            //tagContent.Replace("@everyone", "@\u200beveryone").Replace("@here", "@\u200bhere");
            var response = TagFunctions.UpdateTag(tagName, tagContent, guildAcc);
            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(AdminCategories.ServerTags)]
        [Command("tagRemove"), Summary("Removes a tag off the server")]
        [Remarks("n!tagRemove <tag name> Ex: n!tag remove door")]
        [Cooldown(5)]
        public async Task RemoveTag(string tagName)
        {
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var response = TagFunctions.RemoveTag(tagName, guildAcc);
            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(AdminCategories.ServerTags)]
        [Command("tagList"), Summary("Show all tag on this server")]
        [Remarks("Ex: n!tagList")]
        [Cooldown(5)]
        public async Task ListTags()
        {
            var guildAcc = GlobalGuildAccounts.GetGuildAccount(Context.Guild.Id);
            var emb = TagFunctions.BuildTagListEmbed(guildAcc);
            await SendMessage(Context, emb);
        }
    }
    
    public class PersonalTags : NayuModule
    {
        [Subject(Categories.PersonalTags)]
        [Command("ptag"), Priority(-1), Summary("Lets the bot send a message with the content of your named tag")]
        [Remarks("n!ptag <tag name> Ex: n!ptag door")]
        [Cooldown(5)]
        public async Task ShowTag(string tagName = "")
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                var description = "You need to use this with some more input...\n" +
                                 "Try the `help ptag` command to get more information on how to use this command.";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error!", description, EmbedHandler.EmbedMessageType.Error, false);
                await SendMessage(Context, errorEmbed);
                return;
            }
            var userAcc = GlobalUserAccounts.GetUserAccount(Context.User.Id);
            var response = TagFunctions.GetTag(tagName, userAcc);
            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(Categories.PersonalTags)]
        [Command("ptagNew"), Alias("add"), Summary("Adds a new (not yet existing) tag to your collection")]
        [Remarks("n!ptagNew <tag content> Ex: n!ptag door :door: Get out.")]
        [Cooldown(5)]
        public async Task AddTag(string tagName, [Remainder] string tagContent)
        {
            var userAcc = GlobalUserAccounts.GetUserAccount(Context.User.Id);
            tagContent.Replace("@everyone", "@\u200beveryone").Replace("@here", "@\u200bhere");
            var response = TagFunctions.AddTag(tagName, tagContent, userAcc);
            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(Categories.PersonalTags)]
        [Command("ptagUpdate"), Summary("Updates an existing tag of yours")]
        [Remarks("n!ptagUpdate <tag name> <tag content> Ex: n!ptag update door :door: Get in.")]
        [Cooldown(5)]
        public async Task UpdateTag(string tagName, [Remainder] string tagContent)
        {
            var userAcc = GlobalUserAccounts.GetUserAccount(Context.User.Id);
            tagContent.Replace("@everyone", "@\u200beveryone").Replace("@here", "@\u200bhere");
            var response = TagFunctions.UpdateTag(tagName, tagContent, userAcc);
            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }
        
        [Subject(Categories.PersonalTags)]
        [Command("ptagRemove"), Summary("Removes an existing tag of yours")]
        [Remarks("n!ptagRemove <tag name> Ex: n!ptag remove door")]
        [Cooldown(5)]
        public async Task RemoveTag(string tagName)
        {
            var userAcc = GlobalUserAccounts.GetUserAccount(Context.User.Id);
            var response = TagFunctions.RemoveTag(tagName, userAcc);
            var embed = EmbedHandler.CreateEmbed(Context, "Success!", response, EmbedHandler.EmbedMessageType.Success, false);
            await SendMessage(Context, embed);
        }

        [Subject(Categories.PersonalTags)]
        [Command("ptagList"), Summary("Show all your tags")]
        [Remarks("Ex: n!ptagList")]
        [Cooldown(5)]
        public async Task ListTags()
        {
            var userAcc = GlobalUserAccounts.GetUserAccount(Context.User.Id);
            var emb = TagFunctions.BuildTagListEmbed(userAcc);
            await SendMessage(Context, emb);
        }
    }


    internal static class TagFunctions
    {
        internal static string AddTag(string tagName, string tagContent, IGlobalAccount account)
        {
            var response = "A tag with that name already exists!\n" +
                           "If you want to override it use `update <tagName> <tagContent>`";
            if (account.Tags.ContainsKey(tagName)) return response;
            account.Tags.Add(tagName, tagContent);
            if (account is GlobalGuildAccount)
                GlobalGuildAccounts.SaveAccounts(account.Id);
            else GlobalUserAccounts.SaveAccounts(account.Id);
            response = $"Successfully added tag `{tagName}`.";

            return response;
        }

        internal static Embed BuildTagListEmbed(IGlobalAccount account)
        {
            var tags = account.Tags;
            var embed = new EmbedBuilder().WithTitle("No tags set up yet... add some!");
            if (tags.Count > 0) embed.WithTitle("Here are all available tags:");
            embed.WithColor(Global.NayuColor);
            foreach (var tag in tags)
            {
                embed.AddField(tag.Key, tag.Value, true);
            }

            return embed.Build();
        }

        internal static string GetTag(string tagName, IGlobalAccount account)
        {
            if (account.Tags.ContainsKey(tagName))
                return account.Tags[tagName];
            return "A tag with that name doesn't exists!";
        }

        internal static string RemoveTag(string tagName, IGlobalAccount account)
        {
            if (account.Tags.ContainsKey(tagName) == false)
                return "You can't remove a tag that doesn't exist...";

            account.Tags.Remove(tagName);
            if (account is GlobalGuildAccount)
                GlobalGuildAccounts.SaveAccounts(account.Id);
            else GlobalUserAccounts.SaveAccounts(account.Id);

            return $"Successfully removed the tag {tagName}!";
        }

        internal static string UpdateTag(string tagName, string tagContent, IGlobalAccount account)
        {
            if (account.Tags.ContainsKey(tagName) == false)
                return "You can't update a tag that doesn't exist...";

            account.Tags[tagName] = tagContent;
            if (account is GlobalGuildAccount)
                GlobalGuildAccounts.SaveAccounts(account.Id);
            else GlobalUserAccounts.SaveAccounts(account.Id);

            return $"Successfully updated the tag {tagName}!";
        }
    }
}
