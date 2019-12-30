namespace Nayu.Modules.Chomusuke.Dueling
{
    public class Helpers
    {
        public static bool GetCritical()
        {
            int hit = Global.Rng.Next(1, 8);
            if (hit == 2) return true;
            else return false;
        }
    }
    public class AttackCommand
    {
        public bool Success { get; set; } 
        public string Response { get; set; }
        public int Damage { get; set; }
    }
}