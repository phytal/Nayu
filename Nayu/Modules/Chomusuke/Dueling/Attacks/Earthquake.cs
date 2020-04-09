using System;
using System.Collections.Generic;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Chomusuke.Dueling.Enums;

namespace Nayu.Modules.Chomusuke.Dueling.Attacks
{
    public class Earthquake
    {
        private static readonly AttackStructure Attack = new AttackStructure
        {
            Name = "Earthquake",
            Damage = 18,
            Mana = 6,
            Effects = new List<Effect> {Effect.Stunned},
            Accuracy = 16
        };

        public static AttackResult EarthquakeAttack(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var choms = ActiveChomusuke.GetActiveChomusuke(config.Id, config.OpponentId);
            var chom1 = choms.ChomusukeOne;
            var chom2 = choms.ChomusukeTwo;

            string response;
            bool success;
            var dmg = (int) Math.Round(Attack.Damage * chom1.CP * .05);
            if (chom1.Effects.Contains(Effect.Blocking))
                chom1.Effects.Remove(Effect.Blocking);
            if (chom2.Effects.Contains(Effect.Blocking))
                chom2.Effects.Remove(Effect.Blocking);
            if (chom1.Effects.Contains(Effect.Deflecting))
                chom1.Effects.Remove(Effect.Deflecting);
            if (chom2.Effects.Contains(Effect.Deflecting))
                chom2.Effects.Remove(Effect.Deflecting);
            chom1.Health -= (uint) dmg / 3;
            chom2.Health -= (uint) dmg;
            GlobalUserAccounts.SaveAccounts(config.Id);
            response =
                $"<:shatter:532002647692148748>  | **{chom1.Name}**, created an Earthquake! {dmg} damage was dealt to {chom2.Name} with {chom1.Name} taking {dmg / 3} and Blocks/Deflects are canceled";
            success = true;

            return new AttackResult {Success = success, Response = response, Damage = dmg};
        }
    }
}