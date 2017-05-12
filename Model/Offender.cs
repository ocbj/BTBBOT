using System.Collections.Generic;

namespace LuisBot.Model
{
    internal class Offender : User
    {
        public Offender()
        {
        }

        public List<JIRA> JIRAs { get; internal set; }
    }
}