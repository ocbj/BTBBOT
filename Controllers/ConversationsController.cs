using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LuisBot.Controllers
{
    /// <summary>
    /// Initiates a conversation with the bot
    /// </summary>
    public class ConversationsController : ApiController
    {

        /// <summary>
        /// Get api/conversations/{username}
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get([FromUri]string username)
        {
            try
            {
                var newMessage = Activity.CreateMessageActivity();
                newMessage.From = new ChannelAccount("177b2eem05dc4542ac", "Bot");
                newMessage.Recipient = new ChannelAccount("default-user", "default-user");
                newMessage.ServiceUrl = "http://localhost:3979/api/messages";
                newMessage.Text = $"Hello {username}";

                HeroCard heroCard = new HeroCard()
                {
                    Title = "You broke the build bro?",
                    Subtitle = $"I think you broke the build bro",
                    Images = new List<CardImage>()
                        {
                            new CardImage() { Url = "https://placeholdit.imgix.net/~text?txtsize=33&txt=350%C3%97150&w=350&h=150" }
                            //    },
                            //Buttons = new List<CardAction>()
                            //    {
                            //        new CardAction()
                            //        {
                            //            Title = "More details",
                            //            Type = ActionTypes.OpenUrl,
                            //            Value = $"https://www.bing.com/search?q=hotels+in+" + HttpUtility.UrlEncode(hotel.Location)
                            //        }
                            //    }
                    }
                };

                newMessage.Attachments.Add(heroCard.ToAttachment());



                var connector = new ConnectorClient(new Uri("http://localhost:54424"), "", "");
                var conversations = connector.Conversations;
                var conversation = conversations.CreateDirectConversation(newMessage.From, newMessage.Recipient);
                newMessage.Conversation = new ConversationAccount(id: conversation.Id);

                newMessage.Conversation.Id = conversation.Id;
                await connector.Conversations.SendToConversationAsync((Activity)newMessage);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.OK, e.InnerException?.Message ?? e.Message); 
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}