using System;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;

namespace Nayu.Modules.Chomusuke.Dueling.Attacks
{
    public class Absorb
    {
        public static AttackResult AbsorbAttack(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var choms = ActiveChomusuke.GetActiveChomusuke(config.Id, config.OpponentId);
            var chom1 = choms.Item1;
            var chom2 = choms.Item2;

            string response = string.Empty;
            bool success = false;
            int hit = Global.Rng.Next(1, 3);

            if (hit == 1)
            {
                //use if something negates this
                /*if (configg.armour == "reinforced")
                {
                    GlobalUserAccounts.SaveAccounts();
                    response =
                        $":fire:  | **{configg.OpponentName}** used **Absorb** but {config.OpponentName}** has reinforced armour. Therefore it had no effect.";
                    success = true;
                    return new AttackResult{Success = success, Response = response, Damage = 0};
                }*/

                int dmg = Global.Rng.Next(7, 15);

                var result = CheckModifiers.GetDMGModifiers(context.User, player2, dmg);
                dmg = result.Item1;
                int modifier = result.Item2;

                var block = CheckModifiers.CheckForBlock(context.User, player2, dmg);
                if (block.Item1 == true)
                {
                    chom2.Health -= (uint)block.Item3;
                    GlobalUserAccounts.SaveAccounts(config.Id);
                    response =
                        $":comet:  | **{context.User.Username}**,You absorbed **{dmg}** health and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{block.Item2}** damage was blocked!\n\n{Helpers.GetHpLeft(chom1, chom2)}";
                    dmg = block.Item3;
                    success = true;
                    return new AttackResult{Success = success, Response = response, Damage = dmg};
                }

                var deflect = CheckModifiers.CheckForDeflect(context.User, player2, dmg);
                if (deflect.Item1 == true)
                {
                    chom2.Health -= (uint)deflect.Item2; 
                    chom1.Health -= (uint)deflect.Item2;
                    GlobalUserAccounts.SaveAccounts(config.Id);
                    response =
                        $":comet:  | **{context.User.Username}**, You absorbed **{dmg}** health and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{deflect.Item2}** damage was deflected back!\n\n{Helpers.GetHpLeft(chom1, chom2)}";
                    dmg = deflect.Item2;
                    success = true;
                    return new AttackResult{Success = success, Response = response, Damage = dmg};
                }

                chom2.Health -= (uint)dmg;
                GlobalUserAccounts.SaveAccounts(config.Id);

                response =
                    $":comet:  | **{context.User.Username}**, You absorbed **{dmg}** health and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers)!\n\n{Helpers.GetHpLeft(chom1, chom2)}";
                success = true;
                return new AttackResult{Success = success, Response = response, Damage = dmg};
            }
            else
            {
                int dmg = 0;
                var result = CheckModifiers.GetDMGModifiers(context.User, player2, dmg); //resets 1 turn buffs
                response = $":dash:  | **{context.User.Username}**, your attack missed!";
                success = true;
                return new AttackResult{Success = success, Response = response, Damage = dmg};
            }
        }
    }
}