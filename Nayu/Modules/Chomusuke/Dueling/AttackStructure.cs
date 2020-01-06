using System.Collections.Generic;
using Nayu.Modules.Chomusuke.Dueling.Enums;

namespace Nayu.Modules.Chomusuke.Dueling
{
    public struct AttackStructure
    {
        public string Name { get; set; }
        
        public int Damage { get; set; }
        
        public uint Mana { get; set; }
        
        public uint Accuracy { get; set; }
        
        public List<Type> Types { get; set; }
        
        public List<Effect> Effects { get; set; }
    }
}