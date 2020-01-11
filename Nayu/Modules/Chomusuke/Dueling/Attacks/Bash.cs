using System;
using System.Collections.Generic;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Chomusuke.Dueling.Enums;

namespace Nayu.Modules.Chomusuke.Dueling.Attacks
{
    public class Bash
    {
        private static readonly AttackStructure Attack = new AttackStructure
        {
            Damage = Global.Rng.Next(5, 10),
            Effects = new List<Effect>{Effect.Stunned},
            Mana = 5,
            Name = "Bash"
        };
        
        public static  AttackResult BashAttack(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var choms = ActiveChomusuke.GetActiveChomusuke(config.Id, config.OpponentId);
            var chom1 = choms.Item1;
            var chom2 = choms.Item2;
            
            string response;
            bool success = false;
            var dmg = (int)Math.Round(Attack.Damage * chom1.CP * .05);
            var stunProb = Global.Rng.Next(1, 3);
            chom1.Mana -= Attack.Mana;

            var result = CheckModifiers.GetDMGModifiers(context.User, player2, dmg);
            dmg = result.Item1;
            int modifier = result.Item2;

            var block = CheckModifiers.CheckForBlock(context.User, player2, dmg);
            if (block.Item1)
            {
                chom2.Health -= (uint)block.Item3;
                GlobalUserAccounts.SaveAccounts(config.Id);
                response = $"**{context.User.Username}**, You bashed **{chom2.Name}** and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{block.Item2}** damage was blocked!\n\n{Helpers.GetHpLeft(chom1, chom2)}";
                dmg = block.Item3;
                return new AttackResult{Success = success, Response = response, Damage = dmg};
            }
            var deflect = CheckModifiers.CheckForDeflect(context.User, player2, dmg);
            if (deflect.Item1)
            {
                chom2.Health -= (uint)deflect.Item2; 
                chom1.Health -= (uint)deflect.Item2;
                GlobalUserAccounts.SaveAccounts(config.Id);
                response = $"**{context.User.Username}**, You bashed **{chom2.Name}** and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{deflect.Item2}** damage was deflected back!\n\n{Helpers.GetHpLeft(chom1, chom2)}";
                dmg = deflect.Item2;
                return new AttackResult{Success = success, Response = response, Damage = dmg};
            }

            if (stunProb == 1)
            {
                response = $"**{context.User.Username}**, You bashed **{chom2.Name}** and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers)!**{chom2.Name}** is now stunned!\n\n{Helpers.GetHpLeft(chom1, chom2)}";
                foreach (Effect effect in Attack.Effects)
                {
                    chom2.Effects.Add(effect);
                }
            }
            
            chom2.Health -= (uint)dmg;
            GlobalUserAccounts.SaveAccounts(context.User.Id, player2.Id);

            response = $"**{context.User.Username}**, You bashed **{chom2.Name}** and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers)!\n\n{Helpers.GetHpLeft(chom1, chom2)}";
            success = true;

            return new AttackResult{Success = success, Response = response, Damage = dmg};
        }
    }
}