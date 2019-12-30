using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Preconditions;

namespace Nayu.Modules.Admin.Commands.Management
{
    public class Warning : NayuModule
    {
        [Command("Warn")]
        [Summary("Warns a User")]
        [Remarks("n!carn <user you want to warn> <reason> Ex: n!carn @Phytal bullied my brother")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [Cooldown(5)]
        public async Task WarnUser(IGuildUser user, [Remainder]string reason = "No reason provided.")
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.BanMembers)
            {
                try
                {
                    var userAccount = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)user);
                    var dmchannel = await user.GetOrCreateDMChannelAsync();
                    userAccount.NumberOfWarnings++;
                    userAccount.Warnings.Add(reason);
                    GlobalGuildUserAccounts.SaveAccounts();

                    if (userAccount.NumberOfWarnings >= 5)
                    {
                        await user.Guild.AddBanAsync(user);
                        try
                        {
                            await dmchannel.SendMessageAsync($":exclamation:  **You have been banned from** ***{Context.Guild}*** ** from having too many warnings.**");
                        }
                        catch
                        {
                            await Context.Channel.SendMessageAsync($":exclamation:  **{user.Mention} has been banned from** ***{Context.Guild}*** ** from having too many warnings.** \n*This message was shown in a server text channel because you had DMs turned off.*");
                        }
                        await Context.Channel.SendMessageAsync($"Successfully warned and banned**{user.Username}** for **{reason}**. **({userAccount.NumberOfWarnings}/5)**");
                    }
                    else if (userAccount.NumberOfWarnings == 3 || userAccount.NumberOfWarnings == 4)
                    {
                        await user.KickAsync();
                        try
                        {
                            await dmchannel.SendMessageAsync($":exclamation:  **You have been kicked from** ***{Context.Guild}*** **. Think over your actions and you may rejoin the server once you are ready. (5 Warnings = Ban)**");
                        }
                        catch
                        {
                            await Context.Channel.SendMessageAsync($":exclamation:  **{user.Mention} has been kicked from** ***{Context.Guild}*** **. Think over your actions and you may rejoin the server once you are ready. (5 Warnings = Ban)** \n*This message was shown in a server text channel because you had DMs turned off.*");
                        }
                        await Context.Channel.SendMessageAsync($"Successfully warned and kicked **{user.Username}** for **{reason}**. **({userAccount.NumberOfWarnings}/5)**");
                    }
                    else if (userAccount.NumberOfWarnings == 1 || userAccount.NumberOfWarnings == 2)
                    {
                        try
                        {
                            await dmchannel.SendMessageAsync($":exclamation:  **You have been warned in** ***{Context.Guild}*** **. (5 Warnings = Ban)**");
                        }
                        catch
                        {
                            await Context.Channel.SendMessageAsync($":exclamation:  **{user.Mention} has been warned in** ***{Context.Guild}*** **. (5 Warnings = Ban)**\n*This message was shown in a server text channel because you had DMs turned off.*");
                        }
                        await Context.Channel.SendMessageAsync($"Successfully warned **{user.Username}** for **{reason}**. **({userAccount.NumberOfWarnings}/5)**");
                    }
                }
                catch
                {
                    if (user == null)
                    {
                        var embed = new EmbedBuilder();
                        embed.WithColor(37, 152, 255);
                        embed.WithTitle(":hand_splayed:  | Please say who you want to warn and a reason for their warning. Ex: n!carn @Phytal bullied my brother");
                        await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
                    }
                }
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Ban Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("Warnings")]
        [Summary("Shows all of a user's warnings")]
        [Remarks("n!carnings <user whose warnings you want to look at> Ex: n!carnings @Phytal")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [Cooldown(5)]
        public async Task Warnings(IGuildUser user)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.BanMembers)
            {
                var num = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)user).NumberOfWarnings;
                var warnings = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)user).Warnings;
                var embed = new EmbedBuilder();
                embed.WithTitle($"{user}'s Warnings");
                embed.WithDescription($"Total of **{num}** warnings");
                for (var i = 0; i < warnings.Count; i++)
                {
                    embed.AddField($"Warning #{i + 1}: ", warnings[i], true);
                }
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Ban Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }

        [Command("ClearWarnings")]
        [Summary("Clears all of a user's warnings")]
        [Alias("cw")]
        [Remarks("n!cw <user whose warnings you want to clear> Ex: n!cw @Phytal")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [Cooldown(5)]
        public async Task ClearWarnings(IGuildUser user)
        {
            var guser = Context.User as SocketGuildUser;
            if (guser.GuildPermissions.BanMembers)
            {
                var userAccount = GlobalGuildUserAccounts.GetUserID((SocketGuildUser)user);
                userAccount.NumberOfWarnings = 0;
                userAccount.Warnings.Clear();
                GlobalGuildUserAccounts.SaveAccounts();

                await Context.Channel.SendMessageAsync($":white_check_mark:  Succesfully cleared all of **{user.Username}'s** warnings.");
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithColor(37, 152, 255);
                embed.Title = $":x:  | You Need the Ban Members Permission to do that {Context.User.Username}";
                await ReplyAndDeleteAsync("", embed: embed.Build(), timeout: TimeSpan.FromSeconds(5));
            }
        }
    }
}
