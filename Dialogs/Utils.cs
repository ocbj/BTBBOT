using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace LuisBot.Dialogs
{
    public class Utils
    {
        public static void Blame(string name)
        {
            var user = Model.DataDump.Users.Where(u => string.Compare(u.FirstName, name, true) == 0).Select(u => u.SkypeName).FirstOrDefault();
            string html = string.Empty;
            var endPoint = $"http://btbbot.azurewebsites.net/api/conversations?username={user}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) ;
        }
    }
}