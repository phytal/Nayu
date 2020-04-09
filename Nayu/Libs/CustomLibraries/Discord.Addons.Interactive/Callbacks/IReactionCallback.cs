using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Criteria;

namespace Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Callbacks
{
    public interface IReactionCallback
    {
        RunMode RunMode { get; }
        ICriterion<SocketReaction> Criterion { get; }
        TimeSpan? Timeout { get; }
        ShardedCommandContext Context { get; }

        Task<bool> HandleCallbackAsync(SocketReaction reaction);
    }
}