using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using Nayu.Preconditions;

namespace Nayu.Modules.API
{
    public class Memegen : NayuModule
    {
        [Command("memegen")]
        [Summary("Create a meme!")]
        [Alias("memecreate")]
        [Remarks("n!meme <top text>/<bottom text> (Note that there is no space between top and bottom text from the slash) Ex: n!meme hi/lol")]
        [Cooldown(10)]
        public async Task Define([Remainder] string message)
        {
            message= message.Replace(' ', '_');
            var user = Context.User as SocketGuildUser;
            var aurl = (Context.User.GetAvatarUrl());
            string url = "https://memegen.link/custom/" + message + ".jpg?alt=" + aurl; //+ "?font=Verdana";

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                await Context.Channel.SendFileAsync(stream, "meme.jpg");
            }
        }
        [Command("meme")]
        [Summary("Sends a meme from r/dankmemes")]
        [Remarks("n!meme")]
        [Cooldown(5)]
        public async Task Image()
        {
            string json;
            using (var client = new WebClient())
            {
                json = client.DownloadString("https://www.reddit.com/r/dankmemes/random/.json");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);
            string image = dataObject[0].data.children[0].data.url.ToString();
            string posttitle = dataObject[0].data.children[0].data.title.ToString();
            string link = dataObject[0].data.children[0].data.permalink.ToString();
            string ups = dataObject[0].data.children[0].data.ups.ToString();
            string comments = dataObject[0].data.children[0].data.num_comments.ToString();

            var embed = new EmbedBuilder()
                .WithTitle(posttitle)
                .WithImageUrl(image)
                .WithFooter($"👍 {ups} | 💬 {comments}")
                .WithUrl($"https://www.reddit.com{link}")
                .WithColor(37, 152, 255);
            await SendMessage(Context, embed);
        }
    }
}
