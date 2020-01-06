using System.Threading.Tasks;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;

namespace Nayu.Modules.Chomusuke.Dueling.Attacks
{
    public class Slash
    {
        public static AttackResult SlashAttack(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var choms = ActiveChomusuke.GetActiveChomusuke(config.Id, config.OpponentId);
            var chom1 = choms.Item1;
            var chom2 = choms.Item2;

            string response = string.Empty;
            int dmg = 0;
            int hitormiss = Global.Rng.Next(1, 9);

            if (hitormiss > 1)
            {
                dmg = Global.Rng.Next(7, 15);
                var result = CheckModifiers.GetDMGModifiers(context.User, player2, dmg);
                dmg = result.Item1;
                int modifier = result.Item2;

                var block = CheckModifiers.CheckForBlock(context.User, player2, dmg);
                var deflect = CheckModifiers.CheckForDeflect(context.User, player2, dmg);
                if (block.Item1)
                {
                    chom2.Health -= (uint)block.Item3;
                    GlobalUserAccounts.SaveAccounts(config.Id);
                    response = $":dagger:  | **{context.User.Username}**, You hit and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{block.Item2}** damage was blocked!\n\n{Helpers.GetHpLeft(chom1, chom2)}";
                    dmg = block.Item3;
                }
                else if (deflect.Item1)
                {
                    chom2.Health -= (uint)deflect.Item2; 
                    chom1.Health -= (uint)deflect.Item2;
                    GlobalUserAccounts.SaveAccounts(config.Id);
                    response = $":dagger:  | **{context.User.Username}**, You hit and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers), but **{deflect.Item2}** damage was deflected back!\n\n{Helpers.GetHpLeft(chom1, chom2)}";
                    dmg = deflect.Item2;
                }
                else
                {
                    chom2.Health -= (uint)dmg;
                    GlobalUserAccounts.SaveAccounts(config.Id);
                    response = $":dagger:  | **{context.User.Username}**, You hit and did **{dmg}** damage (buffed by {modifier}% due to active damage modifiers)!\n\n**{chom1.Name}** has **{chom1.Health}** health left!\n{Helpers.GetHpLeft(chom1, chom2)}";
                }
            }
            else
            {
                response = $":dash:  | **{context.User.Username}**, your attack missed!";
            }

            return new AttackResult{Success = true, Response = response, Damage = dmg};
        }
    }
}