namespace LuisBot
{
    using System;
    using Microsoft.Bot.Builder.FormFlow;

    [Serializable]
    public class DontKnowQuery
    {
        [Prompt("What does {&} mean?")]
        [Optional]
        public string Meaning { get; set; }
    }
}