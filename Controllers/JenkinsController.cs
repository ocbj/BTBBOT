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





        public async Task<HttpResponseMessage> Post([FromBody]BrokeBuildJson data)
        {
            try
            {
                var user = new User() { SkypeName = "ucariouk", FirstName =  "Bill" }; //  DataDump.Users[r.Next(DataDump.Users.Count())];

                var from = new ChannelAccount(botId, "Bot");
                var recipient = new ChannelAccount(user.SkypeName, user.FirstName);
                var connector = new ConnectorClient(new Uri(connectorUrl), id, pass);
                var conversations = connector.Conversations;
                var conversation = conversations.CreateDirectConversation(from, recipient);

                // create msg
                var msg = Activity.CreateMessageActivity();
                msg.From = new ChannelAccount(botId, "BTBBot");
                msg.Recipient = new ChannelAccount(user.SkypeName, user.FirstName);
                msg.Conversation = new ConversationAccount(id: "8:" + conversation.Id);
                msg.ServiceUrl = svcUrl;

                var changeLog = data.changelog.OrderByDescending(x => x.date)
                    .Select(x => new CardAction()
                    {
                        Title = $"{x.author} revision {x.revision}",
                        Type = ActionTypes.OpenUrl,
                        Value = $"https://jira.vermilionreporting.com/browse/" + HttpUtility.UrlEncode("VP-319")
                    });

                HeroCard heroCard = new HeroCard()
                {
                    Title = $"Someone broke build #{data.buildNumber}.",
                    Subtitle = $"Was it you {user.FirstName}?",
                    Images = new List<CardImage>() { new CardImage() { Url = "http://btbbot.azurewebsites.net/wow.jpg" } },
                    Buttons = changeLog.ToList()
                };

                msg.Attachments.Add(heroCard.ToAttachment());

                await connector.Conversations.SendToConversationAsync((Activity)msg);

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