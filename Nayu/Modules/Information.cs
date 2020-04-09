using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Configuration;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Core.Handlers;
using Nayu.Helpers;
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

        [Subject(Categories.Information)]
        [Command("help")]
        [Summary("Shows all possible standard commands for this bot")]
        [Remarks("Ex: n!help")]
        [Cooldown(30)]
        public async Task HelpMessage()
        {
            var assembly = Assembly.Load("Nayu");
            var methods = assembly.GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(Subject), false).Length > 0)
                .ToList();

            var helpMessage = "```cs\n" +
                              "'Standard Command List'\n" +
                              "```" +
                              "Use `n!command [command]` to get more info on a specific command. Ex: `n!command stats`  `[Prefix 'n!']` \n ";
            var categoryNum = 1;
            var categories = new List<Categories>((Categories[]) Enum.GetValues(typeof(Categories)));
            //removes the none enum
            categories.RemoveAt(0);

            foreach (var category in categories)
            {
                string categoryDesc = null;
                try
                {
                    categoryDesc = MiscHelpers.GetAttributeOfType<DescriptionAttribute>(category).Description;
                }
                catch (Exception)
                {
                    //do nothing
                }

                var categoryName = categoryDesc != null ? categoryDesc : category.ToString();


                helpMessage += $"\n**{categoryNum}. {categoryName} -**";

                foreach (var method in methods.ToList())
                {
                    var sub = method.GetCustomAttribute<Subject>(true);
                    if (sub.GetAdminCategories() == AdminCategories.None &&
                        sub.GetChomusukeCategories() == ChomusukeCategories.None &&
                        sub.GetOwnerCategories() == OwnerCategories.None &&
                        sub.GetNSFWCategories() == NSFWCategories.None)
                        if (sub.GetCategories() == category)
                        {
                            helpMessage += $" `{MiscHelpers.GetCommandAttribute(method).Text}`";
                            //removes the method from the list if successful to make it faster
                            methods.Remove(method);
                        }
                }

                categoryNum++;
            }

            helpMessage += "\n" +
                           "```\n" +
                           "# Don't include the example brackets when using commands!\n" +
                           "# To view Moderator commands, use n!helpMod\n" +
                           "# To view NSFW commands, use n!helpNsfw\n" +
                           "# To view Chomusuke commands, use n!helpChom\n" +
                           "```";
            var embed = EmbedHandler.CreateEmbed(Context, "Help Command", helpMessage,
                EmbedHandler.EmbedMessageType.Info,
                false);
            await SendMessage(Context, embed);
        }

        [Subject(Categories.Information)]
        [Command("helpMod")]
        [Summary("Shows all possible moderator commands for this bot")]
        [Remarks("Ex: n!helpMod")]
        [Cooldown(30)]
        public async Task HelpMessageMod()
        {
            var guildUser = Context.User as SocketGuildUser;
            if (!guildUser.GuildPermissions.Administrator)
            {
                var description =
                    $"{Global.ENo} **|** You Need the **Administrator** Permission to do that {Context.User.Username}";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", description,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var assembly = Assembly.Load("Nayu");
            var methods = assembly.GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(Subject), false).Length > 0)
                .ToList();

            var helpMessage = "```cs\n" +
                              "Moderator Command List\n" +
                              "```" +
                              "Use `n!command [command]` to get more info on a specific command. Ex: `n!command stats`  `[Prefix 'n!']` \n ";
            var categoryNum = 1;
            var categories = new List<AdminCategories>((AdminCategories[]) Enum.GetValues(typeof(AdminCategories)));
            //removes the none enum
            categories.RemoveAt(0);

            foreach (var category in categories)
            {
                string categoryDesc = null;
                try
                {
                    categoryDesc = MiscHelpers.GetAttributeOfType<DescriptionAttribute>(category).Description;
                }
                catch (Exception)
                {
                    //do nothing
                }

                var categoryName = categoryDesc != null ? categoryDesc : category.ToString();


                helpMessage += $"\n**{categoryNum}. {categoryName} -**";

                foreach (var method in methods.ToList())
                {
                    var sub = method.GetCustomAttribute<Subject>(true);
                    if (sub.GetCategories() == Categories.None &&
                        sub.GetChomusukeCategories() == ChomusukeCategories.None &&
                        sub.GetOwnerCategories() == OwnerCategories.None &&
                        sub.GetNSFWCategories() == NSFWCategories.None)
                        if (sub.GetAdminCategories() == category)
                        {
                            helpMessage += $" `{MiscHelpers.GetCommandAttribute(method).Text}`";
                            //removes the method from the list if successful to make it faster
                            methods.Remove(method);
                        }
                }

                categoryNum++;
            }

            helpMessage += "\n" +
                           "```\n" +
                           "# Don't include the example brackets when using commands!\n" +
                           "# To view standard commands, use n!help\n" +
                           "# To view NSFW commands, use n!helpNsfw\n" +
                           "# To view Chomusuke commands, use n!helpChom\n" +
                           "```";

            var embed = EmbedHandler.CreateEmbed(Context, "Moderator Help Command", helpMessage,
                EmbedHandler.EmbedMessageType.Info,
                false);
            await SendMessage(Context, embed);
        }

        [Subject(Categories.Information)]
        [Command("helpNsfw")]
        [Summary("Shows all possible NSFW Commands for this bot")]
        [Remarks("Ex: n!helpNsfw")]
        [Cooldown(30)]
        public async Task HelpMessageNSFW()
        {
            var channel = Context.Channel as ITextChannel;
            if (!channel.IsNsfw)
            {
                var nsfwText =
                    $"{Global.ENo} **|** You need to use this command in a NSFW channel, {Context.User.Username}!";
                var errorEmbed = EmbedHandler.CreateEmbed(Context, "Error", nsfwText,
                    EmbedHandler.EmbedMessageType.Exception);
                await ReplyAndDeleteAsync("", embed: errorEmbed);
                return;
            }

            var assembly = Assembly.Load("Nayu");
            var methods = assembly.GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(Subject), false).Length > 0)
                .ToList();

            var helpMessage = "```cs\n" +
                              "NSFW Command List\n" +
                              "```" +
                              "Use `n!command [command]` to get more info on a specific command. Ex: `n!command stats`  `[Prefix 'n!']` \n ";
            var categoryNum = 1;
            var categories = new List<NSFWCategories>((NSFWCategories[]) Enum.GetValues(typeof(NSFWCategories)));
            //removes the none enum
            categories.RemoveAt(0);

            foreach (var category in categories)
            {
                string categoryDesc = null;
                try
                {
                    categoryDesc = MiscHelpers.GetAttributeOfType<DescriptionAttribute>(category).Description;
                }
                catch (Exception)
                {
                    //do nothing
                }

                var categoryName = categoryDesc != null ? categoryDesc : category.ToString();


                helpMessage += $"\n**{categoryNum}. {categoryName} -**";

                foreach (var method in methods.ToList())
                {
                    var sub = method.GetCustomAttribute<Subject>(true);
                    if (sub.GetCategories() == Categories.None &&
                        sub.GetChomusukeCategories() == ChomusukeCategories.None &&
                        sub.GetOwnerCategories() == OwnerCategories.None &&
                        sub.GetAdminCategories() == AdminCategories.None)
                        if (sub.GetNSFWCategories() == category)
                        {
                            helpMessage += $" `{MiscHelpers.GetCommandAttribute(method).Text}`";
                            //removes the method from the list if successful to make it faster
                            methods.Remove(method);
                        }
                }

                categoryNum++;
            }

            helpMessage += "\n" +
                           "```\n" +
                           "# Don't include the example brackets when using commands!\n" +
                           "# To view standard commands, use n!help\n" +
                           "# To view moderator commands, use n!helpmod\n" +
                           "# To view Chomusuke commands, use n!helpChom\n" +
                           "```";

            var embed = EmbedHandler.CreateEmbed(Context, "NSFW Help Command", helpMessage,
                EmbedHandler.EmbedMessageType.Info,
                false);
            await SendMessage(Context, embed);
        }

        [Subject(Categories.Information)]
        [Command("helpChom")]
        [Alias("helpChomusuke")]
        [Summary("Shows all possible Chomusuke commands for this bot")]
        [Remarks("Ex: n!helpChom")]
        [Cooldown(30)]
        public async Task ChomusukeHelpMessage()
        {
            var assembly = Assembly.Load("Nayu");
            var methods = assembly.GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(Subject), false).Length > 0)
                .ToList();

            var helpMessage = "```cs\n" +
                              "'Chomusuke Command List'\n" +
                              "```" +
                              "Use `n!command [command]` to get more info on a specific command. Ex: `n!command stats`  `[Prefix 'n!']` \n ";
            var categoryNum = 1;
            var categories =
                new List<ChomusukeCategories>((ChomusukeCategories[]) Enum.GetValues(typeof(ChomusukeCategories)));
            //removes the none enum
            categories.RemoveAt(0);

            foreach (var category in categories)
            {
                string categoryDesc = null;
                try
                {
                    categoryDesc = MiscHelpers.GetAttributeOfType<DescriptionAttribute>(category).Description;
                }
                catch (Exception)
                {
                    //do nothing
                }

                var categoryName = categoryDesc != null ? categoryDesc : category.ToString();


                helpMessage += $"\n**{categoryNum}. {categoryName} -**";

                foreach (var method in methods.ToList())
                {
                    var sub = method.GetCustomAttribute<Subject>(true);
                    if (sub.GetAdminCategories() == AdminCategories.None &&
                        sub.GetCategories() == Categories.None &&
                        sub.GetOwnerCategories() == OwnerCategories.None &&
                        sub.GetNSFWCategories() == NSFWCategories.None)
                        if (sub.GetChomusukeCategories() == category)
                        {
                            helpMessage += $" `{MiscHelpers.GetCommandAttribute(method).Text}`";
                            //removes the method from the list if successful to make it faster
                            methods.Remove(method);
                        }
                }

                categoryNum++;
            }

            helpMessage += "\n" +
                           "```\n" +
                           "# Don't include the example brackets when using commands!\n" +
                           "# To view standard commands, use n!help\n" +
                           "# To view moderator commands, use n!helpMod\n" +
                           "# To view NSFW commands, use n!helpNsfw\n" +
                           "```";
            var embed = EmbedHandler.CreateEmbed(Context, "Chomusuke Help Command", helpMessage,
                EmbedHandler.EmbedMessageType.Info,
                false);
            await SendMessage(Context, embed);
        }

        [Subject(Categories.Information)]
        [Command("command")]
        [Alias("cmd")]
        [Summary("Shows what a specific command does and the usage")]
        [Remarks("n!cmd <command you want to search up> Ex: n!cmd stats")]
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
                Color = Global.NayuColor,
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
                        $"Usage: {cmd.Remarks}\n" +
                        $"Category: {cmd.Attributes[0]}";
                    x.IsInline = false;
                });
            }

            await SendMessage(Context, builder.Build());
        }


        [Subject(Categories.Information)]
        [Command("info")]
        [Summary("Shows Nayu's information")]
        [Remarks("Ex: n!info")]
        [Cooldown(5)]
        public async Task Info()
        {
            var config = BotAccounts.GetAccount();
            var version = Config.bot.version;
            var embed = new EmbedBuilder();
            embed.WithColor(Global.NayuColor);
            embed.AddField("Creator", "Phytal#8213", true);
            embed.AddField("Last Updated", config.LastUpdate.ToShortDateString(), true);
            embed.AddField("Bot version", $"Beta {version}", true);
            embed.AddField("Vote Link", "https://discordbots.org/bot/417160957010116608/vote");
            embed.AddField("Invite Link",
                "https://discordapp.com/api/oauth2/authorize?client_id=417160957010116608&permissions=8&scope=bot");
            embed.AddField("Patreon Link", "https://www.patreon.com/phytal");
            embed.WithImageUrl(Global.Client.CurrentUser.GetAvatarUrl());

            await SendMessage(Context, embed.Build());
        }

        [Subject(Categories.Information)]
        [Command("nayuLink")]
        [Summary("Provides Nayu's server invite link")]
        [Alias("serverInviteLink")]
        [Remarks("Ex: n!nayuLink")]
        [Cooldown(5)]
        public async Task SendAsync()
        {
            var embed = new EmbedBuilder
            {
                Title = "Server Invite",
                Url = "https://discord.gg/eyHg6hS",
                Description = "^^ ~~ Here's my server! :blush:"
            };
            await SendMessage(Context, embed.Build());
        }

        [Subject(Categories.Information)]
        [Command("dailyVote")]
        [Alias("dv")]
        [Summary("Provides Nayu's daily vote link")]
        [Remarks("Ex: n!dv")]
        [Cooldown(5)]
        public async Task DailyVote()
        {
            var embed = new EmbedBuilder
            {
                Title = "Vote Link",
                Url = "https://discordbots.org/bot/417160957010116608/vote",
                Description = "^^ ~~ Vote for me please! I'll love you forever! :blush:"
            };
            await SendMessage(Context, embed.Build());
        }

        [Subject(Categories.Information)]
        [Command("invite")]
        [Summary("Invite Nayu to your server!")]
        [Alias("Nayuinvitelink")]
        [Remarks("Ex: n!invite")]
        [Cooldown(5)]
        public async Task InviteAsync()
        {
            var embed = new EmbedBuilder
            {
                Title = "Invite!",
                Url =
                    "https://discordapp.com/api/oauth2/authorize?client_id=417160957010116608&permissions=8&scope=bot",
                Description = "^^ ~~ Invite me to your servers! :blush: "
            };
            await SendMessage(Context, embed.Build());
        }

        [Subject(Categories.Information)]
        [Command("patreon")]
        [Summary("Sends the Patreon link to help contribute to the efforts of Nayu")]
        [Alias("donate")]
        [Remarks("Ex: n!patreon")]
        [Cooldown(5)]
        public async Task Patreon()
        {
            var embed = new EmbedBuilder
            {
                Title = "Donate!",
                Url = "https://www.patreon.com/phytal",
                Description = "^^ ~~ Help us out! :blush:"
            };
            await SendMessage(Context, embed.Build());
        }

        [Subject(Categories.Information)]
        [Command("changeLog")]
        [Alias("update", "cl")]
        [Summary("Shows the latest update notes")]
        [Remarks("Ex: n!changeLog")]
        [Cooldown(5)]
        public async Task Update()
        {
            var version = Config.bot.version;
            var config = BotAccounts.GetAccount();
            var embed = EmbedHandler.CreateEmbed(Context, "Update Notes",
                $"**<<Last Updated on {config.LastUpdate.ToShortDateString()}>>**\n`Version {version}`\n" +
                config.ChangeLog,
                EmbedHandler.EmbedMessageType.Success);

            await SendMessage(Context, embed);
        }
    }
}