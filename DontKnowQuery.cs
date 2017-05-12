namespace LuisBot
{
    using System;
    using Microsoft.Bot.Builder.FormFlow;

    [Serializable]
    public class DontKnowQuery
    {
        [Prompt("What does that mean?")]
        public string Meaning { get; set; }
    }
}