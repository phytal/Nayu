using System.Threading.Tasks;
using Discord.Commands;

namespace Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Criteria
{
    public interface ICriterion<in T>
    {
        Task<bool> JudgeAsync(ShardedCommandContext sourceContext, T parameter);
    }
}