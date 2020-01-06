using System;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Chomusuke.Dueling.Enums;

namespace Nayu.Modules.Chomusuke.Dueling.Attacks
{
    public class Earthquake
    {
        public static AttackResult EarthquakeAttack(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var player2 = context.Guild.GetUser(config.OpponentId);
            var choms = ActiveChomusuke.GetActiveChomusuke(config.Id, config.OpponentId);
            var chom1 = choms.Item1;
            var chom2 = choms.Item2;

            string response = string.Empty;
            bool success = false;
            int dmg = Global.Rng.Next(15, 25);
            if (chom1.Effects.Contains(Effect.Blocking))
                chom1.Effects.Remove(Effect.Blocking);
            if (chom2.Effects.Contains(Effect.Blocking))
                chom2.Effects.Remove(Effect.Blocking);
            if (chom1.Effects.Contains(Effect.Deflecting))
                chom1.Effects.Remove(Effect.Deflecting);
            if (chom2.Effects.Contains(Effect.Deflecting))
                chom2.Effects.Remove(Effect.Deflecting);
            chom1.Health -= (uint)dmg/3;
            chom2.Health -= (uint)dmg;
            GlobalUserAccounts.SaveAccounts(config.Id);
            response = $"<:shatter:532002647692148748>  | **{chom1.Name}**, created an Earthquake! {dmg} damage was dealt to {chom2.Name} with {chom1.Name} taking {dmg/3} and Blocks/Deflects are canceled";
            success = true;

            return new AttackResult{Success = success, Response = response, Damage = dmg};
        }
    }
}