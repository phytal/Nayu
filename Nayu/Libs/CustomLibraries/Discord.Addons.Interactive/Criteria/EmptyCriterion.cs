using System.Threading.Tasks;
using Discord.Commands;

namespace Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Criteria
{
    public class EmptyCriterion<T> : ICriterion<T>
    {
        public Task<bool> JudgeAsync(ShardedCommandContext sourceContext, T parameter)
            => Task.FromResult(true);
    }
}