using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Criteria;

namespace Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Paginator
{
    internal class EnsureReactionFromSourceUserCriterion : ICriterion<SocketReaction>
    {
        public Task<bool> JudgeAsync(ShardedCommandContext sourceContext, SocketReaction parameter)
        {
            bool ok = parameter.UserId == sourceContext.User.Id;
            return Task.FromResult(ok);
        }
    }
}
