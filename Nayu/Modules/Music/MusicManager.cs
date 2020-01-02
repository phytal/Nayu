using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Victoria;
using Victoria.EventArgs;

namespace Nayu.Modules.Music {
    public class MusicManager 
    {
        private readonly LavaNode _lavaNode;
        private readonly Logger _logger;
        public readonly HashSet<ulong> VoteQueue;

        public MusicManager(LavaNode lavaNode)
        {
            _lavaNode = lavaNode;
            _lavaNode.OnLog += OnLog;
            _lavaNode.OnPlayerUpdated += OnPlayerUpdated;
            _lavaNode.OnStatsReceived += OnStatsReceived;
            _lavaNode.OnTrackEnded += OnTrackEnded;
            _lavaNode.OnTrackException += OnTrackException;
            _lavaNode.OnTrackStuck += OnTrackStuck;
            _lavaNode.OnWebSocketClosed += OnWebSocketClosed;

            VoteQueue = new HashSet<ulong>();
        }

        private Task OnLog(LogMessage arg) {
            _logger.ConsoleMusicLog(arg.Exception + arg.Message);
            return Task.CompletedTask;
        }

        private Task OnPlayerUpdated(PlayerUpdateEventArgs arg) {
            _logger.ConsoleMusicLog($"Player update received for {arg.Player.VoiceChannel.Name}.");
            return Task.CompletedTask;
        }

        private Task OnStatsReceived(StatsEventArgs arg) {
            _logger.ConsoleMusicLog($"Lavalink Uptime {arg.Uptime}.");
            return Task.CompletedTask;
        }

        private async Task OnTrackEnded(TrackEndedEventArgs args) {
            if (!args.Reason.ShouldPlayNext())
                return;

            var player = args.Player;
            if (!player.Queue.TryDequeue(out var queueable)) {
                await player.TextChannel.SendMessageAsync("No more tracks to play.");
                return;
            }

            if (!(queueable is LavaTrack track)) {
                await player.TextChannel.SendMessageAsync("Next item in queue is not a track.");
                return;
            }

            VoteQueue.Clear();
            
            await args.Player.PlayAsync(track);
            await args.Player.TextChannel.SendMessageAsync(
                $"{args.Reason}: {args.Track.Title}\nNow playing: {track.Title}");
        }

        private Task OnTrackException(TrackExceptionEventArgs arg) {
            _logger.ConsoleMusicLog($"Track exception received for {arg.Track.Title}.");
            return Task.CompletedTask;
        }

        private Task OnTrackStuck(TrackStuckEventArgs arg) {
            _logger.ConsoleMusicLog($"Track stuck received for {arg.Track.Title}.");
            return Task.CompletedTask;
        }

        private Task OnWebSocketClosed(WebSocketClosedEventArgs arg) {
            _logger.ConsoleMusicLog($"Discord WebSocket connection closed with following reason: {arg.Reason}");
            return Task.CompletedTask;
        }
    }
}