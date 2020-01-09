using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Configuration;
using Nayu.Preconditions;

namespace Nayu.Modules
{
    public class Information : NayuModule
    {
        private CommandService _service;

        public Information(CommandService service)
        {
            _service = service;
        }


        [Command("info")]
        [Summary("Shows Nayu's information")]
        [Remarks("Ex: n!info")]
        [Cooldown(10)]
        public async Task Info()
        {
            string version = Config.bot.version;
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.AddField("Creator", "Phytal#8213", true);
            embed.AddField("Last Updated", "11/29/2018", true);
            embed.AddField("Bot version", $"Beta {version}", true);
            embed.WithImageUrl(Global.Client.CurrentUser.GetAvatarUrl());

            await SendMessage(Context, embed.Build());
        }

        [Command("help")]
        [Summary("Shows all possible standard commands for this bot")]
        [Remarks("Ex: n!help")]
        [Cooldown(30)]
        public async Task HelpMessage()
        {
            string helpMessage =
            "```cs\n" +
            "'Standard Command List'\n" +
            "```\n" +
            "Use `n!command [command]` to get more info on a specific command. Ex: `n!command stats`  `[Prefix 'n!']` \n " +
            "\n" +
            "**1. Core -** `help` `invite` `patreon` `ping` `dailyVote` `command` `changeLog`\n" +
            "**2. Social -** `stats` `topXP` `report`\n" +
            "**3. Emotes -** `cuddle` `feed` `hug` `kiss` `pat` `poke` `tickle` `slap` `baka` `bite` `waifuInsult` `dab` `greet` `insult` `kill` `lick` `shrug` `triggered` `bang` `baka` `cry` `dance` `highFive` `holdHand` `pout` `punch` `smug` `stare` `thumbsUp` `wag`\n" +
            "**4. Fun -**  `lenny` `rateWaifu` `bigLetter` `rps` `define` `meme` `gif`\n" +
            "**5. Economy -** `balance` `daily` `rank`\n" +
            "**6. Gambling -** `roll` `coinFlip` `newSlots` `slots` `showSlots`\n" +
            "**7. Misc -** `shiba`\n" +
            "**7. Anime Images -** `neko` `foxGirl` `kemonomimi` `lewdSFW` `megumin` `rem` `owo`\n" +
            "**8. Overwatch -** `owStats` `owStatsComp` `owStatsQP` `myOwStats` `myOwStatsComp` `myOwStatsQP` `owHeroStats` `owHeroStatsComp` `owHeroStatsQP` `myOwHeroStats` `myOwHeroStatsComp` `myOwHeroStatsQP` `owAccount`\n" +
            "**9. osu! -** `osuStats` `maniaStats` `taikoStats` `ctbStats`\n" +
            "**10. Self Roles -** `iAm` `iAmNot` `selfRoleList`\n" +
            "**11. Chomusuke (n!chomusuke <command>) -** `stats` `feed` `clean` `train` `play` `name` `buy` `picture` `help` `openCapsule` `inventory`\n" +
            "**17. Personal Tags (n!ptag <command>)-** `new` `update` `remove` `list`\n" +
            "**18. Lootboxes -** `openLootBox` `lootBoxInventory` `giftLootbox`\n" +
            "\n" +
            "```\n" +
            "# Don't include the example brackets when using commands!\n" +
            "# To view Moderator commands, use n!helpmod\n" +
            "# To view NSFW commands, use n!helpnsfw\n" +
            "# To view Dueling commands, use n!duelhelp\n" +
            "```";

            await ReplyAsync(helpMessage);
        }

        [Command("helpmod")]
        [Summary("Shows all possible moderator commands for this bot")]
        [Remarks("Ex: n!helpmod")]
        [Cooldown(30)]
        public async Task HelpMessageMod()
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.ManageMessages)
            {
                string helpMessageMod =
            "```cs\n" +
            "Moderator Command List\n" +
            "```\n" +
            "Use `n!command [command]` to get more info on a specific command. Ex: `n!command xp`  `[Prefix 'n!']`\n" +
            "\n" +
            "**Filters -** `antiLink` `filter` `pingChecks` `antiLinkIgnore` `filterIgnore` `blacklistAdd` `blacklistRemove` `blacklistClear` `blacklistList`\n" +
            "**User Management -** `ban` `kick` `mute` `unmute` `clear` `warn` `warnings` `clearWarnings` `say` `softBan` `idBan` `promote` `demote` `rename`\n" +
            "**Bot Settings -** `serverPrefix` `leveling` `levelingMsg` `config` `customCurrency`\n" +
            "**Welcome Messages (n!celcome <command>) -** `channel` `add` `remove` `list`\n" +
            "**Leaving Messages (n!leave <command>) -** `channel` `add` `remove` `list`\n" +
            "**Announcements (n!announcements <command>) -** `setChannel` `unsetChannel` `announce`\n" +
            "**Server Management -** `serverLogging` `slowMode` `lockChannel` `unlockChannel`\n" +
            "**Roles -** `helperRole` `modRole` `adminRole` `selfRoleAdd` `selfRoleRem` `selfRoleClear`\n" +
                        "**Server Tags (n!tag <command>)-** `new` `update` `remove` `list`\n" +
            "**Fun Stuff -** `unFlip` `vote` `customCommandAdd` `customCommandRemove` `customCommandList`\n" +
            "\n" +
            "```\n" +
            "# Don't include the example brackets when using commands!\n" +
            "# To view standard commands, use n!help\n" +
            "# To view NSFW commands, use n!helpnsfw\n" +
            "```";

                await ReplyAsync(helpMessageMod);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $"{Global.ENo} | You Need the Administrator Permission to do that {Context.User.Username}";
                var use = await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("helpnsfw")]
        [Summary("Shows all possible NSFW Commands for this bot")]
        [Remarks("Ex: n!helpnsfw")]
        [Cooldown(30)]
        public async Task HelpMessageNSFW()
        {
            if (Context.Channel is ITextChannel text)
            {
                var nsfw = text.IsNsfw;
                if (nsfw)
                {
                    string helpMessageNSFW =
                "```cs\n" +
                "NSFW Command List (why did i make this)\n" +
                "```\n" +
                "Use `n!command [command]` to get more info on a specific command. Ex: `n!command xp`  `[Prefix 'n!']`\n" +
                "\n" +
                "**Neko -** `lewd` `nekoNsfwGif` `autoLewd`\n" +
                "**Hentai -** `anal` `boobs` `cum` `les` `pussy` `blowJob` `classic` `kuni` `overwatchNSFW`\n" +
                "\n" +
                "```\n" +
                "# Don't include the example brackets when using commands!\n" +
                "# To view Standard commands, use n!help\n" +
                "# To view Moderator commands, use n!helpmod\n" +
                "```";

                    await ReplyAsync(helpMessageNSFW);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(37, 152, 255);
                    embed.Title = $"{Global.ENo} | You Need to be in a NSFW channel to do that {Context.User.Username}";
                    var use = await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
                }
            }
        }

        [Command("command")]
        [Summary("Shows what a specific command does and the usage")]
        [Remarks("n!command <command you want to search up> Ex: n!command stats")]
        [Cooldown(5)]
        public async Task CommandAsync(string command)
        {
            var result = _service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like {command}.");
                return;
            }

            var thumbnailurl = Context.User.GetAvatarUrl();

            var auth = new EmbedAuthorBuilder()
            {
                IconUrl = thumbnailurl,
            };
            var builder = new EmbedBuilder()
            {
                Author = auth,
                Title = ":book: Command Dictionary",
                Color = new Color(37, 152, 255),
                Description = $"Here are the aliases of **{command}**"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = //$"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                                $"Description: {cmd.Summary}\n" +
                                $"Usage: {cmd.Remarks}";
                    x.IsInline = false;
                });
            }
            await SendMessage(Context, builder.Build());
        }

        [Command("nayuLink")]
        [Summary("Provides Nayu's server invite link")]
        [Alias("serverinvitelink")]
        [Remarks("Ex: n!Nayulink")]
        [Cooldown(10)]
        public async Task SendAsync()
        {
            await ReplyAsync("https://discord.gg/NuUdx4h ~~ Here's my server! :blush: ");
        }

        [Command("dailyvote")]
        [Summary("Provides Nayu's daily vote link")]
        [Alias("dv")]
        [Remarks("Ex: n!dv")]
        [Cooldown(10)]
        public async Task DailyVote()
        {
            await ReplyAsync("https://discordbots.org/bot/417160957010116608/vote ~~ Vote for me please! I'll love you forever! ");
        }

        [Command("invite")]
        [Summary("Invite Nayu to your server!")]
        [Alias("Nayuinvitelink")]
        [Remarks("Ex: n!invite")]
        [Cooldown(10)]
        public async Task InviteAsync()
        {
            await ReplyAsync("https://discordapp.com/api/oauth2/authorize?client_id=417160957010116608&permissions=8&scope=bot ~~ Invite me to your servers! :blush: ");
        }

        [Command("patreon")]
        [Summary("Sends the Patreon link to help contribue to the efforts of Nayu")]
        [Alias("donate")]
        [Remarks("Ex: n!patreon")]
        [Cooldown(10)]
        public async Task Patreon()
        {
            await ReplyAsync("https://www.patreon.com/phytal ~~ Help us out! :blush: ");
        }

        [Command("changelog")]
        [Summary("Shows the latest update notes")]
        [Alias("update")]
        [Remarks("Ex: n!changelog")]
        [Cooldown(15)]
        public async Task Update()
        {
            string version = Config.bot.version;
            var embed = new EmbedBuilder();
            embed.WithColor(37, 152, 255);
            embed.WithTitle("Update Notes");
            embed.WithDescription($"`Bot version {version}` **<<Last Updated on 6/24>>**\n"
                + "`----- LAST UPDATE -----`\n"
                + "• Added rock paper scissors!Use `n!rps`!\n"
                + "• Made it so that you will now have a * seperate * account per server, money is carried over, but XP is different (the leveling system was also updated :D)!\n"
                + "• Aesthetically improved the `n!command` command!\n"
                + "• Squished a bugs and fixed typos :D..\n"
                + "`----- CURRENT UPDATE -----`\n"
                + " • Improved the old dueling system(IMPROVED IT SO MUCH) use `n!duelhelp` to see the commands!\n"
                + " • Added more stuff to the Dog and Cat API(Cat API broke so I got a new onw :D)!\n"
                + " • Fixed `n!help` as some commands were missing..\n"
                + " • Added Reminders!You can use `n!reminder add < reminder > in < time >`.\n"
                + " • Improved gambling(just `n!coinflip` i'm sorry xd)\n"
                );

            await ReplyAsync("", embed: embed.Build());
        }
    }
}
