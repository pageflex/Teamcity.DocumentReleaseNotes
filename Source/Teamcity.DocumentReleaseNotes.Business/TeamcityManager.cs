using Microsoft.Extensions.Logging;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Teamcity.DocumentReleaseNotes.Business;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Teamcity.DocumentReleaseNotes.Models;
using Teamcity.DocumentReleaseNotes.Interfaces;
namespace Teamcity.DocumentReleaseNotes.Business
{
    public class TeamcityManager : ITeamcityManager
    {
        private string _urlBase;
        private string _userName;
        private string _password;
        private string _serverAuth;
        private ILogger _logger;
        private ApiUrls _urls;

        public TeamcityManager(IConfigurationManager _cMgr = null, ApiConfigManager _apiMgr = null, ILogger<TeamcityManager> logger = null)
        {
            _logger = logger;
            SetAppConfiguration(_cMgr);
            SetApiUrls(_cMgr, _apiMgr);

        }

        public async Task GetTestData(string projectName, string buildType, int buildId, int changeId)
        {
            using (var client = GetClient())
            {
                var projectTask = client.GetStringAsync(_urls.GetProjectByNameId(projectName));
                var pMsg = await projectTask;
                var pData = TeamCityProject.FromJson(pMsg);


                var buildsTask = client.GetStringAsync(_urls.GetBuilds(buildType));
                var bsMsg = await buildsTask;
                var bsData = TeamCityBuilds.FromJson(bsMsg);

                var buildTask = client.GetStringAsync(_urls.GetBuild(buildId));
                var bMsg = await buildTask;
                var bData = TeamCityBuild.FromJson(bMsg);


                var changesTask = client.GetStringAsync(_urls.GetChanges(buildId));
                var csMsg = await changesTask;
                var csData = TeamCityChanges.FromJson(csMsg);

                var changeTask = client.GetStringAsync(_urls.GetChange(changeId));
                var cMsg = await changeTask;
                var cData = TeamCityChange.FromJson(cMsg);
            }
        }

        public async Task<TeamCityProject> GetProjectData(string ProjectNameId)
        {
            try
            {
                using (var client = GetClient())
                {
                    var projectTask = client.GetStringAsync(_urls.GetProjectByNameId(ProjectNameId));
                    var pMsg = await projectTask;
                    var pData = TeamCityProject.FromJson(pMsg);
                    return pData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to retrieve project data for Project: {0}", ProjectNameId);
                throw;
            }
        }

        public async Task<TeamCityBuilds> GetBuilds(string BuildType)
        {
            try
            {
                using (var client = GetClient())
                {
                    var buildsTask = client.GetStringAsync(_urls.GetBuilds(BuildType));
                    var bsMsg = await buildsTask;
                    var bsData = TeamCityBuilds.FromJson(bsMsg);
                    return bsData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to retrieve builds for Build Type: {0}", BuildType);
                throw;
            }
        }

        public async Task<TeamCityChanges> GetChanges(long BuildId)
        {
            try
            {
                using (var client = GetClient())
                {
                    var changesTask = client.GetStringAsync(_urls.GetChanges(BuildId));
                    var csMsg = await changesTask;
                    var csData = TeamCityChanges.FromJson(csMsg);

                    if (csData == null || csData.Change == null || csData.Change.Count == 0) return csData; // No changes therefore no need to get details

                    Parallel.ForEach<Change>(csData.Change, c =>
                    {
                        c.ChangeDetails = GetChange(c.Id).GetAwaiter().GetResult();
                    });

                    return csData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get Changes for Build ID: {0}", BuildId);
                throw;
            }
        }

        public async Task<TeamCityChange> GetChange(long ChangeId)
        {
            try
            {
                using (var client = GetClient())
                {
                    var changeTask = client.GetStringAsync(_urls.GetChange(ChangeId));
                    var cMsg = await changeTask;
                    var cData = TeamCityChange.FromJson(cMsg);
                    return cData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to retrieve Change Details for Change Id: {0}", ChangeId);
                throw;
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(String.Format("{0}:{1}", _userName, _password))));

            return client;
        }

        #region Configuration Methods
        private void SetAppConfiguration(IConfigurationManager _cMgr)
        {
            try
            {
                if (_cMgr == null)
                {
                    throw new Exception("Could not load App Configuration.");
                }

                _userName = _cMgr?.Configuration["BuildServerUserName"];
                _password = _cMgr?.Configuration["BuildServerPassword"];

                string serverAuth = _cMgr?.Configuration["BuildServerAuth"];

                if (String.IsNullOrWhiteSpace(serverAuth))
                    _serverAuth = "/httpAuth";
                else
                    _serverAuth = String.Format("/{0}", serverAuth.TrimStart('/'));

                if (_serverAuth.ToLower().Contains("httpauth") && (String.IsNullOrEmpty(_userName) || String.IsNullOrEmpty(_password)))
                {
                    throw new Exception("Basic Authentication requested for API but no username or password given.");
                }

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                throw ex;
            }
        }

        private void SetApiUrls(IConfigurationManager _cMgr, ApiConfigManager _apiMgr)
        {
            try
            {
                _urlBase = _cMgr?.Configuration["BuildServerUrlBase"];

                var fullUrl = _urlBase + _serverAuth; 

                if (String.IsNullOrWhiteSpace(_urlBase))
                    throw new Exception("Could not retrieve Build Server Url.");

                _urls = new ApiUrls(fullUrl, _logger, _apiMgr);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                throw ex;
            }
        }

        #endregion ConfigurationMethods

    }
}
