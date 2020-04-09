using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Callbacks;
using Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Criteria;

namespace Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Paginator
{
    public class PaginatedMessageCallback : IReactionCallback
    {
        public ShardedCommandContext Context { get; }
        public InteractiveService Interactive { get; private set; }
        public IUserMessage Message { get; private set; }

        public RunMode RunMode => RunMode.Sync;
        public ICriterion<SocketReaction> Criterion => _criterion;
        public TimeSpan? Timeout => options.Timeout;

        private readonly ICriterion<SocketReaction> _criterion;
        private readonly PaginatedMessage _pager;

        private PaginatedAppearanceOptions options => _pager.Options;
        private readonly int pages;
        private readonly int titles;
        private int page = 1;
        private int title = 1;


        public PaginatedMessageCallback(InteractiveService interactive,
            ShardedCommandContext sourceContext,
            PaginatedMessage pager,
            ICriterion<SocketReaction> criterion = null)
        {
            Interactive = interactive;
            Context = sourceContext;
            _criterion = criterion ?? new EmptyCriterion<SocketReaction>();
            _pager = pager;
            pages = _pager.Pages.Count();
            _pager = pager;
            titles = _pager.Title.Count();
        }

        public async Task DisplayAsync()
        {
            var embed = BuildEmbed();
            var message = await Context.Channel.SendMessageAsync(_pager.Content, embed: embed).ConfigureAwait(false);
            Message = message;
            Interactive.AddReactionCallback(message, this);
            // Reactions take a while to add, don't wait for them
            _ = Task.Run(async () =>
            {
                await message.AddReactionAsync(options.First);
                await message.AddReactionAsync(options.Back);
                await message.AddReactionAsync(options.Next);
                await message.AddReactionAsync(options.Last);

                var manageMessages = (Context.Channel is IGuildChannel guildChannel)
                    ? (Context.User as IGuildUser).GetPermissions(guildChannel).ManageMessages
                    : false;
            });
            if (Timeout.HasValue && Timeout.Value != null)
            {
                _ = Task.Delay(Timeout.Value).ContinueWith(_ =>
                {
                    Interactive.RemoveReactionCallback(message);
                    _ = Message.DeleteAsync();
                });
            }
        }

        public async Task<bool> HandleCallbackAsync(SocketReaction reaction)
        {
            var emote = reaction.Emote;

            if (emote.Equals(options.First))
            {
                title = 1;
                page = 1;
            }
            else if (emote.Equals(options.Next))
            {
                if (page >= pages)
                    return false;
                ++page;
                ++title;
            }
            else if (emote.Equals(options.Back))
            {
                if (page <= 1)
                    return false;
                --page;
                --title;
            }
            else if (emote.Equals(options.Last))
                page = pages;

            _ = Message.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
            await RenderAsync().ConfigureAwait(false);
            return false;
        }

        protected Embed BuildEmbed()
        {
            return new EmbedBuilder()
                .WithAuthor(_pager.Author)
                .WithColor(_pager.Color)
                .WithDescription(_pager.Pages.ElementAt(page - 1).ToString())
                .WithFooter(f => f.Text = string.Format(options.FooterFormat, page, pages))
                .WithTitle(_pager.Title.ElementAt(title - 1).ToString())
                .Build();
        }

        private async Task RenderAsync()
        {
            var embed = BuildEmbed();
            await Message.ModifyAsync(m => m.Embed = embed).ConfigureAwait(false);
        }
    }
}