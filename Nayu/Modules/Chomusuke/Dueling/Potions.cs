using System.Linq;
using Discord.Commands;
using Nayu.Core.Entities;
using Nayu.Core.Features.GlobalAccounts;

namespace Nayu.Modules.Chomusuke.Dueling.Attacks
{
    public class Potions
    {
        public static PotionResult StrengthPotion(ShardedCommandContext context, Core.Entities.Chomusuke chom1,
            Core.Entities.Chomusuke chom2, ulong target)
        {
            var potionName = "Strength Potion";
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var check = CheckForPotion(potionName, config);
            if (!check.Success)
                return new PotionResult {Success = false, Response = check.Response};
            string response = string.Empty;
            if (chom1.PotionEffects["Strength"] <= 25 ||
                !chom1.PotionEffects.Keys.Contains("Strength") || chom2.PotionEffects["Strength"] <= 25 ||
                !chom2.PotionEffects.Keys.Contains("Strength"))
            {
                config.Items[potionName] -= 1;
                if (target == context.User.Id)
                {
                    chom1.PotionEffects.Add("Strength", 0);
                    chom1.PotionEffects["Strength"] += 5;
                    response = $"**{context.User.Mention}** used a **{potionName}** on {chom1.Name}!";
                }

                if (target == player2.Id)
                {
                    chom1.PotionEffects.Add("Strength", 0);
                    chom2.PotionEffects["Strength"] += 5;
                    response = $"**{context.User.Mention}** used a **{potionName}** on {chom2.Name}!";
                }

                GlobalUserAccounts.SaveAccounts(config.Id);

                return new PotionResult {Success = true, Response = response};
            }

            response =
                $"**{context.User.Mention}**, you cant use a **{potionName}** because both players are already at the max strength level (25%)!";
            return new PotionResult {Success = false, Response = response};
        }

        public static PotionResult DebuffPotion(ShardedCommandContext context, Core.Entities.Chomusuke chom1,
            Core.Entities.Chomusuke chom2, ulong target)
        {
            var potionName = "Debuff Potion";
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var check = CheckForPotion(potionName, config);
            if (!check.Success)
                return new PotionResult {Success = false, Response = check.Response};
            string response = string.Empty;
            if (chom1.PotionEffects["Debuff"] <= 25 ||
                !chom1.PotionEffects.Keys.Contains("Debuff") || chom2.PotionEffects["Debuff"] <= 25 ||
                !chom2.PotionEffects.Keys.Contains("Debuff"))
            {
                config.Items[potionName] -= 1;
                if (target == context.User.Id)
                {
                    chom1.PotionEffects.Add("Debuff", 0);
                    chom1.PotionEffects["Debuff"] += 5;
                    response = $"**{context.User.Mention}** used a **{potionName}** on {chom1.Name}!";
                    GlobalUserAccounts.SaveAccounts(config.Id);
                    return new PotionResult {Success = true, Response = response};
                }

                if (target == player2.Id)
                {
                    chom1.PotionEffects.Add("Debuff", 0);
                    chom2.PotionEffects["Debuff"] += 5;
                    response = $"**{context.User.Mention}** used a **{potionName}** on {chom2.Name}!";
                    GlobalUserAccounts.SaveAccounts(player2.Id);
                    return new PotionResult {Success = true, Response = response};
                }
            }

            response =
                $"**{context.User.Mention}**, you cant use a **{potionName}** because both players are already at the max debuff level (25%)!";
            return new PotionResult {Success = false, Response = response};
        }

        public static PotionResult SpeedPotion(ShardedCommandContext context, Core.Entities.Chomusuke chom1,
            Core.Entities.Chomusuke chom2, ulong target)
        {
            var potionName = "Speed Potion";
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var check = CheckForPotion(potionName, config);
            if (!check.Success)
                return new PotionResult {Success = false, Response = check.Response};
            string response = string.Empty;
            if (!chom1.PotionEffects.Keys.Contains("Speed") || !chom2.PotionEffects.Keys.Contains("Speed"))
            {
                config.Items[potionName] -= 1;
                if (target == context.User.Id)
                {
                    chom1.PotionEffects.Add("Speed", 0); //going to use its existence to determine true/false
                    response = $"**{context.User.Mention}** used a **{potionName}** on {chom1.Name}!";
                    GlobalUserAccounts.SaveAccounts(config.Id);
                    return new PotionResult {Success = true, Response = response};
                }

                if (target == player2.Id)
                {
                    chom2.PotionEffects.Add("Speed", 0);
                    response = $"**{context.User.Mention}** used a **{potionName}** on {chom2.Name}!";
                    GlobalUserAccounts.SaveAccounts(player2.Id);
                    return new PotionResult {Success = true, Response = response};
                }
            }

            response =
                $"**{context.User.Mention}**, you cant use a **{potionName}** because both players already have the effect!";
            return new PotionResult {Success = false, Response = response};
        }

        public static PotionResult EqualizerPotion(ShardedCommandContext context, Core.Entities.Chomusuke chom1,
            Core.Entities.Chomusuke chom2, ulong target)
        {
            var potionName = "Equalizer Potion";
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var check = CheckForPotion(potionName, config);
            if (!check.Success)
                return new PotionResult {Success = false, Response = check.Response};
            string response = string.Empty;
            config.Items[potionName] -= 1;
            if (target == context.User.Id)
            {
                if (chom1.PotionEffects == null)
                {
                    response =
                        $"**{context.User.Mention}**, you cant use an **{potionName}** on yourself because you don't have any active potion effects!";
                    return new PotionResult {Success = false, Response = response};
                }

                chom1.PotionEffects.Clear();
                response = $"**{context.User.Mention}** used a **{potionName}** on {chom1.Name}!";
                GlobalUserAccounts.SaveAccounts(config.Id);
                return new PotionResult {Success = true, Response = response};
            }

            //opponent
            if (chom2.PotionEffects == null)
            {
                response =
                    $"**{context.User.Mention}**, you cant use a **{potionName}** on your opponent because they don't have any active potion effects!";
                return new PotionResult {Success = false, Response = response};
            }

            chom2.PotionEffects.Clear();
            response = $"**{context.User.Mention}** used a **{potionName}** on {chom2.Name}!";
            GlobalUserAccounts.SaveAccounts(config.OpponentId);
            return new PotionResult {Success = true, Response = response};
        }

        /// <summary>
        /// Checks if a user has a potion
        /// </summary>
        public static PotionResult CheckForPotion(string potionName, GlobalUserAccount user)
        {
            var result = new PotionResult();
            if (user.Items[potionName] < 1)
            {
                result.Response = $"You don't have any {potionName}s!";
                return result;
            }

            result.Success = true;
            return result;
        }
    }
}