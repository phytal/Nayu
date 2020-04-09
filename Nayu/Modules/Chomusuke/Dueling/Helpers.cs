using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Chomusuke.Dueling.Attacks;
using Nayu.Modules.Chomusuke.Dueling.Effects;
using Nayu.Modules.Chomusuke.Dueling.Enums;

namespace Nayu.Modules.Chomusuke.Dueling
{
    public class Helpers : NayuModule
    {
        public static bool GetCritical()
        {
            int hit = Global.Rng.Next(1, 8);
            if (hit == 2) return true;
            return false;
        }

        public static AttackResult ExecuteAttack(ShardedCommandContext context, Core.Entities.Chomusuke activeChomusuke,
            string attack)
        {
            var result = new AttackResult();
            switch (attack)
            {
                case "Slash":
                    result = Slash.SlashAttack(context);
                    break;
                case "Block":
                    result = Block.BlockAttack(context);
                    break;
                case "Deflect":
                    result = Deflect.DeflectAttack(context);
                    break;
                case "Absorb":
                    result = Absorb.AbsorbAttack(context);
                    break;
                case "Bash":
                    result = Bash.BashAttack(context);
                    break;
                case "Fireball":
                    result = Fireball.FireballAttack(context);
                    break;
                case "Earthquake":
                    result = Earthquake.EarthquakeAttack(context);
                    break;
                case "Meditate":
                    result = Meditate.MeditateAttack(context, activeChomusuke);
                    break;
            }

            return result;
        }

        public static PotionResult ExecutePotion(ShardedCommandContext context, string potion, ulong user, ulong target)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            var choms = ActiveChomusuke.GetActiveChomusuke(user, config.OpponentId);
            var chom1 = choms.ChomusukeOne;
            var chom2 = choms.ChomusukeTwo;
            var result = new PotionResult();
            switch (potion)
            {
                case "Strength Potion":
                    result = Potions.StrengthPotion(context, chom1, chom2, target);
                    break;
                case "Debuff Potion":
                    result = Potions.DebuffPotion(context, chom1, chom2, target);
                    break;
                case "Speed Potion":
                    result = Potions.SpeedPotion(context, chom1, chom2, target);
                    break;
                case "Equalizer Potion":
                    result = Potions.EqualizerPotion(context, chom1, chom2, target);
                    break;
            }

            return result;
        }

        public static async Task ApplyEffects(ShardedCommandContext context, ulong user1, ulong user2)
        {
            var choms = ActiveChomusuke.GetActiveChomusuke(user1, user2);
            var chom1 = choms.ChomusukeOne;
            var chom2 = choms.ChomusukeTwo;
            foreach (var effect in chom1.Effects)
            {
                switch (effect)
                {
                    //TODO: Add effects and their results
                    case Effect.Binded:
                        break;
                    case Effect.Blocking:
                        break;
                    case Effect.Burned:
                        await Burning.Burned(context);
                        break;
                    case Effect.Confused:
                        break;
                    case Effect.Decay:
                        break;
                    case Effect.Deflecting:
                        break;
                    case Effect.Frozen:
                        break;
                    case Effect.Meditating:
                        break;
                    case Effect.Restricted:
                        break;
                    case Effect.Stunned:
                        break;
                }
            }
        }

        public static string GetHpLeft(Core.Entities.Chomusuke chom1, Core.Entities.Chomusuke chom2)
        {
            return
                $"**{chom1.Name}** has **{chom1.Health}** health left!\n**{chom2.Name}** has **{chom2.Health}** health left!";
        }

        public static int GetEffectIndex(List<Effect> effectList, Effect effect)
        {
            int index;
            for (index = 0; index < effectList.Count; index++)
            {
                if (effectList[index] == effect)
                    break;
            }

            return index;
        }

        /*
        public static int GetNextElement(int[] strArray, int index)
        {
            if ((index > strArray.Length - 1) || (index < 0))
                throw new Exception("Invalid index");

            if (index == strArray.Length - 1)
                index = 0;

            else
                index++;

            return strArray[index];
        }*/
    }

    public class AttackResult
    {
        public bool Success { get; set; }
        public string Response { get; set; }
        public int Damage { get; set; }
    }

    public class PotionResult
    {
        public bool Success { get; set; }
        public string Response { get; set; }
    }
}