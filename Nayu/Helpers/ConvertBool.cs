using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nayu.Modules;

namespace Nayu.Helpers
{
    public class ConvertBool : NayuModule
    {
        public static bool CheckStringToBoolean(string boolean)
        {
            if (boolean == "on" || boolean == "off") return true;
            else return false;
        }

        public static Tuple<bool, bool> ConvertStringToBoolean(string boolean) // <is it a bool, true / false>
        {
            bool result = CheckStringToBoolean(boolean);
            if (result == true)
            {
                if (boolean == "on") return new Tuple<bool, bool>(true, true);
                if (boolean == "off") return new Tuple<bool, bool>(true, false);
            }
            else
            {
                return new Tuple<bool, bool>(false, false);
            }
            return new Tuple<bool, bool>(false, false);
        }

        public static string ConvertBooleanOF(bool boolean)
        {
            return boolean == true ? "**On**" : "**Off**";
        }

        public static string ConvertBooleanYN(bool boolean)
        {
            return boolean == true ? "**Yes**" : "**No**";
        }
    }
}
