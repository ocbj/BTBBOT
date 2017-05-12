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

    [LuisModel("34a2c531-aba6-433a-841d-14ebf95eb628", "239ef3be43ec41c7b8a750cc73fbc542")]
    [Serializable]
    public class RootLuisDialog : LuisDialog<object>
    {
        private const string EntityGeographyCity = "builtin.geography.city";
        private const string EntityUserFirstName = "user::firstName";


        private const string EntityHotelName = "Hotel";

        private const string EntityAirportCode = "AirportCode";

        private IList<string> titleOptions = new List<string> { "“Very stylish, great stay, great staff”", "“good hotel awful meals”", "“Need more attention to little things”", "“Lovely small hotel ideally situated to explore the area.”", "“Positive surprise”", "“Beautiful suite and resort”" };


        /*
         * 
         *BlameSomeone  10    
 CheckUserInOffendersList  3    
 DenyResponsibility  12    
 FinishConversation  7    
 GetAssociatedJIRA  2    
 GetBrokenBuildsForDate  3    
 GetLastBrokenBuild  5    
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
                var message = "I don't think I needed to know that";
                await c.PostAsync(message);
                c.Wait(this.MessageReceived);
            };

            context.Wait(processDontKnow);
        }

        [LuisIntent("OffendBot")]
        public async Task OffendBot(IDialogContext context, LuisResult result)
        {
            string message = $"Do you talk to your mom like that? >_<";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("BlameSomeone")]
        public async Task BlameSomeone(IDialogContext context, LuisResult result)
        {
            EntityRecommendation userFirstNameRecommendation;

            if (result.TryFindEntity(EntityUserFirstName, out userFirstNameRecommendation))
            {
                string message = $"So you believe it was " + userFirstNameRecommendation.Entity + "? I'll go interrogate them";

                await context.PostAsync(message);
            } else
            {
                string message = $"It sounds like you were trying to blame someone, but failed miserably LOL!";

                await context.PostAsync(message);
            }


            context.Wait(this.MessageReceived);
        }
    }
}
