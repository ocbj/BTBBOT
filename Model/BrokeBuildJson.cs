using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisBot.Model
{
    public class AffectedFile
    {
        public string editType { get; set; }
        public string path { get; set; }
    }

    public class Changelog
    {
        public string author { get; set; }
        public string revision { get; set; }
        public string message { get; set; }
        public string date { get; set; }
        public List<AffectedFile> affectedFiles { get; set; }
    }

    public class BrokeBuildJson
    {
        public string buildNumber { get; set; }
        public string jobName { get; set; }
        public List<Changelog> changelog { get; set; }
    }
}