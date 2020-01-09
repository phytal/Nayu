using Discord;

namespace Nayu.Helpers
{
    public class ImageEmbed
    {
        public static Embed GetImageEmbed(string link, Source source, string title, string description = "")
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithDescription(description);
            embed.WithImageUrl(link);
            embed.WithColor(Global.NayuColor);

            switch (source)
            {
                case Source.NekosLife:
                    embed.WithFooter("Powered by Nekos.life");
                    break;
                case Source.WeebDotSh:
                    embed.WithFooter("Powered by weeb.sh");
                    break;
                case Source.None:
                    break;
            }

            return embed.Build();
        }
    }
    
    public enum Source
    {
        WeebDotSh,
        NekosLife,
        None
    }
}