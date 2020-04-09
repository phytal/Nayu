using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Chomusuke.Dueling.Enums;
using Type = System.Type;

namespace Nayu.Modules.Chomusuke.Dueling.Attacks
{
    public class Fireball
    {
        private static readonly AttackStructure Attack = new AttackStructure
        {
            Name = "Fireball",
            Damage = 11,
            Mana = 4,
            Accuracy = 7,
            Types = new List<Enums.Type> {Enums.Type.Fire},
            Effects = new List<Effect> {Effect.Burned}
        };

        public static AttackResult FireballAttack(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var configg = GlobalUserAccounts.GetUserAccount(player2);
            var choms = ActiveChomusuke.GetActiveChomusuke(config.Id, config.OpponentId);
            var chom1 = choms.Item1;
            var chom2 = choms.Item2;

            var accuracy = Global.Rng.Next(1, Attack.Accuracy + 1);
            string response;
            bool success;
            var dmg = (int) Math.Round(Attack.Damage * chom1.CP * .05);
            //use if something negates this
            /*if (configg.armour == "reinforced")
            {
                GlobalUserAccounts.SaveAccounts();
                response = $":fire: **|** **{configg.OpponentName}** used **Fireball** but {config.OpponentName}** has reinforced armour. Therefore it had no effect.";
                success = true;
                return new AttackResult{Success = success, Response = response, Damage = 0};
            }*/
            if (accuracy > 1)
            {
                var result = CheckModifiers.GetDMGModifiers(context.User, player2, dmg);
                dmg = result.Item1;
                var modifier = result.Item2;
                chom2.Health -= (uint) dmg;
                chom2.Effects.Add(Effect.Burned);

                GlobalUserAccounts.SaveAccounts(config.Id);
                response =
                    $":fire:  | **{configg.OpponentName}** used **Fireball** and did {dmg} damage (buffed by {modifier}% due to active damage modifiers)!\n\n**{config.OpponentName}** is now burning";
                success = true;
                return new AttackResult {Success = success, Response = response, Damage = dmg};
            }
            else
            {
                dmg = 0;
                var result = CheckModifiers.GetDMGModifiers(context.User, player2, dmg);
                response = $":dash:  **|** **{context.User.Username}**, your attack missed!";
                success = true;
                return new AttackResult {Success = success, Response = response, Damage = dmg};
            }
        }
    }
}