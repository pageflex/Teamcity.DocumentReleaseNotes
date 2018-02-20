using Teamcity.DocumentReleaseNotes.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public class ProjectListProvider : IProjectListProvider
    {
        private readonly IConfigurationManager _mgr;
        private readonly ILogger _logger;
        private static string _configPullFromApi = "PullProjectsFromApi";
        private static string _configLocalProjectKey = "Projects";
        private static IDocumentReleaseNotesApiManager _apiMgr;

        public ProjectListProvider(IConfigurationManager mgr, ILogger<ProjectListProvider> logger = null, IDocumentReleaseNotesApiManager apiMgr = null)
        {
            _mgr = mgr;
            _logger = logger;
            _apiMgr = apiMgr;
        }

        /// <summary>
        /// If ConfiguredProjectsApiUrl is null, it will try and use the local configuration
        /// </summary>
        /// <returns></returns>
        public List<string> GetConfigured()
        {
            List<string> projects = new List<string>();

            if (!GetProjectsFromApi(ref projects))
            {
                _logger.LogInformation("Logs not found in api, using local projects.");
                projects = _mgr?.Configuration.GetSection(_configLocalProjectKey).GetChildren().Select(x => x.Value).ToList<string>();
                _logger.LogInformation("Projects found in local config: {0}", string.Join(',', projects.ToArray()));
            }

            return projects;
        }

        private bool GetProjectsFromApi(ref List<string> projects)
        {
            var retval = false;
            try
            {
                if (_mgr.Configuration[_configPullFromApi] == "true")
                {
                    _logger.LogInformation("Config file says pull from api.");
                    //var apiMgr = new DocumentReleaseNotesApiManager(_mgr, _logger);
                    projects = _apiMgr.GetConfiguredProjects();
                    _logger.LogInformation("Projects found in web api: {0}", string.Join(',', projects.ToArray()));
                    retval = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetProjectsFromApi in ProjectListProvider. {0}", ex.Message);
            }

            return retval;
        }
    }
}
