using LuisBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LuisBot.Controllers
{
    public class ConversationsController : ApiController
    {
        /// <summary>
        /// Get api/conversations/{username}
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get([FromUri]string username)
        {
            var newMessage = Activity.CreateMessageActivity();
            newMessage.From = new ChannelAccount("botId", "botName");
            newMessage.Recipient = new ChannelAccount("email", "name");
            newMessage.ServiceUrl = "http://BrokenBuildBot/Uri/App";
            newMessage.Text = "Hello World";
            var connector = new ConnectorClient(new Uri("http://BrokenBuildBot/Uri/App"), "AppId", "AppPassword");
            var conversation = connector.Conversations.CreateDirectConversation(newMessage.From, newMessage.Recipient);
            newMessage.Conversation.Id = conversation.Id;
            await connector.Conversations.SendToConversationAsync((Activity)newMessage);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}