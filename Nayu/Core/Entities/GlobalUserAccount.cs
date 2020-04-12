using System;
using System.Collections.Generic;
using Nayu.Modules.Chomusuke;
using Nayu.Modules.Chomusuke.Dueling.Enums;
using Type = Nayu.Modules.Chomusuke.Dueling.Enums.Type;

namespace Nayu.Core.Entities
{

    #region Chomusuke

    public class Chomusuke
    {
        public bool Have { get; set; }
        public string Name { get; set; }
        public string Zodiac { get; set; }
        public bool Shiny { get; set; }
        public byte Hunger { get; set; }
        public uint Xp { get; set; }
        public uint LevelNumber
        {
            get
            {
                return (uint)Math.Sqrt(Xp / 200);
            }
        }
        public byte Trust { get; set; }
        public byte Waste { get; set; }
        public bool Sick { get; set; }
        public string Attack1 { get; set; }
        public string Attack2 { get; set; }
        public string Attack3 { get; set; }
        public string Attack4 { get; set; }
        public Type Type { get; set; }
        public uint CP { get; set; } //combat power
        public Trait Trait1 { get; set; }
        public Trait Trait2 { get; set; }
        public uint Health { get; set; } //CP * 1.5
        public uint Shield { get; set; }
        public uint Mana { get; set; }
        public uint HealthCapacity { get; set; } //CP * 1.5
        public uint ShieldCapacity { get; set; }
        public uint ManaCapacity { get; set; }
        public sbyte Control { get; set; } //-100 to 100
        public uint Wins { get; set; }
        public uint Losses { get; set; }
        public uint Draws { get; set; }
        public DateTime BoughtDay { get; set; }
        public List<string> Attacks { get; set; }
        public List<Effect> Effects { get; set; }
        public Dictionary<string, byte> PotionEffects { get; set; }

        public Chomusuke(bool have, string name, string zodiac, bool shiny, byte hunger, uint xp, byte trust, byte waste, bool sick, string attack1, string attack2, string attack3, 
            string attack4, Type type, uint cp, Trait trait1, Trait trait2, uint health, uint shield, uint mana, uint healthcapacity, uint shieldcapacity, uint manacapacity, 
            sbyte control, uint wins, uint losses, uint draws, DateTime boughtDay, List<string> attacks, List<Effect> effects, Dictionary<string,byte> potionEffects)
        {
            Have = have;
            Name = name;
            Zodiac = zodiac;
            Shiny = shiny;
            Hunger = hunger;
            Xp = xp;
            Trust = trust; //gained by feeding in time, lost by starving and not cleaning up
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
            Draws = draws;
            BoughtDay = boughtDay;
            Attacks = attacks;
            Effects = effects;
            PotionEffects = potionEffects;
        }
    }

    #endregion

    public class Message
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public bool Read { get; set; }
        public ulong Id { get; set; }

        public Message(string title, string content, DateTime time, bool read, ulong id)
        {
            Title = title;
            Content = content;
            Time = time;
            Read = read;
            Id = id;
        }
    }

    public class GlobalUserAccount : IGlobalAccount
    {
        public Chomusuke Chomusuke1 { get; set; } 

        public Chomusuke Chomusuke2 { get; set; }

        public Chomusuke Chomusuke3 { get; set; } 

        public byte ActiveChomusuke { get; set; }
        public ushort NormalCapsule { get; set; }
        public ushort ShinyCapsule { get; set; }
        public ushort MythicalCapsule { get; set; }
        public string WhosTurn { get; set; }
        public string WhoWaits { get; set; }
        public string PlaceHolder { get; set; }
        public bool Fighting { get; set; }
        public ulong OpponentId { get; set; }
        public string OpponentName { get; set; }
        public ushort Wins { get; set; }
        public ushort Losses { get; set; }
        public ushort Draws { get; set; }
        public ushort WinStreak { get; set; }
        public string Title { get; set; }
        public ushort LootBoxCommon { get; set; }
        public ushort LootBoxRare { get; set; }
        public ushort LootBoxUncommon { get; set; }
        public ushort LootBoxEpic { get; set; }
        public ushort LootBoxLegendary { get; set; }
        public uint Xp { get; set; }
        public DateTime LastXpMessage { get; set; } = DateTime.UtcNow;

        public uint LevelNumber =>
            (uint) Math.Sqrt(Xp / 50);

        public ulong Id { get; set; }
        public string OwId { get; set; }
        public string OwRegion { get; set; }
        public string OwPlatform { get; set; }
        public ulong Taiyaki { get; set; }
        public string Armour { get; set; }
        public string Weapon { get; set; }
        public string Blessing { get; set; }
        public ulong InboxIdTracker { get; set; }
        public ulong InboxIdLastRead { get; set; }
        public DateTime LastDaily { get; set; } = DateTime.UtcNow.AddDays(-2);
        public DateTime LastMessage { get; set; } = DateTime.UtcNow;
        public List<Message> Inbox { get; set; } = new List<Message>();
        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, ushort> Items { get; set; } = new Dictionary<string, ushort>();
        /*
        public ushort ConstantineMedallion { get; set; } //decreases control (chaos) e
        public ushort BookOfExodus { get; set; } //increases control (nature) e
        public ushort FireThread { get; set; }//fire type r
        public ushort SkyPowder { get; set; }//wind type r
        public ushort TearsOfHera { get; set; }//water type r
        public ushort HornOfVeles { get; set; }//chaos type r 
        public ushort BranchOfYggdrasil { get; set; }//nature type r
        public ushort VolcanicRune { get; set; }//special fire type attack that can be given to any type e
        public ushort FlaskOfIchor { get; set; }//boosts cp drastically e
        public ushort FlaskOfElixir { get; set; }//boosts hp e
        public ushort FlaskOfMana { get; set; }//boosts mana capacity e
        public ushort ShardsOfImmortality { get; set; }//used to craft items c
        public ushort ChainsOfTartarus { get; set; }//locks control l
        public ushort ReviveCrystal { get; set; }//revives a chomusuke c
        public ushort FreyasBlessing { get; set; }//boosts xp gain l
        //dice of god - use on chomusuke, boosts stats but 1% death chance l
        //mead of poetry- very efficient mana usage l
         */
    }
}