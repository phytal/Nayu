using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Libs.CustomLibraries.Discord.Addons.Interactive.Paginator;
using Nayu.Helpers;
using Nayu.Modules.Chomusuke.Dueling;
using Nayu.Preconditions;

namespace Nayu.Modules.Chomusuke
{
    public class General : NayuModule
    {
        [Subject(ChomusukeCategories.Chomusuke)]
        [Command("chomusukeStats"), Alias("cStats")]
        [Summary("Brings up the main stats/info of your or someone else's Chomusukes!")]
        [Remarks("n!cStats <specified user (will be yours if left empty)> Ex: n!cStats @Phytal")]
        [Cooldown(5)]
        public async Task ChomusukeUser([Remainder] string arg = "")
        {
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            SocketUser user = mentionedUser ?? Context.User;
            var config = GlobalUserAccounts.GetUserAccount(user);
            if (!config.Chomusuke1.Have) //if they own a Chomusuke or not
            {
                await SendMessage(Context, null,
                    $"{Global.ENo}  **|  {Context.User.Username}**, you don't own a {Global.EChomusuke} Chomusuke! \n\nPurchase one with n!chomusuke buy!");
            }
            else //show their Chomusuke status
            {
                string sick = ConvertBool.ConvertBooleanYN(config.Chomusuke1.Sick);
                string chomusuke1 =
                    $"Owner: **{user.Username}**\nName: **{config.Chomusuke1.Name}**\nZodiac: **{config.Chomusuke1.Zodiac}** :{config.Chomusuke1.Zodiac.ToLower()}:\nType: **{config.Chomusuke1.Type}**\nTrait 1: **{config.Chomusuke1.Trait1}**\nTrait 2: **{config.Chomusuke1.Trait2}**\nCombat Power: **{config.Chomusuke1.CP}**\nExp: **{config.Chomusuke1.XP}**\nLevel: **{config.Chomusuke1.LevelNumber}**\n Control: **{config.Chomusuke1.Control}**\n Health: **{config.Chomusuke1.HealthCapacity}**\nShield: **{config.Chomusuke1.ShieldCapacity}**\nMana: **{config.Chomusuke1.ManaCapacity}**";
                string chomusuke2 = $"{user.Username} doesn't have this many chomusukes!";
                if (config.Chomusuke2.Have)
                    chomusuke2 =
                        $"Owner: **{user.Username}**\nName: **{config.Chomusuke2.Name}**\nZodiac: **{config.Chomusuke2.Zodiac}** :{config.Chomusuke2.Zodiac.ToLower()}:\nType: **{config.Chomusuke2.Type}**\nTrait 1: **{config.Chomusuke2.Trait1}**\nTrait 2: **{config.Chomusuke2.Trait2}**\nCombat Power: **{config.Chomusuke2.CP}**\nExp: **{config.Chomusuke2.XP}**\nLevel: **{config.Chomusuke2.LevelNumber}**\n Control: **{config.Chomusuke2.Control}**\n Health: **{config.Chomusuke2.HealthCapacity}**\nShield: **{config.Chomusuke2.ShieldCapacity}**\nMana: **{config.Chomusuke2.ManaCapacity}**";
                string chomusuke3 = $"{user.Username} doesn't have this many chomusukes!";
                if (config.Chomusuke3.Have)
                    chomusuke3 =
                        $"Owner: **{user.Username}**\nName: **{config.Chomusuke3.Name}**\nZodiac: **{config.Chomusuke3.Zodiac}** :{config.Chomusuke3.Zodiac.ToLower()}:\nType: **{config.Chomusuke3.Type}**\nTrait 1: **{config.Chomusuke3.Trait1}**\nTrait 2: **{config.Chomusuke3.Trait2}**\nCombat Power: **{config.Chomusuke3.CP}**\nExp: **{config.Chomusuke3.XP}**\nLevel: **{config.Chomusuke3.LevelNumber}**\n Control: **{config.Chomusuke3.Control}**\n Health: **{config.Chomusuke3.HealthCapacity}**\nShield: **{config.Chomusuke3.ShieldCapacity}**\nMana: **{config.Chomusuke3.ManaCapacity}**";

                PaginatedMessage pages = new PaginatedMessage
                {
                    Pages = new[] {chomusuke1, chomusuke2, chomusuke3}, Content = $"{user.Username}'s Chomusukes",
                    Color = Color.Blue,
                    Title = new[] {config.Chomusuke1.Name, config.Chomusuke2.Name, config.Chomusuke3.Name}
                };
                await PagedReplyAsync(pages);
            }
        }

        [Subject(ChomusukeCategories.Chomusuke)]
        [Command("activeChomusuke"), Alias("aStats")]
        [Summary("Brings up the stats/info of your or someone else's Chomusuke!")]
        [Remarks("n!aStats <specified user (will be yours if left empty)> Ex: n!aStats @Phytal")]
        [Cooldown(5)]
        public async Task Chomusuke([Remainder] string arg = "")
        {
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            SocketUser user = mentionedUser ?? Context.User;
            var config = GlobalUserAccounts.GetUserAccount(user);
            if (config.ActiveChomusuke == 0 && user == Context.User) //if they own a Chomusuke or not
            {
                await SendMessage(Context, null,
                    $"{Global.ENo}  **|**  **{Context.User.Username}**, you don't have an active {Global.EChomusuke} Chomusuke set!\n\nSet an active Chomusuke with `n!active`!");
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
                    string sick = ConvertBool.ConvertBooleanYN(config.Chomusuke1.Sick);
                    if (config.Chomusuke1.Shiny)
                        embed.WithThumbnailUrl("https://i.imgur.com/oKGxPZ4.png");
                    else
                        embed.WithThumbnailUrl("https://i.imgur.com/7kpEWPb.png");
                    embed.WithColor(Global.NayuColor);
                    embed.AddField("Owner", user, true);
                    embed.AddField("Name", config.Chomusuke1.Name, true);
                    embed.AddField("Zodiac", config.Chomusuke1.Zodiac + $":{config.Chomusuke1.Zodiac.ToLower()}:",
                        true);
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
                    string sick = ConvertBool.ConvertBooleanYN(config.Chomusuke2.Sick);
                    if (config.Chomusuke2.Shiny)
                        embed.WithThumbnailUrl("https://i.imgur.com/oKGxPZ4.png");
                    else
                        embed.WithThumbnailUrl("https://i.imgur.com/7kpEWPb.png");
                    embed.WithColor(Global.NayuColor);
                    embed.AddField("Owner", user, true);
                    embed.AddField("Name", config.Chomusuke2.Name, true);
                    embed.AddField("Zodiac", config.Chomusuke2.Zodiac + $":{config.Chomusuke2.Zodiac.ToLower()}:",
                        true);
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
                    string sick = ConvertBool.ConvertBooleanYN(config.Chomusuke3.Sick);
                    if (config.Chomusuke3.Shiny)
                        embed.WithThumbnailUrl("https://i.imgur.com/oKGxPZ4.png");
                    else
                        embed.WithThumbnailUrl("https://i.imgur.com/7kpEWPb.png");
                    embed.WithColor(Global.NayuColor);
                    embed.AddField("Owner", user, true);
                    embed.AddField("Name", config.Chomusuke3.Name, true);
                    embed.AddField("Zodiac", config.Chomusuke3.Zodiac + $":{config.Chomusuke3.Zodiac.ToLower()}:",
                        true);
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

                await SendMessage(Context, embed.Build());
            }
        }

        [Subject(Categories.Information)]
        [Command("chomusukeHelp"), Alias("cHelp")]
        [Summary("Displays all Chomusuke commands with a description of what they do")]
        [Remarks("Ex: n!cHelp")]
        [Cooldown(8)]
        public async Task ChomusukeHelp()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            string[] footers =
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
            embed.WithTitle($"{Global.EChomusuke} Chomusuke Command List");
            embed.WithColor(Global.NayuColor);
            embed.AddField("n!chomusuke help", "Brings up the help command", true);
            embed.AddField("n!chomusuke shop", "Opens the Chomusuke shop menu!", true);
            embed.AddField("n!chomusuke stats", "Brings up the stats/info of your or someone else's Chomusuke!", true);
            embed.AddField("n!chomusuke name", "Set the name of your Chomusuke!", true);
            embed.AddField("n!chomusuke feed",
                "Feeds your Chomusuke at the cost of Taiyakis! Otherwise it will starve!", true);
            embed.AddField("n!chomusuke clean", "Clean up your Chomusuke's waste, Otherwise it'll get sick!", true);
            embed.AddField("n!chomusuke play",
                "Play with your chomusuke! Your Chomusuke must have high attention levels at all times!", true);
            embed.AddField("n!chomusuke train", "Train your Chomusuke to earn Exp and level up!", true);
            embed.WithFooter(text);
            await SendMessage(Context, embed.Build());
        }

        [Subject(ChomusukeCategories.Chomusuke)]
        [Command("chomusukeName"), Alias("cName")]
        [Summary("Set the name of your Chomusuke!")]
        [Remarks("n!cName <your desired name> Ex: n!cName Taiyaki")]
        [Cooldown(8)]
        public async Task ChomusukeName([Remainder] string name)
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (config.ActiveChomusuke == 0) //if they set an active chomusuke
                await SendMessage(Context, null,
                    $"{Global.ENo}  **|**  **{Context.User.Username}**, you don't have an active {Global.EChomusuke} Chomusuke set!\n\nSet an active Chomusuke with `n!active`!");
            else
            {
                var chom = ActiveChomusuke.GetOneActiveChomusuke(config.Id);
                chom.Name = name;
                await ActiveChomusuke.ConvertOneActiveVariable(config.Id, chom);
                GlobalUserAccounts.SaveAccounts(Context.User.Id);
                await SendMessage(Context, null,
                    $"✅   **|**  **{Context.User.Username}**, you successfully changed {Global.EChomusuke} {chom.Name}'s name to **{name}**!");
            }
        }

        [Subject(ChomusukeCategories.Chomusuke)]
        [Command("chomusukeFeed"), Alias("cFeed")]
        [Summary("Feeds your Chomusuke at the cost of Taiyakis! Otherwise it will starve!")]
        [Remarks("Ex: n!cFeed")]
        [Cooldown(8)]
        public async Task ChomusukeFeed()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (config.ActiveChomusuke == 0) //if they set an active chomusuke
                await SendMessage(Context, null,
                    $"{Global.ENo}  **|**  **{Context.User.Username}**, you don't have an active {Global.EChomusuke} Chomusuke set!\n\nSet an active Chomusuke with `n!active`!");
            else
            {
                var chom = ActiveChomusuke.GetOneActiveChomusuke(config.Id);
                if (chom.Hunger == 20)
                {
                    var thumbnailurl = Context.User.GetAvatarUrl();
                    var auth = new EmbedAuthorBuilder()
                    {
                        Name = "Chiiiii..",
                        IconUrl = thumbnailurl,
                    };
                    var embed = new EmbedBuilder()
                    {
                        Author = auth
                    };
                    embed.WithColor(255, 128, 0);
                    embed.WithThumbnailUrl("https://i.imgur.com/Sc4HGir.gif");
                    embed.WithDescription(
                        $":poultry_leg:  **|**  **{Context.User.Username}**, {Global.EChomusuke} {chom.Name} is full!");
                    await SendMessage(Context, embed.Build());
                }
                else
                {
                    int cost = Global.Rng.Next(34, 87);
                    int hungerGain = Global.Rng.Next(4, 9);
                    chom.Hunger += (byte) hungerGain;
                    if (chom.Hunger > 20)
                    {
                        chom.Hunger = 20;
                    }

                    await ActiveChomusuke.ConvertOneActiveVariable(config.Id, chom);
                    GlobalUserAccounts.SaveAccounts(Context.User.Id);

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
                    embed.WithThumbnailUrl("https://i.imgur.com/Sc4HGir.gif");
                    embed.WithDescription(
                        $":poultry_leg:  **|**  **{Context.User.Username}**, you fill {Global.EChomusuke} {chom.Name}'s bowl with food. It looks happy! **(+{hungerGain} food [-{cost} {Global.ETaiyaki}])**");
                    await SendMessage(Context, embed.Build());
                }
            }
        }

        [Subject(ChomusukeCategories.Chomusuke)]
        [Command("chomusukeClean"), Alias("cClean")]
        [Summary("Clean up your Chomusuke's waste, otherwise it'll get sick!")]
        [Remarks("Ex: n!cClean")]
        [Cooldown(8)]
        public async Task ChomusukeClean()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User.Id);
            if (config.ActiveChomusuke == 0) //if they set an active chomusuke
                await SendMessage(Context, null,
                    $"{Global.ENo}  **|**  **{Context.User.Username}**, you don't have an active {Global.EChomusuke} Chomusuke set!\n\nSet an active Chomusuke with `n!active`!");
            else
            {
                var chom = ActiveChomusuke.GetOneActiveChomusuke(config.Id);
                if (chom.Waste == 0)
                {
                    var thumbnailurl = Context.User.GetAvatarUrl();
                    var auth = new EmbedAuthorBuilder()
                    {
                        Name = "Chiiiii..",
                        IconUrl = thumbnailurl,
                    };
                    var embed = new EmbedBuilder()
                    {
                        Author = auth
                    };
                    embed.WithColor(255, 128, 0);
                    embed.WithThumbnailUrl("https://i.imgur.com/OtVepvM.gif");
                    embed.WithDescription(
                        $":sparkles:  **|** **{Context.User.Username}, your {Global.EChomusuke} Chomusuke's room is squeaky clean!**");
                    await SendMessage(Context, embed.Build());
                }
                else
                {
                    int randomIndex = Global.Rng.Next(cleanTexts.Length);
                    string text = cleanTexts[randomIndex];
                    int cleanedAmount = Global.Rng.Next(4, 8);
                    chom.Waste -= (byte) cleanedAmount;
                    if (chom.Waste > 20)
                    {
                        chom.Waste = 0;
                    }

                    await ActiveChomusuke.ConvertOneActiveVariable(config.Id, chom);
                    GlobalUserAccounts.SaveAccounts(Context.User.Id);

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
                    embed.WithThumbnailUrl("https://i.imgur.com/PI2z8rm.gif");
                    embed.WithDescription(
                        $":sparkles:  **|**  **{Context.User.Username}**, {text} **(-{cleanedAmount} waste)**");
                    await SendMessage(Context, embed.Build());
                }
            }
        }

        [Subject(ChomusukeCategories.Chomusuke)]
        [Command("chomusukePlay"), Alias("cPlay")]
        [Summary("Play with your chomusuke! Your Chomusuke must have high trust at all times!")]
        [Remarks("Ex: n!cPlay")]
        [Cooldown(8)]
        public async Task ChomusukePlay()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (config.ActiveChomusuke == 0) //if they set an active chomusuke
                await SendMessage(Context, null,
                    $"{Global.ENo}  **|**  **{Context.User.Username}**, you don't have an active {Global.EChomusuke} Chomusuke set!\n\nSet an active Chomusuke with `n!active`!");
            else
            {
                var chom = ActiveChomusuke.GetOneActiveChomusuke(config.Id);
                if (chom.Trust == 20)
                {
                    var thumbnailurl = Context.User.GetAvatarUrl();
                    var auth = new EmbedAuthorBuilder()
                    {
                        Name = "Chiiiii..",
                        IconUrl = thumbnailurl,
                    };
                    var embed = new EmbedBuilder()
                    {
                        Author = auth
                    };
                    embed.WithColor(255, 128, 0);
                    embed.WithThumbnailUrl(NoPlayLinks[Global.Rng.Next(NoPlayLinks.Length)]);
                    embed.WithDescription(
                        $":soccer:  **|**  **{Context.User.Username}, your {Global.EChomusuke} Chomusuke is bored of playing right now!**");
                    await SendMessage(Context, embed.Build());
                    return;
                }

                {
                    int randomIndex = Global.Rng.Next(playTexts.Length);
                    string text = playTexts[randomIndex];
                    int trustGain = Global.Rng.Next(4, 9);
                    chom.Trust += (byte) trustGain;
                    if (chom.Trust > 20)
                    {
                        chom.Trust = 20;
                    }

                    await ActiveChomusuke.ConvertOneActiveVariable(config.Id, chom);
                    GlobalUserAccounts.SaveAccounts(Context.User.Id);
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
                    embed.WithThumbnailUrl(YesPlayLinks[Global.Rng.Next(YesPlayLinks.Length)]);
                    embed.WithDescription(
                        $":soccer:  **|**  **{Context.User.Username}**, {text} **(+{trustGain} trust)**");
                    await SendMessage(Context, embed.Build());
                }
            }
        }

        [Subject(ChomusukeCategories.Chomusuke)]
        [Command("chomusukeTrain"), Alias("cTrain")]
        [Summary("Train your Chomusuke to earn Exp and level up!")]
        [Remarks("Ex: n!cTrain")]
        [Cooldown(8)]
        public async Task ChomusukeTrain()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            if (config.ActiveChomusuke == 0) //if they set an active chomusuke
                await SendMessage(Context, null,
                    $"{Global.ENo}  **|**  **{Context.User.Username}**, you don't have an active {Global.EChomusuke} Chomusuke set!\n\nSet an active Chomusuke with `n!active`!");
            else
            {
                var chom = ActiveChomusuke.GetOneActiveChomusuke(config.Id);
                int choice = Global.Rng.Next(1, 3);

                if (choice == 1)
                {
                    uint xpGain = (uint) Global.Rng.Next(20, 30);
                    chom.XP += xpGain;
                    await ActiveChomusuke.ConvertOneActiveVariable(config.Id, chom);
                    GlobalUserAccounts.SaveAccounts(config.Id);
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
                    embed.WithThumbnailUrl(yesTrainLinks[Global.Rng.Next(yesTrainLinks.Length)]);
                    embed.WithDescription($"{text} \n**(+{xpGain} exp)**");
                    await SendMessage(Context, embed.Build());
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
                    embed.WithThumbnailUrl(noTrainLinks[Global.Rng.Next(noTrainLinks.Length)]);
                    embed.WithDescription($"{text}");
                    await SendMessage(Context, embed.Build());
                }
            }
        }

        [Subject(ChomusukeCategories.Chomusuke)]
        [Command("chomusukeAdd")]
        [RequireOwner]
        public async Task AddCapsules()
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);
            config.MythicalCapsule = +3;
            config.NormalCapsule = +3;
            config.ShinyCapsule = +3;
            GlobalUserAccounts.SaveAccounts(Context.User.Id);
            await SendMessage(Context, null, "done now u can go die");
        }

        string[] cleanTexts =
        {
            "you hold your nose and start cleaning up the mess.",
            $"you cleaned up {Global.EChomusuke} Chomusuke's...business!",
        };

        string[] playTexts =
        {
            $"you entertain your {Global.EChomusuke} Chomusuke. It seems to like you!",
            $"you throw a ball and your {Global.EChomusuke} Chomusuke fetches it!",
        };

        private readonly string[] YesPlayLinks =
        {
            "https://i.imgur.com/wFVe8Pr.gif",
            "https://i.imgur.com/8DeW6ub.gif"
        };

        private readonly string[] NoPlayLinks =
        {
            "https://i.imgur.com/gmrBEiF.gif",
            "https://i.imgur.com/lw88wr4.gif"
        };

        private readonly string[] yesTrainTexts =
        {
            $"Somehow, you managed to get {Global.EChomusuke} Chomusuke to listen! It is making progress.",
            $"Your {Global.EChomusuke} Chomusuke seems to respond well to the training! It looks happy.",
        };

        private readonly string[] noTrainTexts =
        {
            $"Despite your best attempts, your {Global.EChomusuke} Chomusuke does not want to learn. Persistence is key!",
            $"You wonder why your {Global.EChomusuke} Chomusuke won't listen. Maybe you should give it a few more tries.",
            $"Your {Global.EChomusuke} Chomusuke sits down and looks excited, but doesn't do what you wanted.",
            $"Your {Global.EChomusuke} Chomusuke doesn't quite understand what you are trying to do. Try again!",
        };

        private readonly string[] yesTrainLinks =
        {
            "https://i.imgur.com/dyB6bpn.gif",
            "https://i.imgur.com/4j5wwiQ.gif",
            "https://i.imgur.com/ok5KkQs.gif"
        };

        private readonly string[] noTrainLinks =
        {
            "https://i.imgur.com/lw88wr4.gif",
            "https://i.imgur.com/gmrBEiF.gif"
        };
    }
}