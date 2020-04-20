using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Helpers;
using Nayu.Modules.Chomusuke.Dueling;

namespace Nayu.Modules.Chomusuke
{
    public class Shop : NayuModule
    {
        [Subject(ChomusukeCategories.Chomusuke)]
        [Command("chomusukeBuy"), Alias("cShop", "cBuy")]
        [Summary("Opens the Chomusuke shop menu!")]
        [Remarks("Ex: n!cShop")]
        public async Task ChomusukeBuy()
        {
            var user = Context.User as SocketGuildUser ?? Context.User;
            var config = GlobalUserAccounts.GetUserAccount(user);
            var activeChomusuke = ActiveChomusuke.GetOneActiveChomusuke(user.Id);
            var shopText =
                "🏬  **|  Chomusuke Shop** \n ```xl\nPlease select the purchase you would like to make.\n\n[1] Capsules\n[2] Boosts\n[3] Items\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.```";
            var shop = await Context.Channel.SendMessageAsync(shopText);
            var response = await NextMessageAsync();
            if (response == null)
            {
                await shop.ModifyAsync(m =>
                {
                    m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity";
                });
            }

            else if (response.Content.Equals("1"))
            {
                await shop.ModifyAsync(m =>
                {
                    m.Content =
                        $"🐾  |  **Are you sure you want to purchase a {Global.EChomusuke} Chomusuke? (**900** {Global.ETaiyaki})**\n\nType `confirm` to continue or `cancel` to cancel.";
                });
                var newResponse = await NextMessageAsync();
                if (newResponse == null)
                {
                    await shop.ModifyAsync(m =>
                    {
                        m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity";
                    });
                }

                else if (newResponse.Content.Equals("confirm", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (config.Taiyaki < 900)
                    {
                        await shop.ModifyAsync(m =>
                        {
                            m.Content =
                                $"**{Global.ENo}  |  {Context.User.Username}, you don't have enough Taiyakis for that! **You require **{900 - config.Taiyaki}** more Taiyakis!";
                        });
                        return;
                    }

                    config.NormalCapsule += 1;
                    config.Taiyaki -= 900;
                    GlobalUserAccounts.SaveAccounts(user.Id);
                    await SendMessage(Context, null,
                        $"You have successfully bought a {Global.EChomusuke} Normal Chomusuke Capsule!");
                }

                else if (newResponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
                {
                    await shop.ModifyAsync(m =>
                    {
                        m.Content = $"🐾 **|**  **{Context.User.Username}**, purchase cancelled.";
                    });
                }

                else
                {
                    await shop.ModifyAsync(m =>
                    {
                        m.Content = $"{Global.ENo} **|** That is an invalid response. Please try again.";
                    });
                }
            }

            else if (response.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
            {
                await shop.ModifyAsync(m =>
                {
                    m.Content = $"🐾 **|**  **{Context.User.Username}**, purchase cancelled.";
                });
            }

            //TODO: add boosts
            else if (response.Content.Equals("2"))
            {
                await shop.ModifyAsync(m =>
                {
                    m.Content =
                        $"```xl\n[1] Megumin - takes care of your Chomusuke for a week! [2000 {Global.ETaiyaki}]\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.```";
                });
                var newResponse = await NextMessageAsync();
                if (newResponse == null)
                {
                    await shop.ModifyAsync(m =>
                    {
                        m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity";
                    });
                }

                else if (newResponse.Content.Equals("1"))
                {
                    await ChooseChomusuke.ChooseActionAsync(user, Actions.Megumin);
                    config.Taiyaki -= 2000;
                    GlobalUserAccounts.SaveAccounts(config.Id);
                    await shop.ModifyAsync(m =>
                    {
                        m.Content =
                            $"{Global.EMegumin}  |  **{Context.User.Username}**, your {Global.EChomusuke} is now under Megumin's care for a week!";
                    });
                }

                else if (newResponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
                {
                    await shop.ModifyAsync(m =>
                    {
                        m.Content = $"🐾 **|**  **{Context.User.Username}**, purchase cancelled.";
                    });
                }

                else
                {
                    await shop.ModifyAsync(m =>
                    {
                        m.Content = $"{Global.ENo} **|** That is an invalid response. Please try again.";
                    });
                }
            }

            else if (response.Content.Equals("3"))
            {
                await shop.ModifyAsync(m =>
                {
                    m.Content =
                        $"```xl\n[1] Medicine - cures your Chomusuke's sickness [400 {Global.ETaiyaki}]\n\nType the respective number beside the purchase you would like to select.\nType 'cancel' to cancel your purchase.```";
                });
                var newResponse = await NextMessageAsync();
                if (newResponse == null)
                {
                    await shop.ModifyAsync(m =>
                    {
                        m.Content = $"{Context.User.Mention}, The interface has closed due to inactivity";
                    });
                }
                else if (newResponse.Content.Equals("1"))
                {
                    await ChooseChomusuke.ChooseActionAsync(user, Actions.Cure);
                    config.Taiyaki -= 400;
                    GlobalUserAccounts.SaveAccounts(config.Id);
                    await shop.ModifyAsync(m =>
                    {
                        m.Content =
                            $"💊  |  **{Context.User.Username}**, your {Global.EChomusuke} Chomusuke has been cured of it's sickness! Make sure to keep looking after it!";
                    });
                }

                else if (newResponse.Content.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
                {
                    await shop.ModifyAsync(m =>
                    {
                        m.Content = $"🐾 **|**  **{Context.User.Username}**, purchase cancelled.";
                    });
                }

                else
                {
                    await shop.ModifyAsync(m =>
                    {
                        m.Content = $"{Global.ENo} **|** That is an invalid response. Please try again.";
                    });
                }
            }
            else
            {
                await shop.ModifyAsync(m =>
                {
                    m.Content = $"{Global.ENo} **|** That is an invalid response. Please try again.";
                });
            }
        }
    }
}