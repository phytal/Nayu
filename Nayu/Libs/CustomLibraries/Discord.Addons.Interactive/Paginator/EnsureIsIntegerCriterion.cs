using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Criteria;

namespace Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Paginator
{
    internal class EnsureIsIntegerCriterion : ICriterion<SocketMessage>
    {
        public Task<bool> JudgeAsync(ShardedCommandContext sourceContext, SocketMessage parameter)
        {
            bool ok = int.TryParse(parameter.Content, out _);
            return Task.FromResult(ok);
        }
    }
}