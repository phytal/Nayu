using System;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Chomusuke.Dueling.Enums;

namespace Nayu.Modules.Chomusuke.Dueling.Attacks
{
    public class Block
    {
        public static AttackResult BlockAttack(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var chom = ActiveChomusuke.GetOneActiveChomusuke(config.Id);

            string response;
            bool success = false;
            var dmg = 0;
            if (chom.Effects.Contains(Effect.Blocking))
            {
                response = "You are already in blocking formation! Try again!";
                return new AttackResult {Success = success, Response = response, Damage = dmg};
            }

            if (chom.Effects.Contains(Effect.Deflecting))
            {
                response = "You cannot block while already in deflecting formation! Try again!";
                return new AttackResult {Success = success, Response = response, Damage = dmg};
            }

            chom.Effects.Add(Effect.Blocking);
            GlobalUserAccounts.SaveAccounts(config.Id);
            response =
                $":shield:  **|** **{config.OpponentName}**, You are now in blocking formation!\n\n**{config.OpponentName}**'s shield will absorb 75% of the damage from the next attack";
            success = true;
            return new AttackResult {Success = success, Response = response, Damage = dmg};
        }
    }
}