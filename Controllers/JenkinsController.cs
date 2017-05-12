using LuisBot.Model;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LuisBot.Controllers
{

    public class JenkinsController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]BrokeBuildJson data)
        {
            try
            {
                Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(data)); 


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