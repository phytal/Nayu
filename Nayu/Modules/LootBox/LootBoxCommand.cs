using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Helpers;
using Nayu.Preconditions;

namespace Nayu.Modules.LootBox
{
    public class LootBoxCommand : NayuModule
    {        
        [Subject(Categories.Lootboxes)]
        [Command("openLootBox")]
        [Alias("olb")]
        [Summary("Opens one of the loot boxes you have")]
        [Remarks("Usage: n!openLootBox <rarity (common/uncommon/rare/epic/legendary) Ex: n!openLootBox common")]
        public async Task OpenLootBoxCommand([Remainder] string arg)
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);

            switch (arg)
            {
                case "common":
                    if (config.LootBoxCommon > 0)
                    {
                        config.LootBoxCommon -= 1;
                        await OpenLootBox.OpenCommonBox(Context.User, (ITextChannel) Context.Channel);
                    }
                    else
                    {
                        await SendMessage(Context, null,
                            $"🛑  |  **{Context.User.Username}**, you don't have any Common Loot Boxes!");
                        return;
                    }

                    break;
                case "uncommon":
                    if (config.LootBoxUncommon > 0)
                    {
                        config.LootBoxUncommon -= 1;
                        await OpenLootBox.OpenUncommonBox(Context.User, (ITextChannel) Context.Channel);
                    }
                    else
                    {
                        await SendMessage(Context, null,
                            $"🛑  |  **{Context.User.Username}**, you don't have any Uncommon Loot Boxes!");
                        return;
                    }

                    break;
                case "rare":
                    if (config.LootBoxRare > 0)
                    {
                        config.LootBoxRare -= 1;
                        await OpenLootBox.OpenRareBox(Context.User, (ITextChannel) Context.Channel);
                    }
                    else
                    {
                        await SendMessage(Context, null,
                            $"🛑  |  **{Context.User.Username}**, you don't have any Rare Loot Boxes!");
                        return;
                    }

                    break;
                case "epic":
                    if (config.LootBoxEpic > 0)
                    {
                        config.LootBoxEpic -= 1;
                        await OpenLootBox.OpenEpicBox(Context.User, (ITextChannel) Context.Channel);
                    }
                    else
                    {
                        await SendMessage(Context, null,
                            $"🛑  |  **{Context.User.Username}**, you don't have any Epic Loot Boxes!");
                        return;
                    }

                    break;
                case "legendary":
                    if (config.LootBoxLegendary > 0)
                    {
                        config.LootBoxLegendary -= 1;
                        await OpenLootBox.OpenLegendaryBox(Context.User, (ITextChannel)Context.Channel);
                    }
                    else
                    {
                        await SendMessage(Context, null, $"🛑  |  **{Context.User.Username}**, you don't have any Legendary Loot Boxes!");
                        return;
                    }

                    break;
            }
            GlobalUserAccounts.SaveAccounts(Context.User.Id);
        }
        
        [Subject(Categories.Lootboxes)]
        [Command("lootBoxInventory"), Alias("lbi")]
        [Summary("View your loot box inventory")]
        [Remarks("Usage: n!inventory")]
        public async Task LootBoxInventory()
        {
            var account = GlobalUserAccounts.GetUserAccount(Context.User);
            var embed = new EmbedBuilder();
            embed.WithTitle($"{Context.User.Username}'s Loot Box Inventory");

            embed.AddField("Common Loot Boxes", $"**x{account.LootBoxCommon}**");
            embed.AddField("Uncommon Loot Boxes", $"**x{account.LootBoxUncommon}**");
            embed.AddField("Rare loot Boxes", $"**x{account.LootBoxRare}**");
            embed.AddField("Epic Loot Boxes", $"**x{account.LootBoxEpic}**");
            embed.AddField("Legendary Loot Boxes", $"**x{account.LootBoxLegendary}**");
            embed.WithFooter("You can get Loot Boxes from increasing your Nayu Level (not server level) and winning duels!");

            await SendMessage(Context, embed.Build());
        }
        
        [Subject(OwnerCategories.Owner)]
        [Command("addLootBox"), Alias("alb")]
        [Summary("Adds some loot boxes to a person")]
        [Remarks("Usage: n!alb <user>")]
        [RequireOwner]
        public async Task AddLootBox([Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            var account = GlobalUserAccounts.GetUserAccount(target);
            account.LootBoxCommon += 1;
            account.LootBoxUncommon += 1;
            account.LootBoxRare += 1;
            account.LootBoxEpic += 1;
            account.LootBoxLegendary += 1;
            GlobalUserAccounts.SaveAccounts(account.Id);

            await SendMessage(Context, null, $"Successfully added one of every loot box to {target}");
        }
        
        [Subject(OwnerCategories.Owner)]
        [Command("clearLootBox"), Alias("clb")]
        [Summary("Clears a person's loot boxes")]
        [Remarks("Usage: n!clb <user>")]
        [RequireOwner]
        public async Task ClearLootBox([Remainder] string arg = "")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            var account = GlobalUserAccounts.GetUserAccount(target);
            account.LootBoxCommon = 0;
            account.LootBoxUncommon = 0;
            account.LootBoxRare = 0;
            account.LootBoxEpic = 0;
            account.LootBoxLegendary = 0;
            GlobalUserAccounts.SaveAccounts(account.Id);

            await SendMessage(Context, null, $"Successfully cleared {target}'s loot boxes");
        }
        
        [Subject(Categories.Lootboxes)]
        [Command("giftLootBox")]
        [Alias("giftlb", "grantlb", "glb")]
        [Summary("Gifts a loot box to a selected user from your arsenal of loot boxes Ex: n!giftlb epic @user")]
        [Remarks("n!giftlb <rarity> <user you want to gift to> Ex: n!giftlb rare @Phytal")]
        [Cooldown(5)]
        public async Task Gift(string Rarity, IGuildUser userB)
        {
            var giveaccount = GlobalUserAccounts.GetUserAccount(Context.User);

            Rarity = Rarity.ToUpper();
            uint numOfLootboxes = 0;
            if (Rarity == "COMMON") numOfLootboxes = giveaccount.LootBoxCommon;
            if (Rarity == "UNCOMMON") numOfLootboxes = giveaccount.LootBoxUncommon;
            if (Rarity == "RARE") numOfLootboxes = giveaccount.LootBoxRare;
            if (Rarity == "EPIC") numOfLootboxes = giveaccount.LootBoxEpic;
            if (Rarity == "LEGENDARY") numOfLootboxes = giveaccount.LootBoxLegendary;

            if (numOfLootboxes < 1)
            {
                await ReplyAsync(":angry:  | Stop trying to gift loot boxes you don't have!");
            }
            else
            {
                if (userB == null)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(Global.NayuColor);
                    embed.WithTitle("🖐️ | Please say who you want to gift loot boxes to. Ex: n!gift <rarity of loot box> @user");
                    await SendMessage(Context, embed.Build());
                }
                else
                {
                    var receiver = GlobalUserAccounts.GetUserAccount((SocketUser)userB);

                    if (Rarity == "COMMON") { giveaccount.LootBoxCommon--; receiver.LootBoxCommon++; }
                    if (Rarity == "UNCOMMON") { giveaccount.LootBoxUncommon--; receiver.LootBoxUncommon++; }
                    if (Rarity == "RARE") { giveaccount.LootBoxRare--; receiver.LootBoxRare++; }
                    if (Rarity == "EPIC") { giveaccount.LootBoxEpic--; receiver.LootBoxEpic++; }
                    if (Rarity == "LEGENDARY") { giveaccount.LootBoxLegendary--; receiver.LootBoxLegendary++; }

                    GlobalUserAccounts.SaveAccounts(giveaccount.Id, receiver.Id);

                    await SendMessage(Context, null, $":gift:  | {Context.User.Mention} has gifted {userB.Mention} a **{Rarity}** Loot box! How generous.");
                }
            }
        }
    }
}
