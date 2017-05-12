using System.Collections.Generic;

namespace LuisBot.Model
{
    public class Offender : User
    {
        public Offender()
        {
        }

        public List<JIRA> JIRAs { get; internal set; }
    }
}