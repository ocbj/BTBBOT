namespace LuisBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;
    using Microsoft.Bot.Connector;
    using Model;
    using System.Text.RegularExpressions;

    [LuisModel("34a2c531-aba6-433a-841d-14ebf95eb628", "239ef3be43ec41c7b8a750cc73fbc542")]
    [Serializable]
    public class RootLuisDialog : LuisDialog<object>
    {
        private Random random = new Random();

        private const string EntityGeographyCity = "builtin.geography.city";
        private const string EntityUserFirstName = "user::firstName";

        private void CheckMe(IDialogContext context)
        {
            if (!context.UserData.ContainsKey("me"))
            {
                var myName = context.Activity.From.Name;
                var user = DataDump.Users.Where(u => u.FirstName == myName).FirstOrDefault();
                context.UserData.SetValue("me", user);
            }
        }

        /*
         * 
         *
         * 
         BlameSomeone  10    
         CheckUserInOffendersList  3    
         DenyResponsibility  12    
         FinishConversation  7    
         GetAssociatedJIRA  2    
         GetOtherOffenders  2    
         None  0    
         OffendBot  8    
         TakeResponsibility  16   

         */

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"What does {result.Query} mean?");

            ResumeAfter<object> processDontKnow = async (c, s) =>
            {
                await s;
                var messages = new[] { "I don't think I needed to know that", "I'm not sure that's useful", "Actually, I don't care", "Whatever", "That's not very interesting", "Let's just put that on the back burner" };

                await c.PostAsync(messages[random.Next(messages.Length)]);
                c.Wait(this.MessageReceived);
            };

            context.Wait(processDontKnow);
        }

        [LuisIntent("OffendBot")]
        public async Task OffendBot(IDialogContext context, LuisResult result)
        {
            var messages = new[] { "Do you talk to your mom like that? >_<", "How dare you", "That's not very nice", "Are you drunk?", ":O", "I don't like you anymore" };

            await context.PostAsync(messages[random.Next(messages.Length)]);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("BlameSomeone")]
        public async Task BlameSomeone(IDialogContext context, LuisResult result)
        {
            EntityRecommendation userFirstNameRecommendation;

            if (result.TryFindEntity(EntityUserFirstName, out userFirstNameRecommendation))
            {
                if (random.NextDouble() > 0.3)
                {
                    Utils.Blame(userFirstNameRecommendation.Entity);

                    var messages = new[] {
                    $"So you believe it was {userFirstNameRecommendation.Entity}? I'll go interrogate them",
                    $"I agree, {userFirstNameRecommendation.Entity} is looking kind of guilty",
                    $"{userFirstNameRecommendation.Entity}? No way! Ha! I'll make sure they fix it",
                    $"ok",
                    $"If you're lying you'll regret this",
                    $"..."
                };

                    await context.PostAsync(messages[random.Next(messages.Length)]);
                }
                else
                {
                    var messages = new[] {
                    "It sounds like you were trying to blame someone, but failed miserably LOL!",
                    "Whatever, it was you!",
                    "Sorry, I don't believe you",
                    "I'm not going to fall for that trick"
                    };

                    await context.PostAsync(messages[random.Next(messages.Length)]);
                }
            }
            else
            {
                var messages = new[] {
                    "Who are you talking about",
                    "Whatever, it was you!",
                    "I don't know who that iss",
                    "I'm going for a coffee, when I get back I'm hoping you make sense"
                    };

                await context.PostAsync(messages[random.Next(messages.Length)]);
            }

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("CheckUserInOffendersList")]
        public async Task CheckUserInOffendersList(IDialogContext context, LuisResult result)
        {
            EntityRecommendation userFirstNameRecommendation;

            string message;
            if (DataDump.BrokenBuilds.Any())
            {
                if (result.TryFindEntity(EntityUserFirstName, out userFirstNameRecommendation))
                {
                    var user = DataDump.BrokenBuilds.Last().Offenders.Where(u => u.FirstName == userFirstNameRecommendation.Entity).FirstOrDefault();
                    if (user != null)
                    {
                        message = $"Well I suppose it might have been {user.FirstName}";
                    }
                    else
                    {
                        message = $"Definitely not, they had nothing to do with it";
                    }
                }
                else
                {
                    message = $"I don't know who you're talking about";
                }
            }
            else
            {
                message = $"I don't have any builds";
            }

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("DenyResponsibility")]
        public async Task DenyResponsibility(IDialogContext context, LuisResult result)
        {
            CheckMe(context);
            if (DataDump.BrokenBuilds.Any())
            {
                var user = DataDump.BrokenBuilds.Last().Offenders.Where(u => u.FirstName == context.UserData.GetValue<User>("me").FirstName).FirstOrDefault();
                if (user == null)
                    await context.PostAsync("I know");
                else
                    await context.PostAsync("Ok");
            }
            else
            {
                await context.PostAsync("Obviously!");
            }

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("FinishConversation")]
        public async Task FinishConversation(IDialogContext context, LuisResult result)
        {
            await context.PostAsync((Activity)Activity.CreateEndOfConversationActivity());

            context.Done<object>(null);
        }

        [LuisIntent("GetAssociatedJIRA")]
        public async Task GetAssociatedJIRAS(IDialogContext context, LuisResult result)
        {
            CheckMe(context);
            if (DataDump.BrokenBuilds.Any())
            {
                var user = DataDump.BrokenBuilds.Last().Offenders.Where(u => u.FirstName == context.UserData.GetValue<User>("me").FirstName).FirstOrDefault();
                if (user == null)
                    await context.PostAsync("You don't have a JIRA");
                else
                    await context.PostAsync("This should be the users JIRA list");
            }
            else
            {
                await context.PostAsync("There are no builds");
            }
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("GetOtherOffenders")]
        public async Task GetOtherOffenders(IDialogContext context, LuisResult result)
        {
            CheckMe(context);
            if (DataDump.BrokenBuilds.Any())
            {
                var users = DataDump.BrokenBuilds.Last().Offenders.Where(u => u.FirstName != context.UserData.GetValue<User>("me").FirstName);
                if (users.Count() > 1)
                {
                    await context.PostAsync(string.Join(",", users.Skip(1).Select(u => u.FirstName) + "and" + users.First().FirstName));
                }
                else if (users.Count() == 1)
                {
                    await context.PostAsync(users.First().FirstName);
                }
                else
                {
                    await context.PostAsync("There's no one else, admit it. It was you");
                }
            }
            else
            {
                await context.PostAsync("I find it offensive that you think there's someone to blame for this");
            }

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("TakeResponsibility")]
        public async Task TakeResponsibility(IDialogContext context, LuisResult result)
        {
            string message = $"OK, Great";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }


    }
}
