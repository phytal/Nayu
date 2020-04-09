using System;
using Type = Nayu.Modules.Chomusuke.Dueling.Enums.Type;

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

        public static Type GetTypeAsync(int value)
        {
            if (value > 96) return Type.Nature; //4%
            else if (value > 92) return Type.Chaos; //4%
            else if (value > 69) return Type.Water; //23%
            else if (value > 46) return Type.Fire; //23%
            else if (value > 23) return Type.Wind; //23%
            else return Type.Earth; //23%
        }
    }
}