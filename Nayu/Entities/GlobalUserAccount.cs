using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nayu.Entities
{
    public class Chomusuke
    {
        public bool Have { get; set; }
        public string Name { get; set; }
        public string Zodiac { get; set; }
        public bool Shiny { get; set; }
        public uint Hunger { get; set; }
        public uint XP { get; set; }
        public uint LevelNumber
        {
            get
            {
                return (uint)Math.Sqrt(XP / 200);
            }
        }
        public uint Trust { get; set; }
        public uint Waste { get; set; }
        public bool Sick { get; set; }
        public string Attack1 { get; set; }
        public string Attack2 { get; set; }
        public string Attack3 { get; set; }
        public string Attack4 { get; set; }
        public string Type { get; set; }
        public uint CP { get; set; } //combat power
        public string Trait1 { get; set; }
        public string Trait2 { get; set; }
        public uint Health { get; set; } //CP * 1.5
        public uint Shield { get; set; }
        public uint Mana { get; set; }
        public uint HealthCapacity { get; set; } //CP * 1.5
        public uint ShieldCapacity { get; set; }
        public uint ManaCapacity { get; set; }
        public int Control { get; set; } //-100 to 100
        public uint Wins { get; set; }
        public uint Losses { get; set; }

        public Chomusuke(bool have, string name, string zodiac, bool shiny, uint hunger, uint xp, uint trust, uint waste, bool sick, string attack1, string attack2, string attack3, 
            string attack4, string type, uint cp, string trait1, string trait2, uint health, uint shield, uint mana, uint healthcapacity, uint shieldcapacity, uint manacapacity, 
            int control, uint wins, uint losses)
        {
            Have = have;
            Name = name;
            Zodiac = zodiac;
            Shiny = shiny;
            Hunger = hunger;
            XP = xp;
            Trust = trust;
            Waste = waste;
            Sick = sick;
            Attack1 = attack1;
            Attack2 = attack2;
            Attack3 = attack3;
            Attack4 = attack4;
            Type = type;
            CP = cp;
            Trait1 = trait1;
            Trait2 = trait2;
            Health = health;
            Shield = shield;
            Mana = mana;
            HealthCapacity = healthcapacity;
            ShieldCapacity = shieldcapacity;
            ManaCapacity = manacapacity;
            Control = control;
            Wins = wins;
            Losses = losses;
        }
    }
    public class GlobalUserAccount : IGlobalAccount
    {
        public uint ConstatineMedallion { get; set; }
        public uint BookOfExodus { get; set; }
        public uint FireThread { get; set; }
        public uint SkyPowder { get; set; }
        public uint TearsOfHera { get; set; }
        public uint HornOfVeles { get; set; }
        public uint BranchOfYggdrasil { get; set; }
        public uint VolcanicRune { get; set; }
        public uint FlaskOfIchor { get; set; }
        public uint FlaskOfElixir { get; set; }
        public uint FlaskOfMana { get; set; }
        public uint ShardsOfImmortality { get; set; }
        public uint ChainsOfTartatus { get; set; }
        public uint ReviveCrystal { get; set; }
        public uint FreyasBlessing { get; set; }
        public Chomusuke Chomusuke1 { get; set; } //= new Chomusuke(false, null, null, false, 0, 0, 0, 0, false, null, null, null, null, null, 0, null, null, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        public Chomusuke Chomusuke2 { get; set; } //= new Chomusuke(false, null, null, false, 0, 0, 0, 0, false, null, null, null, null, null, 0, null, null, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        public Chomusuke Chomusuke3 { get; set; } //= new Chomusuke(false, null, null, false, 0, 0, 0, 0, false, null, null, null, null, null, 0, null, null, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        public Chomusuke ActiveChomusuke { get; set; }
        public uint NormalCapsule { get; set; }
        public uint ShinyCapsule { get; set; }
        public uint MythicalCapsule { get; set; }
        public string WhosTurn { get; set; }
        public string WhoWaits { get; set; }
        public string PlaceHolder { get; set; }
        public bool Fighting { get; set; }
        public ulong OpponentId { get; set; }
        public string OpponentName { get; set; }
        public uint Wins { get; set; }
        public uint Losses { get; set; }
        public uint Draws { get; set; }
        public uint WinStreak { get; set; }
        public string Title { get; set; }
        public uint LootBoxCommon { get; set; }
        public uint LootBoxRare { get; set; }
        public uint LootBoxUncommon { get; set; }
        public uint LootBoxEpic { get; set; }
        public uint LootBoxLegendary { get; set; }
        public uint XP { get; set; }
        public DateTime LastXPMessage { get; set; } = DateTime.UtcNow;
        public uint LevelNumber
        {
            get
            {
                return (uint)Math.Sqrt(XP / 50);
            }
        }
        public ulong Id { get; set; }
        public string OverwatchID { get; set; }
        public string OverwatchRegion { get; set; }
        public string OverwatchPlatform { get; set; }
        public ulong Taiyaki { get; set; }
        public ulong TaiyakiFromMessages { get; set; }
        public ulong TaiyakiFromGambling { get; set; }
        public DateTime LastDaily { get; set; } = DateTime.UtcNow.AddDays(-2);
        public DateTime LastMessage { get; set; } = DateTime.UtcNow;
        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
    }
}