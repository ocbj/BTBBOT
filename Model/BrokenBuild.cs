using System.Collections.Generic;

namespace LuisBot.Model
{
    public class BrokenBuild
    {
        public List<User> Offenders { get; internal set; }
        public string Job { get; internal set; }
        public string Build { get; internal set; }
    }
}