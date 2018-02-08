using Teamcity.DocumentReleaseNotes.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public class DocumentReleaseNotesApiManager : IDocumentReleaseNotesApiManager
    {
        private readonly ILogger _logger;
        private readonly IConfigurationManager _mgr;
        private readonly string _settingUrl = "/Settings/GetConfiguredProjects";
        private readonly string _configApiKey = "ConfiguredProjectsApiUrl";
        private readonly string _baseUrl = "http://Teamcity.documentreleasenotes.api/api/"; //Docker Compose Service URL

        public DocumentReleaseNotesApiManager(IConfigurationManager cMgr, ILogger logger = null)
        {
            try
            {
                _mgr = cMgr ?? throw new Exception("No configuration manager sent to the DocumentReleaseNotesApiManager.");
                var _baseUrl = _mgr.Configuration[_configApiKey] ?? throw new Exception("No base api url for DocumentReleaseNotesApiManager");
                _logger = logger;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error finding values needed in config. {0}", ex.Message);
                throw;
            }
        }
        public List<string> GetConfiguredProjects()
        {
            try
            {
                List<string> projects = new List<string>();
                using (var client = GetClient())
                {
                    var url = String.Format("{0}/{1}", _baseUrl.TrimEnd('/'), _settingUrl.TrimStart('/'));
                    _logger?.LogDebug("Configuration URL: {0}", url);
                    var response = client.GetAsync(url).GetAwaiter().GetResult();

                    response.EnsureSuccessStatusCode();

                    projects = response.ContentAsType<List<string>>();
                }

                return projects;
            }
            catch(Exception ex)
            {
                var errorMessage = "Unable to retrieve a list of configured projects in DocumentReleaseNotesApiManager. {0}";
                _logger?.LogError(ex, errorMessage, ex.Message);
                throw;
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();

            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(String.Format("{0}:{1}", _userName, _password))));

            return client;
        }
    }
}
