using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Helpers;
using Nayu.Preconditions;
using Type = Nayu.Modules.Chomusuke.Dueling.Enums.Type;

namespace Nayu.Modules.Chomusuke
{
    public class OpenCapsule : NayuModule
    {
        //TODO: update this
        [Subject(ChomusukeCategories.Chomusuke)]
        [Command("opencapsule"), Alias("opencapsule")]
        [Summary("Opens one of the Chomusuke capsules you have! Note: You must have the capsule you want to open.")]
        [Remarks("n!opencapsule <type (normal, shiny, mythical)> Ex: n!opencapsule normal")]
        [Cooldown(5)]
        public async Task OpenChomusukeCapsule(string arg)
        {
            var config = GlobalUserAccounts.GetUserAccount(Context.User);

            if (!config.Chomusuke3.Have) //if they own a Chomusuke or not
            {
                int value = 0;//will be used to determine type 
                if (arg == "normal")
                {
                    if (config.NormalCapsule > 0)
                    {
                        config.NormalCapsule -= 1;
                        value = Global.Rng.Next(1, 101);//1-100
                    }
                    else
                    {
                        await SendMessage(Context, null, $":octagonal_sign:  **|**  **{Context.User.Username}**, you don't have any Normal Chomusuke Capsules!");
                        return;
                    }
                }
                if (arg == "shiny")
                {
                    if (config.ShinyCapsule > 0)
                    {
                        config.ShinyCapsule -= 1;
                        value = Global.Rng.Next(1, 101);
                    }
                    else
                    {
                        await SendMessage(Context, null, $":octagonal_sign:  **|**  **{Context.User.Username}**, you don't have any Shiny Chomusuke Capsules!");
                        return;
                    }
                }
                if (arg == "mythical")
                {
                    if (config.MythicalCapsule > 0)
                    {
                        config.MythicalCapsule -= 1;
                        value = Global.Rng.Next(90, 101);
                    }
                    else
                    {
                        await SendMessage(Context, null, $":octagonal_sign:  **|**  **{Context.User.Username}**, you don't have any Mythical Chomusuke Capsules!");
                        return;
                    }
                }
                Tuple<bool, bool> trait = GetTraitsTypes.GetTraitsAsync();
                Type type = GetTraitsTypes.GetTypeAsync(value);
                int randomIndex = Global.Rng.Next(Zodiac.Length);
                string zodiac = Zodiac[randomIndex];
                uint cp = 20 * (config.LevelNumber / 10); //cp is boosted based off their user level *.1
                if (!config.Chomusuke1.Have)
                {
                    if (trait.Item1) config.Chomusuke1.Trait1 = Trait.Lucky;
                    if (trait.Item2) config.Chomusuke1.Shiny = true;
                    config.Chomusuke1.Type = type;
                    config.Chomusuke1.Have = true;
                    config.Chomusuke1.Name = "Chomusuke";
                    config.Chomusuke1.Zodiac = zodiac;
                    config.Chomusuke1.CP = cp;
                    config.Chomusuke1.HealthCapacity = 20;
                    config.Chomusuke1.ManaCapacity = 20;
                    config.Chomusuke1.ShieldCapacity = 5;
                    config.Chomusuke1.BoughtDay = DateTime.Now;
                }
                else if (!config.Chomusuke2.Have)
                {
                    if (trait.Item1) config.Chomusuke2.Trait1 = Trait.Lucky;
                    if (trait.Item2) config.Chomusuke2.Shiny = true;
                    config.Chomusuke2.Type = type;
                    config.Chomusuke2.Have = true;
                    config.Chomusuke2.Name = "Chomusuke";
                    config.Chomusuke2.Zodiac = zodiac;
                    config.Chomusuke2.CP = cp;
                    config.Chomusuke2.HealthCapacity = 20;
                    config.Chomusuke2.ManaCapacity = 20;
                    config.Chomusuke2.ShieldCapacity = 5;
                    config.Chomusuke2.BoughtDay = DateTime.Now;
                }
                else if (!config.Chomusuke3.Have)
                {
                    if (trait.Item1) config.Chomusuke3.Trait1 = Trait.Lucky;
                    if (trait.Item2) config.Chomusuke3.Shiny = true;
                    config.Chomusuke3.Type = type;
                    config.Chomusuke3.Have = true;
                    config.Chomusuke3.Name = "Chomusuke";
                    config.Chomusuke3.Zodiac = zodiac;
                    config.Chomusuke3.CP = cp;
                    config.Chomusuke3.HealthCapacity = 20;
                    config.Chomusuke3.ManaCapacity = 20;
                    config.Chomusuke3.ShieldCapacity = 5;
                    config.Chomusuke3.BoughtDay = DateTime.Now;
                }
                GlobalUserAccounts.SaveAccounts(config.Id);
                await SendMessage(Context, null, $":pill:  |  **{Context.User.Username}**, you got a **{type}-Type Chomusuke**! Would you like to name it? `yes/no` (If not, the name will remain Chomusuke)");

                var responsee = await NextMessageAsync();

                if (responsee.Content.Equals("yes", StringComparison.CurrentCultureIgnoreCase) && (responsee.Author.Equals(Context.User)))
                {
                    await SendMessage(Context, null, $"{Emote.Parse("<:chomusuke:601183653657182280>")}  |  **{Context.User.Username}**, what would you like to name it? (type the name you want to give it!)");
                    var responseee = await NextMessageAsync();
                    if (responseee.Author.Equals(Context.User))
                    {
                        if (config.Chomusuke3.Have) config.Chomusuke3.Name = responseee.Content;
                        else if (config.Chomusuke2.Have) config.Chomusuke2.Name = responseee.Content;
                        else if (config.Chomusuke1.Have) config.Chomusuke1.Name = responseee.Content;
                        GlobalUserAccounts.SaveAccounts(config.Id);
                        await SendMessage(Context, null, $"{Emote.Parse("<:chomusuke:601183653657182280>")}  |  **{Context.User.Username}**, successfully named your new Chomusuke to **{responseee}**! You can use the command `n!chomusuke help` for more Chomusuke commands!");
                    }
                }
                if (responsee.Content.Equals("no", StringComparison.CurrentCultureIgnoreCase) && (responsee.Author.Equals(Context.User)))
                {
                    await SendMessage(Context, null, $"{Emote.Parse("<:chomusuke:601183653657182280>")}  |  **{Context.User.Username}**, your Chomusuke is, well, Chomusuke! You can change the name of it by using the command `n!chomusuke name`");
                    return;
                }
            }
            if (config.Chomusuke3.Have)
            {
                await SendMessage(Context, null, $":warning:  |  **{Context.User.Username}**, you already own 3 {Emote.Parse("<:chomusuke:601183653657182280>")} Chomusukes! Abandon one first or just stop trying to amass Chomusukes! (Megumin won't be happy if you take all her Chomusukes)");
            }
        }

        string[] Zodiac = new string[]
        {
            "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Sagittarius", "Capricorn", "Aquarius", "Pisces"
        };
    }
}
