using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using Nayu.Core.Modules;
using Nayu.Features.GlobalAccounts;
using Nayu.Preconditions;
using Discord.Addons.Interactive;

namespace Nayu.Modules.Chomusuke
{
    public class General : NayuModule
    {

        [Command("chomusuke stats"), Alias("c stats")]
        [Summary("Brings up the main stats/info of your or someone else's Chomusukes!")]
        [Remarks("n!c stats <specified user (will be yours if left empty)> Ex: n!c stats @Phytal")]
        [Cooldown(5)]
        public async Task ChomusukeUser([Remainder]string arg = "")
        {
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            SocketUser user = mentionedUser ?? Context.User;
            var config = GlobalUserAccounts.GetUserAccount(user);
            if (config.Chomusuke1.Have == false) //if they own a Chomusuke or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke! \n\nPurchase one with n!chomusuke buy!");
                return;
            }
            else //show their Chomusuke status
            {
                string sick = Helpers.ConvertBool.ConvertBooleanYN(config.Chomusuke1.Sick);
                string chomusuke1 = $"Owner: **{user.Username}**\nName: **{config.Chomusuke1.Name}**\nZodiac: **{config.Chomusuke1.Zodiac}** :{config.Chomusuke1.Zodiac.ToLower()}:\nType: **{config.Chomusuke1.Type}**\nTrait 1: **{config.Chomusuke1.Trait1}**\nTrait 1: **{config.Chomusuke1.Trait2}**\nCombat Power: **{config.Chomusuke1.CP}**\nExp: **{config.Chomusuke1.XP}**\nLevel: **{config.Chomusuke1.LevelNumber}**\n Control: **{config.Chomusuke1.Control}**\n Health: **{config.Chomusuke1.HealthCapacity}**\nShield: **{config.Chomusuke1.ShieldCapacity}**\nMana: **{config.Chomusuke1.ManaCapacity}**";
                string chomusuke2 = $"{user.Username} doesn't have this many chomusukes!";
                if (config.Chomusuke2.Have) chomusuke2 = $"Owner: **{user.Username}**\nName: **{config.Chomusuke2.Name}**\nZodiac: **{config.Chomusuke2.Zodiac}** :{config.Chomusuke2.Zodiac.ToLower()}:\nType: **{config.Chomusuke2.Type}**\nTrait 1: **{config.Chomusuke2.Trait1}**\nTrait 1: **{config.Chomusuke2.Trait2}**\nCombat Power: **{config.Chomusuke2.CP}**\nExp: **{config.Chomusuke2.XP}**\nLevel: **{config.Chomusuke2.LevelNumber}**\n Control: **{config.Chomusuke2.Control}**\n Health: **{config.Chomusuke2.HealthCapacity}**\nShield: **{config.Chomusuke2.ShieldCapacity}**\nMana: **{config.Chomusuke2.ManaCapacity}**";
                string chomusuke3 = $"{user.Username} doesn't have this many chomusukes!";
                if (config.Chomusuke3.Have) chomusuke3 = $"Owner: **{user.Username}**\nName: **{config.Chomusuke3.Name}**\nZodiac: **{config.Chomusuke3.Zodiac}** :{config.Chomusuke3.Zodiac.ToLower()}:\nType: **{config.Chomusuke3.Type}**\nTrait 1: **{config.Chomusuke3.Trait1}**\nTrait 1: **{config.Chomusuke3.Trait2}**\nCombat Power: **{config.Chomusuke3.CP}**\nExp: **{config.Chomusuke3.XP}**\nLevel: **{config.Chomusuke3.LevelNumber}**\n Control: **{config.Chomusuke3.Control}**\n Health: **{config.Chomusuke3.HealthCapacity}**\nShield: **{config.Chomusuke3.ShieldCapacity}**\nMana: **{config.Chomusuke3.ManaCapacity}**";

                PaginatedMessage pages = new PaginatedMessage { Pages = new[] { chomusuke1, chomusuke2, chomusuke3 }, Content = $"{user.Username}'s Chomusukes", Color = Color.Blue, Title = new[] { config.Chomusuke1.Name, config.Chomusuke2.Name, config.Chomusuke3.Name } };
                await PagedReplyAsync(pages);
            }
        }

        [Command("activechomusuke"), Alias("a stats")]
        [Summary("Brings up the stats/info of your or someone else's Chomusuke!")]
        [Remarks("n!c stats <specified user (will be yours if left empty)> Ex: n!c stats @Phytal")]
        [Cooldown(5)]
        public async Task Chomusuke([Remainder]string arg = "")
        {
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            SocketUser user = mentionedUser ?? Context.User;
            var config = GlobalUserAccounts.GetUserAccount(user);
            if (config.Chomusuke1.Have == false) //if they own a Chomusuke or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke! \n\nPurchase one with n!chomusuke buy!");
                return;
            }
            else //show their Chomusuke status
            {
                var thumbnailurl = Context.User.GetAvatarUrl();
                var auth = new EmbedAuthorBuilder()
                {
                    Name = $"{user.Username}'s Active Chomusuke",
                    IconUrl = thumbnailurl,
                };
                var embed = new EmbedBuilder()
                {
                    Author = auth
                };
                if (config.ActiveChomusuke == 1)
                {
                    string sick = Helpers.ConvertBool.ConvertBooleanYN(config.Chomusuke1.Sick);
                    if (config.Chomusuke1.Shiny == true)
                        embed.WithThumbnailUrl("https://i.imgur.com/oKGxPZ4.png");
                    else
                        embed.WithThumbnailUrl("https://i.imgur.com/7kpEWPb.png");
                    embed.WithColor(37, 152, 255);
                    embed.AddField("Owner", user, true);
                    embed.AddField("Name", config.Chomusuke1.Name, true);
                    embed.AddField("Zodiac", config.Chomusuke1.Zodiac + $":{config.Chomusuke1.Zodiac.ToLower()}:", true);
                    embed.AddField("Type", config.Chomusuke1.Type, true);
                    embed.AddField("Combat Power", config.Chomusuke1.CP, true);
                    embed.AddField("Exp", config.Chomusuke1.XP, true);
                    embed.AddField("Level", config.Chomusuke1.LevelNumber, true);
                    embed.AddField("Control", config.Chomusuke1.Control, true);
                    embed.AddField("Health", config.Chomusuke1.HealthCapacity, true);
                    embed.AddField("Shield", config.Chomusuke1.ShieldCapacity, true);
                    embed.AddField("Mana", config.Chomusuke1.ManaCapacity, true);
                    embed.AddField("Waste", config.Chomusuke1.Waste, true);
                    embed.AddField("Trust", config.Chomusuke1.Trust, true);
                    embed.AddField("Hunger", config.Chomusuke1.Hunger, true);
                    embed.AddField("Sick", sick, true);
                }
                if (config.ActiveChomusuke == 2)
                {
                    string sick = Helpers.ConvertBool.ConvertBooleanYN(config.Chomusuke2.Sick);
                    if (config.Chomusuke2.Shiny == true)
                        embed.WithThumbnailUrl("https://i.imgur.com/oKGxPZ4.png");
                    else
                        embed.WithThumbnailUrl("https://i.imgur.com/7kpEWPb.png");
                    embed.WithColor(37, 152, 255);
                    embed.AddField("Owner", user, true);
                    embed.AddField("Name", config.Chomusuke2.Name, true);
                    embed.AddField("Zodiac", config.Chomusuke2.Zodiac + $":{config.Chomusuke2.Zodiac.ToLower()}:", true);
                    embed.AddField("Type", config.Chomusuke2.Type, true);
                    embed.AddField("Combat Power", config.Chomusuke2.CP, true);
                    embed.AddField("Exp", config.Chomusuke2.XP, true);
                    embed.AddField("Level", config.Chomusuke2.LevelNumber, true);
                    embed.AddField("Control", config.Chomusuke2.Control, true);
                    embed.AddField("Health", config.Chomusuke2.HealthCapacity, true);
                    embed.AddField("Shield", config.Chomusuke2.ShieldCapacity, true);
                    embed.AddField("Mana", config.Chomusuke2.ManaCapacity, true);
                    embed.AddField("Waste", config.Chomusuke2.Waste, true);
                    embed.AddField("Trust", config.Chomusuke2.Trust, true);
                    embed.AddField("Hunger", config.Chomusuke2.Hunger, true);
                    embed.AddField("Sick", sick, true);
                }
                if (config.ActiveChomusuke == 3)
                {
                    string sick = Helpers.ConvertBool.ConvertBooleanYN(config.Chomusuke3.Sick);
                    if (config.Chomusuke3.Shiny == true)
                        embed.WithThumbnailUrl("https://i.imgur.com/oKGxPZ4.png");
                    else
                        embed.WithThumbnailUrl("https://i.imgur.com/7kpEWPb.png");
                    embed.WithColor(37, 152, 255);
                    embed.AddField("Owner", user, true);
                    embed.AddField("Name", config.Chomusuke3.Name, true);
                    embed.AddField("Zodiac", config.Chomusuke3.Zodiac + $":{config.Chomusuke3.Zodiac.ToLower()}:", true);
                    embed.AddField("Type", config.Chomusuke3.Type, true);
                    embed.AddField("Combat Power", config.Chomusuke3.CP, true);
                    embed.AddField("Exp", config.Chomusuke3.XP, true);
                    embed.AddField("Level", config.Chomusuke3.LevelNumber, true);
                    embed.AddField("Control", config.Chomusuke3.Control, true);
                    embed.AddField("Health", config.Chomusuke3.HealthCapacity, true);
                    embed.AddField("Shield", config.Chomusuke3.ShieldCapacity, true);
                    embed.AddField("Mana", config.Chomusuke3.ManaCapacity, true);
                    embed.AddField("Waste", config.Chomusuke3.Waste, true);
                    embed.AddField("Trust", config.Chomusuke3.Trust, true);
                    embed.AddField("Hunger", config.Chomusuke3.Hunger, true);
                    embed.AddField("Sick", sick, true);
                }

                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }
        [Command("chomusuke help"), Alias("w help")]
        [Summary("Displays all Chomusuke commands with a description of what they do")]
        [Remarks("Ex: n!help")]
        [Cooldown(8)]
        public async Task ChomusukeHelp()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            string[] footers = new string[]
{
                "Every 4 hours all Chomusukes will have a time modifier, -1 hunger, -1 attention, and +1 waste. Make sure to check on your Chomusuke often!",
                "If the living conditions you provide for your Chomusuke are too low - never clean, never play, etc - it will run away! (Your room will remain the same)",
                "If your Chomusuke is sick, buy it some medicine with n!buy.",
                "All Chomusuke commands have a 8 second cooldown.",
                "To get a direct link to a picture right click and open image in new tab. Then there's the URL! :)"
};
            Random rand = new Random();
            int randomIndex = rand.Next(footers.Length);
            string text = footers[randomIndex];

            var embed = new EmbedBuilder();
            embed.WithTitle($"{Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke Command List");
            embed.AddField("n!chomusuke help", "Brings up the help commmand (lol)", true);
            embed.AddField("n!chomusuke shop", "Opens the Chomusuke shop menu!", true);
            embed.AddField("n!chomusuke stats", "Brings up the stats/info of your or someone else's Chomusuke!", true);
            embed.AddField("n!chomusuke name", "Set the name of your Chomusuke!", true);
            embed.AddField("n!chomusuke feed", "Feeds your Wasagtochi at the cost of Taiyakis! Otherwise it will starve!", true);
            embed.AddField("n!chomusuke clean", "Clean up your Chomusuke's waste, Otherwise it'll get sick!", true);
            embed.AddField("n!chomusuke play", "Play with your chomusuke! Your Chomusuke must have high attention levels at all times!", true);
            embed.AddField("n!chomusuke train", "Train your Chomusuke to earn Exp and level up!", true);
            embed.WithFooter(text);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("chomusuke name"), Alias("w name")]
        [Summary("Set the name of your Chomusuke!")]
        [Remarks("n!c name <your desired name> Ex: n!c name Taiyaki")]
        [Cooldown(8)]
        public async Task ChomusukeName([Remainder] string name)
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (config.Chomusuke1.Have == false) //if they own a Chomusuke or not
            {
                var no = Emote.Parse("<:no:453716729525174273>");
                await Context.Channel.SendMessageAsync($"{no}  |  **{Context.User.Username}**, you don't own a {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke! \n\nPurchase one with n!chomusuke buy!");
                GlobalUserAccounts.SaveAccounts(Context.User.Id);
                return;
            }
            else
            {
                config.Chomusuke1.Name = name;
                GlobalUserAccounts.SaveAccounts(Context.User.Id);
                await Context.Channel.SendMessageAsync($":white_check_mark:   |  **{Context.User.Username}**, you successfully changed your {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke's name to **{name}**!");
            }
        }


        [Command("chomusuke feed"), Alias("w feed")]
        [Summary("Feeds your Wasagtochi at the cost of Taiyakis! Otherwise it will starve!")]
        [Remarks("Ex: n!c feed")]
        [Cooldown(8)]
        public async Task ChomusukeFeed()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (config.Chomusuke1.Have == false) //if they own a Chomusuke or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke! \n\nPurchase one with n!chomusuke buy!");
                GlobalUserAccounts.SaveAccounts(Context.User.Id);
                return;
            }
            else
            {
                if (config.Chomusuke1.Hunger == 20)
                {
                    await Context.Channel.SendMessageAsync($":poultry_leg:  |  **{Context.User.Username}**, your {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke is full!");
                    return;
                }
                {
                    int cost = Global.Rng.Next(34, 87);
                    int hungerGain = Global.Rng.Next(4, 9);
                    config.Chomusuke1.Hunger += (byte)hungerGain;
                    if (config.Chomusuke1.Hunger > 20)
                    {
                        config.Chomusuke1.Hunger = 20;
                    }
                    GlobalUserAccounts.SaveAccounts(Context.User.Id);
                    await Context.Channel.SendMessageAsync($":poultry_leg:  |  **{Context.User.Username}**, you fill your {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke's bowl with food. It looks happy! **(+{hungerGain} food [-{cost} {Emote.Parse("<:taiyaki:599774631984889857>")}])**");
                }
            }
        }


        string[] cleanTexts = new string[]
{
                "you hold your nose and start cleaning up the mess. ",
                "you cleaned up <:chomusuke:601183653657182280> Chomusuke's...business!",
};

        [Command("chomusuke clean"), Alias("w clean")]
        [Summary("Clean up your Chomusuke's waste, Otherwise it'll get sick!")]
        [Remarks("Ex: n!c clean")]
        [Cooldown(8)]
        public async Task ChomusukeClean()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User.Id);
            if (config.Chomusuke1.Have == false) //if they own a Chomusuke or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke! \n\nPurchase one with n!chomusuke buy!");
                GlobalUserAccounts.SaveAccounts(Context.User.Id);
                return;
            }
            else
            {
                if (config.Chomusuke1.Waste == 0)
                {
                    await Context.Channel.SendMessageAsync($":sparkles:  | **{Context.User.Username}, your {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke's room is squeaky clean!**");
                    return;
                }
                {
                    int randomIndex = Global.Rng.Next(cleanTexts.Length);
                    string text = cleanTexts[randomIndex];
                    int cleanedAmount = Global.Rng.Next(4, 8);
                    config.Chomusuke1.Waste -= (byte)cleanedAmount;
                    if (config.Chomusuke1.Waste > 20)
                    {
                        config.Chomusuke1.Waste = 0;
                    }
                    GlobalUserAccounts.SaveAccounts(Context.User.Id);
                    await Context.Channel.SendMessageAsync($":sparkles:  |  **{Context.User.Username}**, {text} **(-{cleanedAmount} waste)**");
                }
            }
        }
        string[] playTexts = new string[]
    {
                "you entertain your <:chomusuke:601183653657182280> Chomusuke. It seems to like you!",
                "you throw a ball and your <:chomusuke:601183653657182280> Chomusuke fetches it!",
    };

        [Command("chomusuke play"), Alias("w play")]
        [Summary("Play with your chomusuke! Your Chomusuke must have high trust at all times!")]
        [Remarks("Ex: n!c play")]
        [Cooldown(8)]
        public async Task ChomusukePlay()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (config.Chomusuke1.Have == false) //if they own a Chomusuke or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke! \n\nPurchase one with n!chomusuke buy!");
                GlobalUserAccounts.SaveAccounts(Context.User.Id);
                return;
            }
            else
            {
                if (config.Chomusuke1.Trust == 20)
                {
                    await Context.Channel.SendMessageAsync($":soccer:  |  **{Context.User.Username}, your {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke is bored of playing right now!**");
                    return;
                }
                {
                    int randomIndex = Global.Rng.Next(playTexts.Length);
                    string text = playTexts[randomIndex];
                    int trustGain = Global.Rng.Next(4, 9);
                    config.Chomusuke1.Trust += (byte)trustGain;
                    if (config.Chomusuke1.Trust > 20)
                    {
                        config.Chomusuke1.Trust = 20;
                    }
                    GlobalUserAccounts.SaveAccounts(Context.User.Id);
                    await Context.Channel.SendMessageAsync($":soccer:  |  **{Context.User.Username}**, {text} **(+{trustGain} trust)**");
                }
            }
        }

        string[] yesTrainTexts = new string[]
{
                "Somehow, you managed to get <:chomusuke:601183653657182280> Chomusuke to listen! It is making progress.",
                "Your <:chomusuke:601183653657182280> Chomusuke seems to respond well to the training! It looks happy.",
};

        string[] noTrainTexts = new string[]
{
                "Despite your best attempts, your <:chomusuke:601183653657182280> Chomusuke does not want to learn. Persistence is key!",
                "You wonder why your <:chomusuke:601183653657182280> Chomusuke won't listen. Maybe you should give it a few more tries.",
                "Your <:chomusuke:601183653657182280> Chomusuke sits down and looks excited, but doesn't do what you wanted.",
                "Your <:chomusuke:601183653657182280> Chomusuke doesn't quite understand what you are trying to do. Try again!",
};

        [Command("chomusuke train"), Alias("w train")]
        [Summary("Train your Chomusuke to earn Exp and level up!")]
        [Remarks("Ex: n!c train")]
        [Cooldown(8)]
        public async Task ChomusukeTrain()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (config.Chomusuke1.Have == false) //if they own a Chomusuke or not
            {
                await Context.Channel.SendMessageAsync($":no:  |  **{Context.User.Username}**, you don't own a {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusuke! \n\nPurchase one with n!chomusuke buy!");
                GlobalUserAccounts.SaveAccounts(Context.User.Id);
                return;
            }
            else
            {
                int choice = Global.Rng.Next(1, 3);

                if (choice == 1)
                {
                    uint xpGain = (uint)Global.Rng.Next(20, 30);
                    config.XP += xpGain;
                    GlobalUserAccounts.SaveAccounts(Context.User.Id);
                    int randomIndex = Global.Rng.Next(yesTrainTexts.Length);
                    string text = yesTrainTexts[randomIndex];
                    var thumbnailurl = Context.User.GetAvatarUrl();
                    var auth = new EmbedAuthorBuilder()
                    {
                        Name = "Success!",
                        IconUrl = thumbnailurl,
                    };
                    var embed = new EmbedBuilder()
                    {
                        Author = auth
                    };
                    embed.WithColor(0, 255, 0);
                    embed.WithThumbnailUrl("https://i.imgur.com/4j5wwiQ.gifv");
                    embed.WithDescription($"{text} \n**(+{xpGain} exp)**");
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
                if (choice == 2)
                {
                    int randomIndex = Global.Rng.Next(yesTrainTexts.Length);
                    string text = noTrainTexts[randomIndex];
                    var thumbnailurl = Context.User.GetAvatarUrl();
                    var auth = new EmbedAuthorBuilder()
                    {
                        Name = "Try again!",
                        IconUrl = thumbnailurl,
                    };
                    var embed = new EmbedBuilder()
                    {
                        Author = auth
                    };
                    embed.WithColor(255, 0, 0);
                    embed.WithThumbnailUrl("https://i.imgur.com/lw88wr4.gifv");
                    embed.WithDescription($"{text}");
                    await Context.Channel.SendMessageAsync("", embed: embed.Build());
                }
            }
        }
        [Command("chomusuke add")]
        public async Task AddCapsules()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            config.MythicalCapsule = +3;
            config.NormalCapsule = +3;
            config.ShinyCapsule = +3;
            GlobalUserAccounts.SaveAccounts(Context.User.Id);
            await Context.Channel.SendMessageAsync("done now u can go die");
        }
    }
}
