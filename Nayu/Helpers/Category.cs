using System.ComponentModel;

namespace Nayu.Helpers
{
    public enum Categories
    {
        //General Commands
        Fun,
        Music,
        [Description("Game Stats")] GameStats,
        Economy,
        Chomusuke,
        

        //Admin Commands

        Management,
        Server,
        Bot

        
    }

    public class Subject : System.Attribute
    {
        private Categories _categories;

        public Subject(Categories category)
        {
            _categories = category;
        }

        public Categories GetCategory()
        {
            return _categories;
        }
    }
}