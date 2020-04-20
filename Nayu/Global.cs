using System;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Nayu.Core.Entities;
using Nayu.Core.Features.Economy;
using Nayu.Modules.Chomusuke;
using Type = Nayu.Modules.Chomusuke.Dueling.Enums.Type;

namespace Nayu
{
    internal static class Global
    {
        internal static DiscordShardedClient Client { get; set; }
        internal static Random Rng { get; } = new Random();
        internal static Dictionary<ulong, string> MessagesIdToTrack { get; set; }
        internal static readonly Chomusuke NewChomusuke = new Chomusuke(
            false, null, null, false, 0, 0, 0, 0, false, null, null, null, null, Type.None, 0, Trait.None, Trait.None,
            0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, DateTime.MinValue, null, null, null);

        internal static Slot slot = new Slot();
        internal static readonly Color NayuColor = new Color(153, 255, 255);
        internal static readonly Emote ENo = Emote.Parse("<:no:665078954196992001>");
        internal static readonly Emote EMegumin = Emote.Parse("<:megumin1:701664522251010150>");
        internal static readonly Emote EChomusuke = Emote.Parse("<:chomusuke:601183653657182280>");
        internal static readonly Emote ETaiyaki = Emote.Parse("<:taiyaki:599774631984889857>");

        public static string ReplacePlaceholderStrings(this string messageString, IGuildUser user = null)
        {
            var result = messageString;
            if (user != null) result = ReplaceGuildUserPlaceholderStrings(result, user);

            if (Client != null) result = ReplaceClientPlaceholderStrings(result);

            return result;
        }

        private static string ReplaceGuildUserPlaceholderStrings(string messageString, IGuildUser user)
        {
            return messageString.Replace("<username>", user.Nickname ?? user.Username)
                .Replace("<usermention>", user.Mention)
                .Replace("<guildname>", user.Guild.Name);
        }

        private static string ReplaceClientPlaceholderStrings(string messageString)
        {
            return messageString.Replace("<botmention>", Client.CurrentUser.Mention)
                .Replace("<botdiscriminator>", Client.CurrentUser.Discriminator)
                .Replace("<botname>", Client.CurrentUser.Username);
        }

        public static async Task<string> SendWebRequest(string requestUrl)
        {
            using (var client = new HttpClient(new HttpClientHandler()))
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Nayu");
                using (var response = await client.GetAsync(requestUrl))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return response.StatusCode.ToString();
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}