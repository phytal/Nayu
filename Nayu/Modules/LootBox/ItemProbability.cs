using Discord.WebSocket;
using Nayu.Core.Features.GlobalAccounts;

namespace Nayu.Modules.LootBox
{
    public class ItemProbability
    {
        public static string DuelsItemProbabiliy(SocketUser user, char tier)
        {
            var config = GlobalUserAccounts.GetUserAccount(user);
            string[] legendary = {"ChainsOfTartarus", "FreyasBlessing", "DiceOfGod", "MeadOfPoetry",};
            string[] epic = {"BlessingOfProtection", "Blessing of Strength",  "BlessingOfSwiftness", "BlessingOfWar"};
            string[] rare = {"ConstantineMedallion", "BookOfExodus", "VolcanicRune", "FlaskOfIchor", "FlaskOfElixir", "FlaskOfMana"};
            string[] uncommon = {"FireThread", "SkyPowder", "TearsOfHera", "HornOfVeles", "BranchOfYggdrasil"};
            string[] common = {"ShardsOfImmortality", "ReviveCrystal"};
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
                    item =  legendary[randomIndexProvider(legendary)];
                    break;
                case 'e':
                    byte eRate = (byte)Global.Rng.Next(1, 15);
                    if (eRate <= 4) item =  legendary[randomIndexProvider(legendary)];
                    else item =  epic[randomIndexProvider(epic)];
                    break;
                case 'r':
                    byte rRate = (byte)Global.Rng.Next(1, 33);
                    if (rRate <= 4) item =  legendary[randomIndexProvider(legendary)];
                    else if (rRate <= 14) item =  epic[randomIndexProvider(epic)];
                    else item =  rare[randomIndexProvider(rare)];
                    break;
                case 'u':
                    byte uRate = (byte)Global.Rng.Next(1, 63);
                    if (uRate <= 4) item =  legendary[randomIndexProvider(legendary)];
                    else if (uRate <= 14) item =  epic[randomIndexProvider(epic)];
                    else if (uRate <= 32) item =  rare[randomIndexProvider(rare)];
                    else item =  uncommon[randomIndexProvider(uncommon)];
                    break;
                case 'c':
                    byte cRate = (byte)Global.Rng.Next(1, 101);
                    if (cRate <= 4) item =  legendary[randomIndexProvider(legendary)];
                    else if (cRate <= 14) item =  epic[randomIndexProvider(epic)];
                    else if (cRate <= 32) item =  rare[randomIndexProvider(rare)];
                    else if (cRate <= 62) item =  uncommon[randomIndexProvider(uncommon)];
                    else item =  common[randomIndexProvider(common)];
                    break;
            }
            if (!config.Items.ContainsKey(item)) config.Items.Add(item, 1);
            else config.Items[item] += 1;
            GlobalUserAccounts.SaveAccounts();
            return item;
        }

        public static byte randomIndexProvider(string[] list)
        {
            byte randomIndex = (byte) Global.Rng.Next(1, list.Length + 1);
            return randomIndex;
        }
    }
}