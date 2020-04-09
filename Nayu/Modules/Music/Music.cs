using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Helpers;
using Victoria;
using Victoria.Enums;

namespace Nayu.Modules.Music
{
    public class Music : NayuModule
    {
        private readonly LavaNode _lavaNode;
        private readonly MusicManager _musicManager;
        private static readonly IEnumerable<int> Range = Enumerable.Range(1900, 2000);

        public Music(LavaNode lavaNode, MusicManager musicManager)
        {
            _lavaNode = lavaNode;
            _musicManager = musicManager;
        }

        [Subject(Categories.Music)]
        [Command("join")]
        [Summary("Nayu joins the current voice channel you are in")]
        [Remarks("Ex: n!join")]
        public async Task JoinAsync()
        {
            if (_lavaNode.HasPlayer(Context.Guild))
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | I'm already connected to a voice channel!");
                return;
            }

            var voiceState = Context.User as IVoiceState;

            if (voiceState?.VoiceChannel == null)
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | You must be connected to a voice channel!");
                return;
            }

            try
            {
                await _lavaNode.JoinAsync(voiceState.VoiceChannel, Context.Channel as ITextChannel);
                await ReplyAsync($"✅ **|** Joined **{voiceState.VoiceChannel.Name}!**");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Subject(Categories.Music)]
        [Command("leave")]
        [Summary("Nayu leaves from the current voice channel you are in")]
        [Remarks("Ex: n!leave")]
        public async Task LeaveAsync()
        {
            if (!_lavaNode.TryGetPlayer(Context.Guild, out var player))
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | I'm not connected to any voice channels!");
                return;
            }

            var voiceChannel = (Context.User as IVoiceState).VoiceChannel ?? player.VoiceChannel;
            if (voiceChannel == null)
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | I cannot disconnect from a voice channel you are not in!");
                return;
            }

            try
            {
                await _lavaNode.LeaveAsync(voiceChannel);
                await ReplyAsync($"✅ **|** I've left **{voiceChannel.Name}**!");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Subject(Categories.Music)]
        [Command("play")]
        [Summary("Searches and plays a song from Youtube")]
        [Remarks("n!play <song name or link> Ex: n!play gangnam style")]
        public async Task PlayAsync([Remainder] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | Please provide search terms.");
                return;
            }

            var voiceState = Context.User as IVoiceState;

            if (voiceState?.VoiceChannel == null)
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | You must be connected to a voice channel!");
                return;
            }

            if (voiceState?.VoiceChannel != null)
            {
                try
                {
                    await _lavaNode.JoinAsync(voiceState.VoiceChannel, Context.Channel as ITextChannel);
                }
                catch (Exception exception)
                {
                    await ReplyAsync(exception.Message);
                }
            }

            ;

            var searchResponse = await _lavaNode.SearchYouTubeAsync(query);

            if (searchResponse.LoadStatus == LoadStatus.LoadFailed ||
                searchResponse.LoadStatus == LoadStatus.NoMatches)
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | I wasn't able to find anything for `{query}`.");
                return;
            }

            var player = _lavaNode.GetPlayer(Context.Guild);

            if (player.PlayerState == PlayerState.Playing || player.PlayerState == PlayerState.Paused)
            {
                if (!string.IsNullOrWhiteSpace(searchResponse.Playlist.Name))
                {
                    foreach (var track in searchResponse.Tracks)
                    {
                        player.Queue.Enqueue(track);
                    }

                    await ReplyAsync($"✅ **|** **Enqueued {searchResponse.Tracks.Count} tracks.**");
                }
                else
                {
                    var track = searchResponse.Tracks[0];
                    player.Queue.Enqueue(track);
                    await ReplyAsync($"✅ **|** **Enqueued: {track.Title}**");
                }
            }
            else
            {
                var track = searchResponse.Tracks[0];

                if (!string.IsNullOrWhiteSpace(searchResponse.Playlist.Name))
                {
                    for (var i = 0; i < searchResponse.Tracks.Count; i++)
                    {
                        if (i == 0)
                        {
                            await player.PlayAsync(track);
                            await ReplyAsync($"**Now Playing: {track.Title}**");
                        }
                        else
                        {
                            player.Queue.Enqueue(searchResponse.Tracks[i]);
                        }
                    }

                    await ReplyAsync($"✅ **|** **Enqueued {searchResponse.Tracks.Count} tracks.**");
                }
                else
                {
                    await player.PlayAsync(track);
                    await ReplyAsync($"**Now Playing: {track.Title}**");
                }
            }
        }

        [Subject(Categories.Music)]
        [Command("pause")]
        [Summary("Pauses the current song")]
        [Remarks("Ex: n!pause")]
        public async Task PauseAsync()
        {
            if (!_lavaNode.TryGetPlayer(Context.Guild, out var player))
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | I'm not connected to a voice channel.");
                return;
            }

            if (player.PlayerState != PlayerState.Playing)
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | I cannot pause when I'm not playing anything!");
                return;
            }

            try
            {
                await player.PauseAsync();
                await ReplyAsync($"✅ **|** Paused: {player.Track.Title}");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Subject(Categories.Music)]
        [Command("resume")]
        [Summary("Resumes the song that is paused")]
        [Remarks("Ex: n!resume")]
        public async Task ResumeAsync()
        {
            if (!_lavaNode.TryGetPlayer(Context.Guild, out var player))
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | I'm not connected to a voice channel.");
                return;
            }

            if (player.PlayerState != PlayerState.Paused)
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | I cannot resume when I'm not playing anything!");
                return;
            }

            try
            {
                await player.ResumeAsync();
                await ReplyAsync($"✅ **|** Resumed: {player.Track.Title}");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Subject(Categories.Music)]
        [Command("stop")]
        [Summary("Stops the current song")]
        [Remarks("Ex: n!stop")]
        public async Task StopAsync()
        {
            if (!_lavaNode.TryGetPlayer(Context.Guild, out var player))
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | I'm not connected to a voice channel.");
                return;
            }

            if (player.PlayerState == PlayerState.Stopped)
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | The track is already stopped!");
                return;
            }

            try
            {
                await player.StopAsync();
                await ReplyAsync("✅ **|** No longer playing anything.");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Subject(Categories.Music)]
        [Command("skip")]
        [Summary("Skips the song that is currently playing")]
        [Remarks("Ex: n!skip")]
        public async Task SkipAsync()
        {
            if (!_lavaNode.TryGetPlayer(Context.Guild, out var player))
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | I'm not connected to a voice channel.");
                return;
            }

            if (player.PlayerState != PlayerState.Playing)
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | There's nothing playing at the moment.");
                return;
            }

            var voiceChannelUsers = (player.VoiceChannel as SocketVoiceChannel).Users.Where(x => !x.IsBot).ToArray();
            if (_musicManager.VoteQueue.Contains(Context.User.Id))
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | You can't vote again.");
                return;
            }

            _musicManager.VoteQueue.Add(Context.User.Id);
            var percentage = _musicManager.VoteQueue.Count / voiceChannelUsers.Length * 100;
            if (percentage < 85 && !(Context.User as SocketGuildUser).GuildPermissions.ManageMessages)
            {
                await ReplyAndDeleteAsync("You need more than 85% votes to skip this song.");
                return;
            }

            try
            {
                var oldTrack = player.Track;
                var currentTrack = await player.SkipAsync();
                await ReplyAsync($"Skipped: {oldTrack.Title}\nNow Playing: {currentTrack.Title}");
                _musicManager.VoteQueue.Clear();
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        //TODO: fix
        [Subject(Categories.Music)]
        [Command("seek")]
        [Summary("Seeks to a specific time point in the song/video")]
        [Remarks("n!seek <time> Ex: n!seek 5:12")]
        public async Task SeekAsync(TimeSpan timeSpan)
        {
            if (!_lavaNode.TryGetPlayer(Context.Guild, out var player))
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | I'm not connected to a voice channel.");
                return;
            }

            if (player.PlayerState != PlayerState.Playing)
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | There's nothing playing at the moment.");
                return;
            }

            try
            {
                await player.SeekAsync(timeSpan);
                await ReplyAsync($"✅ **|** I've seeked `{player.Track.Title}` to {timeSpan}.");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Subject(Categories.Music)]
        [Command("volume"), Alias("vol")]
        [Summary("Changes the volume of the song per guild (100 is default)")]
        [Remarks("n!vol <volume> Ex: n!vol 86")]
        public async Task VolumeAsync(ushort volume)
        {
            if (!_lavaNode.TryGetPlayer(Context.Guild, out var player))
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | I'm not connected to a voice channel.");
                return;
            }

            if (volume >= 150 || volume <= 0)
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | Volume must be between 1 and 149.");
                return;
            }

            try
            {
                await player.UpdateVolumeAsync(volume);
                await ReplyAsync($"🔊 **|** I've changed the player volume to {volume}.");
            }
            catch (Exception exception)
            {
                await ReplyAsync(exception.Message);
            }
        }

        [Subject(Categories.Music)]
        [Command("nowPlaying"), Alias("Np")]
        [Summary("Shows the song that is currently playing")]
        [Remarks("Ex: n!np")]
        public async Task NowPlayingAsync()
            => await ReplyAsync("", embed: await _musicManager.NowPlayingAsync(Context));

        [Subject(Categories.Music)]
        [Command("queue"), Alias("q")]
        [Summary("Shows the current queue")]
        [Remarks("Ex: n!queue")]
        public async Task GetQueueAsync()
            => await ReplyAsync("", embed: await _musicManager.GetQueueAsync(Context));

        [Subject(Categories.Music)]
        [Command("lyrics")]
        [Summary("Shows the lyrics for the current song playing")]
        [Remarks("Ex: n!lyrics")]
        public async Task ShowGeniusLyrics()
        {
            if (!_lavaNode.TryGetPlayer(Context.Guild, out var player))
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | I'm not connected to a voice channel.");
                return;
            }

            if (player.PlayerState != PlayerState.Playing)
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | A track must be playing for me to get its lyrics.");
                return;
            }

            var lyrics = await player.Track.FetchLyricsFromGeniusAsync();
            if (string.IsNullOrWhiteSpace(lyrics))
            {
                await ReplyAndDeleteAsync($"{Global.ENo} | No lyrics found for {player.Track.Title}");
                return;
            }


            var splitLyrics = lyrics.Split('\n');
            var stringBuilder = new StringBuilder();
            foreach (var line in splitLyrics)
            {
                if (Range.Contains(stringBuilder.Length))
                {
                    var embedd = MiscHelpers.CreateEmbed(Context, "", stringBuilder.ToString());
                    await ReplyAsync("", embed: embedd.Build());
                    stringBuilder.Clear();
                }
                else
                {
                    stringBuilder.AppendLine(line);
                }
            }

            var embed = MiscHelpers.CreateEmbed(Context, "", stringBuilder.ToString());
            await ReplyAsync("", embed: embed.Build());
        }
    }
}