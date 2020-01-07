using System;
using System.Threading.Tasks;
using Discord.Commands;
using Nayu.Core.Features.GlobalAccounts;
using Nayu.Modules.Chomusuke.Dueling.Enums;

namespace Nayu.Modules.Chomusuke.Dueling.Effects
{
    public class Burning : NayuModule
    {
        public static async Task Burned(ShardedCommandContext context)
        {
            var config = GlobalUserAccounts.GetUserAccount(context.User);
            var chom = ActiveChomusuke.GetOneActiveChomusuke(config.Id);

            var dmg = (uint)Global.Rng.Next(2, 5);

            if (!chom.Effects.Contains(Effect.Burned)) return;
            var rng = Global.Rng.Next(1, 5);
            if (rng == 4)
            {
                chom.Effects.Remove(Effect.Burned);
                GlobalUserAccounts.SaveAccounts(config.Id);
                await SendMessage(context, null, $"{chom.Name} stopped burning!");
                return;
            }
            chom.Health -= dmg;
            GlobalUserAccounts.SaveAccounts(config.Id);
            await SendMessage(context, null, $"{chom.Name} took {dmg} from being burned!");
        }
    }
}