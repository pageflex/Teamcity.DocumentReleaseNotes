using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teamcity.DocumentReleaseNotes.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Teamcity.DocumentReleaseNotes.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Settings")]
    public class SettingsController : Controller
    {
        private readonly IConfigurationManager _appSettings;
        private readonly ILogger _logger;

        public SettingsController(IConfigurationManager cMgr = null, ILogger<SettingsController> logger = null)
        {
            _appSettings = cMgr;
            _logger = logger;
        }

        [HttpGet("/api/Settings/GetConfiguredProjects")]
        public List<string> GetConfiguredProjects()
        {
            var projects =  _appSettings?.Configuration.GetSection("Projects").GetChildren().Select(x => x.Value).ToList<string>();
            return projects;
        }
    }
}