using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Nayu.Features.GlobalAccounts;

namespace Nayu.Modules.Fun.Dueling
{
    public class CheckModifiers
    {
        public static Tuple<int, int> GetDMGModifiers(SocketUser attacker, SocketUser defender, int dmg)
        {
            var config = GlobalUserAccounts.GetUserAccount(attacker);
            var configg = GlobalUserAccounts.GetUserAccount(defender);
            int modifier = 0;
            if (configg.armour == "bronze") modifier -= 5;
            if (configg.armour == "steel") modifier -= 10;
            if (configg.armour == "gold") modifier -= 15;
            if (configg.armour == "platinum") modifier -= 20;
            if (config.weapon == "bronze") modifier += 5;
            if (config.weapon == "steel") modifier += 10;
            if (config.weapon == "gold") modifier += 15;
            if (config.HasStrengthPots > 0) modifier += config.HasStrengthPots;
            if (configg.HasDebuffPots > 0) modifier += config.HasDebuffPots;
            if (config.HasPoisonedWeapon == true) modifier += 15;
            if (config.IsMeditate == true)
            {
                modifier += 30;
                config.IsMeditate = false;
                GlobalUserAccounts.SaveAccounts();
            }
            if (config.ActiveBlessing == "Blessing Of Strength") modifier += 10;
            int newdmg = (modifier / 100) * dmg + dmg;
            return new Tuple<int, int>(newdmg, modifier);
        }

        public static Tuple<bool, string> CheckForAcidShield(SocketUser defender)
        {
            var config = GlobalUserAccounts.GetUserAccount(defender);
            if (config.HasBasicTreatment == true) return new Tuple<bool, string>(true, "basic");
            if (config.HasDivineShield == true) return new Tuple<bool, string>(true, "divine");
            else return new Tuple<bool, string>(false, "none");
        }

        public static bool CheckForReinforcedArmour(SocketUser defender)
        {
            var config = GlobalUserAccounts.GetUserAccount(defender);
            if (config.armour == "reinforced") return true;
            else return false;
        }
        public static Tuple<bool, int, int> CheckForBlock(SocketUser attacker, SocketUser defender, int dmg)
        {
            var config = GlobalUserAccounts.GetUserAccount(defender);
            var configg = GlobalUserAccounts.GetUserAccount(attacker);
            int finaldmg1 = 0;
            int finaldmg2 = 0;
            if (config.HasSpeedPots)
            {
                config.Blocking = false;
                GlobalUserAccounts.SaveAccounts();
                return new Tuple<bool, int, int>(true, 0, 0);
            }
            if (config.Blocking == true)
            {
                finaldmg1 = dmg * (3 / 4); //blocked dmg
                finaldmg2 = dmg * (1 / 4); //taken dmg
                config.Blocking = false;
                GlobalUserAccounts.SaveAccounts();
                return new Tuple<bool, int, int>(true, finaldmg1, finaldmg2);
            }
            else return new Tuple<bool, int, int>(false, 0, 0); // use for values
        }

        public static Tuple<bool, int> CheckForDeflect(SocketUser attacker, SocketUser defender, int dmg)
        {
            var config = GlobalUserAccounts.GetUserAccount(defender);
            var configg = GlobalUserAccounts.GetUserAccount(attacker);
            int finaldmg = 0;
            if (config.Deflecting == true)
            {
                finaldmg = dmg / 2; //damage taken and deflected
                return new Tuple<bool, int>(true, finaldmg);
            }
            else return new Tuple<bool, int>(false, 0);
        }
    }
}
