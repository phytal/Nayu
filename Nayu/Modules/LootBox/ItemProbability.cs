using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;

namespace Nayu.Modules.LootBox
{
    public class ItemProbability
    {
        public static string DuelsItemProbabiliy(SocketUser user, char tier)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            string[] legendary = {"ChainsOfTartarus", "FreyasBlessing", "DiceOfGod", "MeadOfPoetry", "HairOfAGoddess"};
            string[] epic = {"BlessingOfProtection", "Blessing of Strength", "BlessingOfSwiftness", "BlessingOfWar"};
            string[] rare =
            {
                "ConstantineMedallion", "BookOfExodus", "VolcanicRune", "FlaskOfIchor", "FlaskOfElixir", "FlaskOfMana"
            };
            string[] uncommon = {"FireThread", "SkyPowder", "TearsOfHera", "HornOfVeles", "BranchOfYggdrasil"};
            string[] common = {"ShardsOfImmortality", "ReviveCrystal", "SilverWood", "AGlowingRock", "PhoenixFeathers"};
            /*
             * rates:
             * legendary: 4%
             * epic: 10%
             * rare = 18%
             * uncommon = 30%
             * common = 38%
             */
            string item = null;
            switch (tier)
            {
                case 'l':
                    item = legendary[RandomIndexProvider(legendary)];
                    break;
                case 'e':
                    var eRate = (byte) Global.Rng.Next(1, 15);
                    if (eRate <= 4) item = legendary[RandomIndexProvider(legendary)];
                    else item = epic[RandomIndexProvider(epic)];
                    break;
                case 'r':
                    var rRate = (byte) Global.Rng.Next(1, 33);
                    if (rRate <= 4) item = legendary[RandomIndexProvider(legendary)];
                    else if (rRate <= 14) item = epic[RandomIndexProvider(epic)];
                    else item = rare[RandomIndexProvider(rare)];
                    break;
                case 'u':
                    var uRate = (byte) Global.Rng.Next(1, 63);
                    if (uRate <= 4) item = legendary[RandomIndexProvider(legendary)];
                    else if (uRate <= 14) item = epic[RandomIndexProvider(epic)];
                    else if (uRate <= 32) item = rare[RandomIndexProvider(rare)];
                    else item = uncommon[RandomIndexProvider(uncommon)];
                    break;
                case 'c':
                    var cRate = (byte) Global.Rng.Next(1, 101);
                    if (cRate <= 4) item = legendary[RandomIndexProvider(legendary)];
                    else if (cRate <= 14) item = epic[RandomIndexProvider(epic)];
                    else if (cRate <= 32) item = rare[RandomIndexProvider(rare)];
                    else if (cRate <= 62) item = uncommon[RandomIndexProvider(uncommon)];
                    else item = common[RandomIndexProvider(common)];
                    break;
            }

            if (!config.Items.ContainsKey(item)) config.Items.Add(item, 1);
            else config.Items[item] += 1;
            GlobalUserAccounts.SaveAccounts(config.Id);
            return item;
        }

        public static byte RandomIndexProvider(string[] list)
        {
            byte randomIndex = (byte) Global.Rng.Next(1, list.Length + 1);
            return randomIndex;
        }
    }
}