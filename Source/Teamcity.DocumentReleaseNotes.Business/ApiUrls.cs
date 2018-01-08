using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;


namespace Teamcity.DocumentReleaseNotes.Business
{
    public class ApiUrls
    {
        public ApiUrls(string url, ILogger logger = null, ApiConfigManager _apiMgr = null)
        {
            GetProjectByIdUrl = ConcatUrl(url, _apiMgr?.Configuration["TeamCityProjectById"]);
            GetBuildsUrl = ConcatUrl(url, _apiMgr?.Configuration["TeamCityBuilds"]);
            GetBuildUrl = ConcatUrl(url, _apiMgr?.Configuration["TeamCityBuild"]);
            GetChangesUrl = ConcatUrl(url, _apiMgr?.Configuration["TeamCityChanges"]);
            GetChangeUrl = ConcatUrl(url, _apiMgr?.Configuration["TeamCityChange"]);
            GetBuildTypeUrl = ConcatUrl(url, _apiMgr?.Configuration["TeamCityBuildType"]);
        }

        private string ConcatUrl(string str1, string str2)
        {
            return String.Format("{0}/{1}", str1.TrimEnd('/'), str2.TrimStart('/'));
        }
        public string GetProjectByIdUrl { get; set; }
        public string GetBuildTypeUrl { get; set; }
        public string GetBuildsUrl { get; set; }
        public string GetBuildUrl { get; set; }
        public string GetChangesUrl { get; set; }
        public string GetChangeUrl { get; set; }

        public string GetProjectByNameId(string name)
        {
            return String.Format(GetProjectByIdUrl, name);
        }

        public string GetBuilds(string buildName)
        {
            return String.Format(GetBuildsUrl, buildName);
        }

        public string GetBuild(int buildId)
        {
            return String.Format(GetBuildUrl, buildId);
        }

        public string GetChanges(long buildId)
        {
            return String.Format(GetChangesUrl, buildId);
        }

        public string GetChange(long changeId)
        {
            return String.Format(GetChangeUrl, changeId);
        }

        public string GetBuildTypeByNameId(string name)
        {
            return String.Format(GetBuildTypeUrl, name);
        }

    }
}
