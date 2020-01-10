using System.ComponentModel;

namespace Nayu.Helpers
{
    public enum Categories
    {
        //General Commands
        Core,
        Fun,
        Interaction,
        [Description("Economy/Gambling")] EconomyGambling,
        Images,
        Music,
        [Description("Game Stats")] GameStats,
        Lootboxes,
        Chomusuke,
        [Description("Self Roles")] SelfRoles,
        [Description("Personal Tags")] PersonalTags,
        Overwatch, 
        [Description("osu!")] osu,
    }

    public enum AdminCategories
    {
        Management,
        Server,
        Bot
    }

    public class Subject : System.Attribute
    {
        private Categories _categories;
        private AdminCategories _adminCategories;

        public Subject(Categories category)
        {
            _categories = category;
        }

        public Categories GetCategories()
        {
            return _categories;
        }
        
        public Subject(AdminCategories category)
        {
            _adminCategories = category;
        }

        public AdminCategories GetAdminCategories()
        {
            return _adminCategories;
        }
    }
}