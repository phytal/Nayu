using Nayu.Core.Modules;
using System;

namespace Nayu.Modules.Chomusuke
{
    public class GetTraitsTypes : NayuModule
    {
        public static Tuple<bool, bool> GetTraitsAsync()
        {
            int chance = Global.Rng.Next(1, 101);
            if (chance > 20) return new Tuple<bool, bool>(false, false);
            else if (chance > 4) return new Tuple<bool, bool>(true, false); //lucky
            else if (chance > 1) return new Tuple<bool, bool>(false, true); //shiny
            else return new Tuple<bool, bool>(true, true); //both
        }

        public static string GetTypeAsync(int value)
        {
            if (value > 96) return "Nature"; //4%
            else if (value > 92) return "Chaos"; //4%
            else if (value > 69) return "Water"; //23%
            else if (value > 46) return "Fire"; //23%
            else if (value > 23) return "Wind"; //23%
            else return "Earth"; //23%
        }
    }
}
