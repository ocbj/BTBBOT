using LuisBot.Model;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace LuisBot.Controllers
{

//    {
//  "buildNumber": "15",
//  "jobName": "Build #1",
//  "changelog": [
//    {
//      "author": "Dmitri.Samoilov",
//      "revision": "39",
//      "message": "Test change #5 [Hackathon]",
//      "date": "12/05/2017 03:28:35",
//      "affectedFiles": [
//        {
//          "editType": "edit",
//          "path": "Restore Backup and Upgrade to 4.0.sql"
//        },
//        {
//          "editType": "edit",
//          "path": "VRSDB User Tools and Create Accounts.sql"
//        }
//      ]
//    },
//    {
//      "author": "Dmitri.Samoilov",
//      "revision": "38",
//      "message": "Test change #4 [Hackathon]",
//      "date": "12/05/2017 03:27:46",
//      "affectedFiles": [
//        {
//          "editType": "edit",
//          "path": "Restore Backup and Upgrade to 4.0.sql"
//        },
//        {
//          "editType": "edit",
//          "path": "VRSDB Global Update User and TID data.sql"
//        }
//      ]
//    }
//  ]
//}

    public class JenkinsController : ApiController
    {
        const string connectorUrl = "https://smba.trafficmanager.net/apis";
        const string id = "ac99631b-69e2-4619-b585-c12a813a7b16";
        const string pass = "53y7pDmxpmPTGipgGjJOgE2";
        const string svcUrl = "http://btbbot.azurewebsites.net/api/messages";
        const string botId = "l7i8emhc2i987i0gl";

        Random r = new Random();


        /// <summary>
        /// First message that gets sent, with appropriate links
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IMessageActivity WelcomeMessage(ConversationAccount conversation, string username)
        {
            var msg = Activity.CreateMessageActivity();
            msg.From = new ChannelAccount(botId, "BTBBot");
            msg.Recipient = new ChannelAccount(username, username);
            msg.Conversation = conversation;
            msg.ServiceUrl = svcUrl;

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


        public IMessageActivity CreateMessage(ConversationAccount conversation, string username, string text)
        {
            var msg = Activity.CreateMessageActivity();
            msg.From = new ChannelAccount(botId, "Bot");
            msg.Recipient = new ChannelAccount(username, username);
            msg.Conversation = conversation;
            msg.ServiceUrl = svcUrl;
            msg.Text = text;

            return msg;
        }


        public async Task<HttpResponseMessage> Post([FromBody]BrokeBuildJson data)
        {
            try
            {
                var user = DataDump.Users[r.Next(DataDump.Users.Count())];

                var changeLog = data.changelog
                    .Select(x => new CardAction(type: ActionTypes.OpenUrl, title: $"author {x.author} revision {x.revision}", value: ""));

                HeroCard heroCard = new HeroCard()
                {
                    Title = $"Someone broke the build #{data.buildNumber}.",
                    Subtitle = $"Was it you {user.FirstName}?",
                    Images = new List<CardImage>() { new CardImage() { Url = "https://uploads.toptal.io/blog/image/92347/toptal-blog-image-1460406405672-52ec53e6624f51828dab1aee43efe75a.jpg" } },
                    Buttons = changeLog.ToList()
                };


                var from = new ChannelAccount(botId, "Bot");
                var recipient = new ChannelAccount(user.SkypeName, user.FirstName);
                var connector = new ConnectorClient(new Uri(connectorUrl), id, pass);
                var conversations = connector.Conversations;
                var conversation = conversations.CreateDirectConversation(from, recipient);

                var newMessage = WelcomeMessage(new ConversationAccount(id: "8:" + conversation.Id), user.SkypeName);

                await connector.Conversations.SendToConversationAsync((Activity)newMessage);
            }
            catch (Exception e)
            {
                string AwesomeError = e.Message + Environment.NewLine + e.StackTrace;
                Exception inner = e.InnerException;
                while (inner != null)
                {
                    AwesomeError += inner.Message + Environment.NewLine + inner.StackTrace;
                    inner = inner.InnerException;
                }
                return Request.CreateResponse(HttpStatusCode.OK, AwesomeError);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}