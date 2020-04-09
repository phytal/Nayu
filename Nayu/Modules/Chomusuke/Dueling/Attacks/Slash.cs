using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Type = Nayu.Modules.Chomusuke.Dueling.Enums.Type;

namespace Nayu.Modules.Chomusuke.Dueling.Attacks
{
    public class Slash
    {
        private static readonly AttackStructure Attack = new AttackStructure
        {
            Name = "Slash",
            Damage = 13,
            Mana = 2,
            Types = new List<Type> {Type.Normal},
            Accuracy = 9
        };

        public static AttackResult SlashAttack(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var choms = ActiveChomusuke.GetActiveChomusuke(config.Id, config.OpponentId);
            var chom1 = choms.Item1;
            var chom2 = choms.Item2;

            string response;
            var dmg = (int) Math.Round(Attack.Damage * chom1.CP * .05);
            var accuracy = Global.Rng.Next(1, Attack.Accuracy + 1);

            if (accuracy > 1)
            {
                var result = CheckModifiers.GetDMGModifiers(context.User, player2, dmg);
                dmg = result.Item1;
                var modifier = result.Item2;

                var block = CheckModifiers.CheckForBlock(context.User, player2, dmg);
                var deflect = CheckModifiers.CheckForDeflect(context.User, player2, dmg);
                if (block.Item1)
                {
                    chom2.Health -= (uint) block.Item3;
                    GlobalUserAccounts.SaveAccounts(config.Id);
                    response =
                        $":dagger:  | **{context.User.Username}**, You hit and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{block.Item2}** damage was blocked!\n\n{Helpers.GetHpLeft(chom1, chom2)}";
                    dmg = block.Item3;
                }
                else if (deflect.Item1)
                {
                    chom2.Health -= (uint) deflect.Item2;
                    chom1.Health -= (uint) deflect.Item2;
                    GlobalUserAccounts.SaveAccounts(config.Id);
                    response =
                        $":dagger:  | **{context.User.Username}**, You hit and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{deflect.Item2}** damage was deflected back!\n\n{Helpers.GetHpLeft(chom1, chom2)}";
                    dmg = deflect.Item2;
                }
                else
                {
                    chom2.Health -= (uint) dmg;
                    GlobalUserAccounts.SaveAccounts(config.Id);
                    response =
                        $":dagger:  | **{context.User.Username}**, You hit and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers)!\n\n**{chom1.Name}** has **{chom1.Health}** health left!\n{Helpers.GetHpLeft(chom1, chom2)}";
                }
            }
            else
            {
                response = $":dash:  **|** **{context.User.Username}**, your attack missed!";
            }

            return new AttackResult {Success = true, Response = response, Damage = dmg};
        }
    }
}