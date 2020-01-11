using System;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Chomusuke.Dueling.Enums;

namespace Nayu.Modules.Chomusuke.Dueling.Attacks
{
    public class Meditate
    {
        public static AttackResult MeditateAttack(ShardedCommandContext context, Core.Entities.Chomusuke chom)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            string response;
            bool success = false;
            var dmg = 0;
            
            if (chom.Effects.Contains(Effect.Meditating))
                response = $"{chom.Name} has already meditated! Try Again!";
            else if (chom.Effects.Contains(Effect.Blocking))
                response = $"{chom.Name} cannot meditate while in blocking formation! Try Again!";
            else if (chom.Effects.Contains(Effect.Deflecting))
                response = $"{chom.Name} cannot meditate while in deflecting formation! Try Again!";
            else
            {
                response =
                    $":shield:  | **{config.OpponentName}**, {chom.Name} just meditated! \n\n**{config.OpponentName}**'s next attack will deal 30% more damage";
                chom.Effects.Add(Effect.Meditating);
                GlobalUserAccounts.SaveAccounts(config.Id);
                success = true;
            }
            
            return new AttackResult{Success = success, Response = response, Damage = dmg};
        }
    }
}