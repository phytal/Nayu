namespace Nayu.Modules.Chomusuke
{
    public enum Trait
    {
        None,
        Shiny,
        Lucky,
        Regeneration, //regens a set amount per turn
        Cooperative, //more trust and xp gain
        Enthusiastic, //More hp = more damage
        Lazy, //less mana capacity
        Resilient, //less damage taken, more health
        Holy,
        Clumsy, //attack misses more often, less xp gain, less control gain/loss
        Cute //cheaper to maintain, opponent misses attacks more
    }
}