using System;
using System.Linq;
using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Chomusuke.Dueling.Enums;

namespace Nayu.Modules.Chomusuke.Dueling
{
    public class CheckModifiers
    {
        public static Tuple<int, int> GetDMGModifiers(SocketUser attacker, SocketUser defender, int dmg)
        {
            var config = GlobalUserAccounts.GetUserAccount(attacker);
            var choms = ActiveChomusuke.GetActiveChomusuke(attacker.Id, defender.Id);
            var chom1 = choms.Item1;
            var chom2 = choms.Item2;
            int modifier = 0;

            if (chom1.PotionEffects.Keys.Contains("Strength")) modifier += chom1.PotionEffects["Strength"];
            if (chom2.PotionEffects.Keys.Contains("Debuff")) modifier += chom2.PotionEffects["Debuff"];
            if (chom1.Effects.Contains(Effect.Meditating))
            {
                modifier += 30;
                chom1.Effects.Remove(Effect.Meditating);
                GlobalUserAccounts.SaveAccounts(config.Id);
            }

            if (Helpers.GetCritical()) modifier += 20;
            if (config.Blessing == "Blessing Of Strength") modifier += 10;
            int newdmg = (modifier / 100 + 1) * dmg;
            return new Tuple<int, int>(newdmg, modifier);
        }

        /// <summary>
        /// Checks if the opponent is blocking.
        /// Returns: Success, Blocked Damage, Taken Damage
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <param name="dmg"></param>
        /// <returns> Success, Blocked Damage, Taken Damage</returns>
        public static Tuple<bool, int, int> CheckForBlock(SocketUser attacker, SocketUser defender, int dmg)
        {
            var config = GlobalUserAccounts.GetUserAccount(defender);
            var choms = ActiveChomusuke.GetActiveChomusuke(attacker.Id, defender.Id);
            var chom1 = choms.Item1;
            var chom2 = choms.Item2;
            
            int finaldmg2 = 0, finaldmg1 = 0;
            if (chom1.PotionEffects.ContainsKey("Speed"))
            {
                chom2.Effects.Remove(Effect.Blocking);
                ActiveChomusuke.ConvertActiveVariable(attacker.Id, config.OpponentId, chom1, chom2);
                return new Tuple<bool, int, int>(true, 0, 0);
            }

            if (chom2.Effects.Contains(Effect.Blocking))
            {
                finaldmg1 = dmg * (3 / 4); //blocked dmg
                finaldmg2 = dmg * (1 / 4); //taken dmg
                chom2.Effects.Remove(Effect.Blocking);
                
                return new Tuple<bool, int, int>(true, finaldmg1, finaldmg2);
            }
            GlobalUserAccounts.SaveAccounts(config.Id);
            return new Tuple<bool, int, int>(false, 0, 0); // use for values
        }

        public static Tuple<bool, int> CheckForDeflect(SocketUser attacker, SocketUser defender, int dmg)
        {
            var config = GlobalUserAccounts.GetUserAccount(defender);
            var choms = ActiveChomusuke.GetActiveChomusuke(attacker.Id, defender.Id);
            var chom1 = choms.Item1;
            var chom2 = choms.Item2;
            int finaldmg = 0;
            if (chom1.PotionEffects.ContainsKey("Speed"))
            {
                chom2.Effects.Remove(Effect.Deflecting);
                ActiveChomusuke.ConvertActiveVariable(attacker.Id, config.OpponentId, chom1, chom2);
                return new Tuple<bool, int>(true, 0);
            }
            if (chom2.Effects.Contains(Effect.Deflecting))
            {
                finaldmg = dmg / 2; //damage taken and deflected
                chom2.Effects.Remove(Effect.Deflecting);
                return new Tuple<bool, int>(true, finaldmg);
            } 
            GlobalUserAccounts.SaveAccounts(config.Id);
            return new Tuple<bool, int>(false, 0);
        }
    }
}
