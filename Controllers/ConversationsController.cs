using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace LuisBot.Controllers
{
    /// <summary>
    /// Initiates a conversation with the bot
    /// </summary>
    public class ConversationsController : ApiController
    {
        /// <summary>
        /// First message that gets sent, with appropriate links and shit
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IMessageActivity WelcomeMessage(ConversationAccount conversation, string username)
        {
            var msg = Activity.CreateMessageActivity();
            msg.From = new ChannelAccount("177b2eem05dc4542ac", "Bot");
            msg.Recipient = new ChannelAccount("default-user", "default-user");
            msg.Conversation = conversation;
            msg.ServiceUrl = "http://localhost:3979/api/messages";

            HeroCard heroCard = new HeroCard()
            {
                Title = "Someone broke the build.",
                Subtitle = $"Was it you {username}?",
                Images = new List<CardImage>() { new CardImage() { Url = "https://uploads.toptal.io/blog/image/92347/toptal-blog-image-1460406405672-52ec53e6624f51828dab1aee43efe75a.jpg" } },
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Title = "Show in JIRA",
                        Type = ActionTypes.OpenUrl,
                        Value = $"https://jira.vermilionreporting.com/browse/" + HttpUtility.UrlEncode("VP-319")
                    },
                    new CardAction()
                    {
                        Title = "Show in Jenkins",
                        Type = ActionTypes.OpenUrl,
                        Value = $"https://jira.vermilionreporting.com/browse/" + HttpUtility.UrlEncode("VP-319")
                    }
                }
            };

            msg.Attachments.Add(heroCard.ToAttachment());

            return msg;
        }

        public IMessageActivity CreateMessage(ConversationAccount conversation, string text)
        {
            var msg = Activity.CreateMessageActivity();
            msg.From = new ChannelAccount("177b2eem05dc4542ac", "Bot");
            msg.Recipient = new ChannelAccount("default-user", "default-user");
            msg.Conversation = conversation;
            msg.ServiceUrl = "http://localhost:3979/api/messages";
            msg.Text = text;

            return msg;
        }


        /// <summary>
        /// Get api/conversations/{username}
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get([FromUri]string username)
        {
            try
            {
                var from = new ChannelAccount("177b2eem05dc4542ac", "Bot");
                var recipient = new ChannelAccount("default-user", "default-user");

                // 
                var connector = new ConnectorClient(new Uri("http://localhost:54424"), "", "");

                // Add convo
                var conversations = connector.Conversations;
                var conversation = conversations.CreateDirectConversation(from, recipient);

                var newMessage = WelcomeMessage(new ConversationAccount(id: conversation.Id), username);
                var createNessage = CreateMessage(new ConversationAccount(id: conversation.Id), "So, spill the beans. Was it you?");

                await connector.Conversations.SendToConversationAsync((Activity)newMessage);
                await connector.Conversations.SendToConversationAsync((Activity)createNessage);

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