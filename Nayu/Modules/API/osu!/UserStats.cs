using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Helpers;
using Nayu.Preconditions;
using Newtonsoft.Json;

namespace Nayu.Modules.API
{
    public class UserStats : NayuModule
    {        
        [Subject(Categories.osu)]
        [Command("osustats")]
        [Summary("Get a osu! user's statistics.")]
        [Remarks("n!osustats <osu username> Ex: n!osustats Phytal")]
        [Cooldown(5)]
        public async Task GetOsuStats([Remainder] string user)
        {
            var json = await Global.SendWebRequest("https://osu.ppy.sh/api/get_user?k=2ce122dfe83fb6826b3f2dfe58336006db65c138&type=string&u=" + user);

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string UserId = dataObject[0].user_id.ToString();
            string Username = dataObject[0].username.ToString();
            string rs = dataObject[0].ranked_score.ToString();
            string ts = dataObject[0].total_score.ToString();
            string pprank = dataObject[0].pp_rank.ToString();
            string lvl = dataObject[0].level.ToString();
            string ppraw = dataObject[0].pp_raw.ToString();
            string acc = dataObject[0].accuracy.ToString();
            string country = dataObject[0].country.ToString();
            string ppcountryrank = dataObject[0].pp_country_rank.ToString();
            string playcount = dataObject[0].playcount.ToString();
            string ss = dataObject[0].count_rank_ss.ToString();
            string ssh = dataObject[0].count_rank_ssh.ToString();
            string s = dataObject[0].count_rank_s.ToString();
            string sh = dataObject[0].count_rank_sh.ToString();
            string a = dataObject[0].count_rank_a.ToString();
            string c5 = dataObject[0].count50.ToString();
            string c1 = dataObject[0].count100.ToString();
            string c3 = dataObject[0].count300.ToString();

            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl("https://images.discordapp.net/avatars/421879566265614337/7035b241f838c0e1de3f0ab047352d0b.png?size=512");
            embed.WithColor(Global.NayuColor);
            embed.WithTitle($":video_game:  | **{Username}'s osu! Profile**");
            embed.AddField("Username", Username + $" ({country})", true);
            embed.AddField("UserId", UserId, true);
            embed.AddField("Level", lvl, true);
            embed.AddField("Ranked Score", rs, true);
            embed.AddField("Total Score", ts, true);
            embed.AddField("PP Rank", pprank + $" ({ppcountryrank} in {country})", true);
            embed.AddField("PP Raw", ppraw, true);
            embed.AddField("Play Count", playcount, true);
            embed.AddField("Accuracy", acc, true);
            embed.AddField("Country PP Rank", ppcountryrank, true);
            embed.WithDescription($"**SS+:** {ssh} | **SS:** {ss} | **S+:** {sh} | **S:** {s} | **A:** {a} \n**300s:** {c3} | **100s:** {c1} | **50s:** {c5}" );

            embed.WithFooter("Powered by the osu.ppy.sh API");

            await SendMessage(Context, embed.Build());
        }
        
        [Subject(Categories.osu)]
        [Command("maniastats")]
        [Summary("Get a osu! mania user's statistics.")]
        [Remarks("n!maniastats <osu username> Ex: n!maniastats Phytal")]
        [Cooldown(5)]
        public async Task GetOsuManiaStats([Remainder] string user)
        {
            var json = await Global.SendWebRequest("https://osu.ppy.sh/api/get_user?k=2ce122dfe83fb6826b3f2dfe58336006db65c138&type=string&m=3&u=" + user);

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string UserId = dataObject[0].user_id.ToString();
            string Username = dataObject[0].username.ToString();
            string rs = dataObject[0].ranked_score.ToString();
            string ts = dataObject[0].total_score.ToString();
            string pprank = dataObject[0].pp_rank.ToString();
            string lvl = dataObject[0].level.ToString();
            string ppraw = dataObject[0].pp_raw.ToString();
            string acc = dataObject[0].accuracy.ToString();
            string country = dataObject[0].country.ToString();
            string ppcountryrank = dataObject[0].pp_country_rank.ToString();
            string playcount = dataObject[0].playcount.ToString();
            string ss = dataObject[0].count_rank_ss.ToString();
            string ssh = dataObject[0].count_rank_ssh.ToString();
            string s = dataObject[0].count_rank_s.ToString();
            string sh = dataObject[0].count_rank_sh.ToString();
            string a = dataObject[0].count_rank_a.ToString();
            string c5 = dataObject[0].count50.ToString();
            string c1 = dataObject[0].count100.ToString();
            string c3 = dataObject[0].count300.ToString();

            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl("https://images.discordapp.net/avatars/421879566265614337/7035b241f838c0e1de3f0ab047352d0b.png?size=512");
            embed.WithColor(Global.NayuColor);
            embed.WithTitle($":video_game:  | **{Username}'s osu! mania Profile**");
            embed.AddField("Username", Username + $" ({country})", true);
            embed.AddField("UserId", UserId, true);
            embed.AddField("Level", lvl, true);
            embed.AddField("Ranked Score", rs, true);
            embed.AddField("Total Score", ts, true);
            embed.AddField("PP Rank", pprank + $" ({ppcountryrank} in {country})", true);
            embed.AddField("PP Raw", ppraw, true);
            embed.AddField("Play Count", playcount, true);
            embed.AddField("Accuracy", acc, true);
            embed.AddField("Country PP Rank", ppcountryrank, true);
            embed.WithDescription($"**SS+:** {ssh} | **SS:** {ss} | **S+:** {sh} | **S:** {s} | **A:** {a} \n**300s:** {c3} | **100s:** {c1} | **50s:** {c5}");

            embed.WithFooter("Powered by the osu.ppy.sh API");

            await SendMessage(Context, embed.Build());
        }
        
        [Subject(Categories.osu)]
        [Command("taikostats")]
        [Summary("Get a osu! taiko user's statistics.")]
        [Remarks("n!taikostats <osu username> Ex: n!taikostats Phytal")]
        [Cooldown(5)]
        public async Task GetOsuTaikoStats([Remainder] string user)
        {
            var json = await Global.SendWebRequest("https://osu.ppy.sh/api/get_user?k=2ce122dfe83fb6826b3f2dfe58336006db65c138&type=string&m=1&u=" + user);

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string UserId = dataObject[0].user_id.ToString();
            string Username = dataObject[0].username.ToString();
            string rs = dataObject[0].ranked_score.ToString();
            string ts = dataObject[0].total_score.ToString();
            string pprank = dataObject[0].pp_rank.ToString();
            string lvl = dataObject[0].level.ToString();
            string ppraw = dataObject[0].pp_raw.ToString();
            string acc = dataObject[0].accuracy.ToString();
            string country = dataObject[0].country.ToString();
            string ppcountryrank = dataObject[0].pp_country_rank.ToString();
            string playcount = dataObject[0].playcount.ToString();
            string ss = dataObject[0].count_rank_ss.ToString();
            string ssh = dataObject[0].count_rank_ssh.ToString();
            string s = dataObject[0].count_rank_s.ToString();
            string sh = dataObject[0].count_rank_sh.ToString();
            string a = dataObject[0].count_rank_a.ToString();
            string c5 = dataObject[0].count50.ToString();
            string c1 = dataObject[0].count100.ToString();
            string c3 = dataObject[0].count300.ToString();

            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl("https://images.discordapp.net/avatars/421879566265614337/7035b241f838c0e1de3f0ab047352d0b.png?size=512");
            embed.WithColor(Global.NayuColor);
            embed.WithTitle($":video_game:  | **{Username}'s osu! taiko Profile**");
            embed.AddField("Username", Username + $" ({country})", true);
            embed.AddField("UserId", UserId, true);
            embed.AddField("Level", lvl, true);
            embed.AddField("Ranked Score", rs, true);
            embed.AddField("Total Score", ts, true);
            embed.AddField("PP Rank", pprank + $" ({ppcountryrank} in {country})", true);
            embed.AddField("PP Raw", ppraw, true);
            embed.AddField("Play Count", playcount, true);
            embed.AddField("Accuracy", acc, true);
            embed.AddField("Country PP Rank", ppcountryrank, true);
            embed.WithDescription($"**SS+:** {ssh} | **SS:** {ss} | **S+:** {sh} | **S:** {s} | **A:** {a} \n**300s:** {c3} | **100s:** {c1} | **50s:** {c5}");

            embed.WithFooter("Powered by the osu.ppy.sh API");

            await SendMessage(Context, embed.Build());
        }
        
        [Subject(Categories.osu)]
        [Command("ctbstats")]
        [Summary("Get a osu! ctb user's statistics.")]
        [Remarks("n!ctbstats <osu username> Ex: n!ctbstats Phytal")]
        [Cooldown(5)]
        public async Task GetOsuCtbStats([Remainder] string user)
        {
            var json = await Global.SendWebRequest("https://osu.ppy.sh/api/get_user?k=2ce122dfe83fb6826b3f2dfe58336006db65c138&type=string&m=2&u=" + user);

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

            string UserId = dataObject[0].user_id.ToString();
            string Username = dataObject[0].username.ToString();
            string rs = dataObject[0].ranked_score.ToString();
            string ts = dataObject[0].total_score.ToString();
            string pprank = dataObject[0].pp_rank.ToString();
            string lvl = dataObject[0].level.ToString();
            string ppraw = dataObject[0].pp_raw.ToString();
            string acc = dataObject[0].accuracy.ToString();
            string country = dataObject[0].country.ToString();
            string ppcountryrank = dataObject[0].pp_country_rank.ToString();
            string playcount = dataObject[0].playcount.ToString();
            string ss = dataObject[0].count_rank_ss.ToString();
            string ssh = dataObject[0].count_rank_ssh.ToString();
            string s = dataObject[0].count_rank_s.ToString();
            string sh = dataObject[0].count_rank_sh.ToString();
            string a = dataObject[0].count_rank_a.ToString();
            string c5 = dataObject[0].count50.ToString();
            string c1 = dataObject[0].count100.ToString();
            string c3 = dataObject[0].count300.ToString();

            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl("https://images.discordapp.net/avatars/421879566265614337/7035b241f838c0e1de3f0ab047352d0b.png?size=512");
            embed.WithColor(Global.NayuColor);
            embed.WithTitle($":video_game:  | **{Username}'s osu! ctb Profile**");
            embed.AddField("Username", Username + $" ({country})", true);
            embed.AddField("UserId", UserId, true);
            embed.AddField("Level", lvl, true);
            embed.AddField("Ranked Score", rs, true);
            embed.AddField("Total Score", ts, true);
            embed.AddField("PP Rank", pprank + $" ({ppcountryrank} in {country})", true);
            embed.AddField("PP Raw", ppraw, true);
            embed.AddField("Play Count", playcount, true);
            embed.AddField("Accuracy", acc, true);
            embed.AddField("Country PP Rank", ppcountryrank, true);
            embed.WithDescription($"**SS+:** {ssh} | **SS:** {ss} | **S+:** {sh} | **S:** {s} | **A:** {a} \n**300s:** {c3} | **100s:** {c1} | **50s:** {c5}");

            embed.WithFooter("Powered by the osu.ppy.sh API");

            await SendMessage(Context, embed.Build());
        }
    }
}

