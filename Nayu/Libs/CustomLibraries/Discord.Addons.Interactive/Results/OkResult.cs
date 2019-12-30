using Discord.Commands;

namespace Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Results
{
    public class OkResult : RuntimeResult
    {
        public OkResult(string reason = null) : base(null, reason) { }
    }
}
