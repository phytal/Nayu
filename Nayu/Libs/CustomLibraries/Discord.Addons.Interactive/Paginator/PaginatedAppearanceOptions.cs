using System;
using Discord;

namespace Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Paginator
{
    public class PaginatedAppearanceOptions
    {
        public static PaginatedAppearanceOptions Default = new PaginatedAppearanceOptions();

        public IEmote First = new Emoji("⏮");
        public IEmote Back = new Emoji("◀");
        public IEmote Next = new Emoji("▶");
        public IEmote Last = new Emoji("⏭");

        public string FooterFormat = "Page {0}/{1}";

        public bool DisplayInformationIcon = true;

        public TimeSpan? Timeout = null;
        public TimeSpan InfoTimeout = TimeSpan.FromSeconds(30);
    }
}