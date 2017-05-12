using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisBot.Model
{
    public static class DataDump
    {
        static DataDump()
        {
            BrokenBuilds = new List<BrokenBuild>();
            Users = new List<User>()
            {
                new User() {FirstName = "James", SkypeName = "bubs75", SkypeNiceName = "bubs75", BuildName = "james.barrass" },
                new User() {FirstName = "Billy", SkypeName = "ucariouk",  SkypeNiceName = "Billy O'Connor", BuildName = "billy.oconnor" },
                new User() {FirstName = "Dmitri", SkypeName = "ahumellihuk", SkypeNiceName = "Dmitri", BuildName = "dmitri.samoilov" },
                new User() {FirstName = "Dan", SkypeName = "forbesdaniel", SkypeNiceName = "Dan", BuildName = "daniel.forbes" }
            };
        }

        

        public static List<BrokenBuild> BrokenBuilds
        {
            get;
            set;
        }

        public static List<User> Users
        {
            get;
            set;
        }
    }
}