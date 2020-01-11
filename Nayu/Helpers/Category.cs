using System.ComponentModel;

namespace Nayu.Helpers
{
    public enum Categories
    {
        None,
        //General Commands
        Information,
        Fun,
        Interaction,
        [Description("Economy/Gambling")] EconomyGambling,
        Images,
        Music,
        Inbox,
        Lootboxes,
        [Description("Self Roles")] SelfRoles,
        [Description("Personal Tags")] PersonalTags,
        Overwatch, 
        [Description("osu!")] osu,
        Other,
    }
    public enum ChomusukeCategories
    {
        None,
        Chomusuke,
    }
    public enum AdminCategories
    {
        None,
        Filters,
        [Description("User Management")] UserManagement,
        [Description("Server Management")] ServerManagement,
        [Description("Bot Settings")] BotSettings,
        [Description("Welcome Messages")] WelcomeMessages,
        [Description("Leaving Messages")] LeavingMessages,
        Roles,
        [Description("Server Tags")] ServerTags,
        [Description("Fun Stuff")] FunStuff,
        NSFW,
    }
    
    public enum NSFWCategories
    {
        None,
        Neko,
        Hentai,
    }
    
    public enum OwnerCategories
    {
        None,
        Owner,
    }

    public class Subject : System.Attribute
    {
        private Categories _categories;
        private AdminCategories _adminCategories;
        private NSFWCategories _nsfwCategories;
        private OwnerCategories _ownerCategories;
        private ChomusukeCategories _chomusukeCategories;
        
        public Subject(Categories category)
        {
            _categories = category;
        }

        public Categories GetCategories()
        {
            var cat = _categories;
            _categories = Categories.None;
            return cat;
            
        }
        
        public Subject(AdminCategories category)
        {
            _adminCategories = category;
        }

        public AdminCategories GetAdminCategories()
        {
            var cat = _adminCategories;
            _adminCategories = AdminCategories.None;
            return cat;
        }
        public Subject(NSFWCategories category)
        {
            _nsfwCategories = category;
        }

        public NSFWCategories GetNSFWCategories()
        {
            var cat = _nsfwCategories;
            _nsfwCategories = NSFWCategories.None;
            return cat;
        }
        public Subject(OwnerCategories category)
        {
            _ownerCategories = category;
        }

        public OwnerCategories GetOwnerCategories()
        {
            var cat = _ownerCategories;
            _ownerCategories = OwnerCategories.None;
            return cat;
        }
        public Subject(ChomusukeCategories category)
        {
            _chomusukeCategories = category;
        }

        public ChomusukeCategories GetChomusukeCategories()
        {
            var cat = _chomusukeCategories;
            _chomusukeCategories = ChomusukeCategories.None;
            return cat;
        }
    }
}