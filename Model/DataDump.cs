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
                new User() {FirstName = "James", SkypeName = "bubs75" },
                new User() {FirstName = "Billy", SkypeName = "ucariouk" },
                new User() {FirstName = "Dmitri", SkypeName = "ahumellihuk" },
                new User() {FirstName = "Dan", SkypeName = "forbesdaniel" }
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