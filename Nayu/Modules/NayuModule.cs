using Nayu.Libs.CustomLibraries.Discord.Addons.Interactive;

namespace Nayu.Modules
{
    public abstract class NayuModule : InteractiveBase
    {
        public bool Enabled = true;
        public bool Disabled = false;
        public int Zero = 0;
    }
}
