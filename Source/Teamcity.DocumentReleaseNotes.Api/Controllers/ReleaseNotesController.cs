using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teamcity.DocumentReleaseNotes.Api.Domain;
using Teamcity.DocumentReleaseNotes.Business;
using Teamcity.DocumentReleaseNotes.Interfaces;
using Teamcity.DocumentReleaseNotes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Teamcity.DocumentReleaseNotes.Api.Controllers
{
    [Produces("application/json")]
    [Route("/api/[controller]")]
    public class ReleaseNotesController : Controller
    {
        private readonly IProjectService _ps;
        private readonly ITeamcityManager _mgr;
        private readonly ILogger _logger;
        public ReleaseNotesController(IProjectService ps, ITeamcityManager mgr, ILogger<ReleaseNotesController> logger = null)
        {
            _ps = ps ?? throw new ArgumentNullException(nameof(ps)); ;
            _mgr = mgr ?? throw new ArgumentNullException(nameof(mgr));
            _logger = logger;
        }
        [HttpGet("/api/ReleaseNotes/Create")]
        public List<TeamCityProject> CreateReleaseDocumentation()
        {
            var pData = _ps.DoProjectWork();

            return pData;
        }

        [HttpGet("/api/ReleaseNotes/GetConfigured")]
        public List<TeamCityProject> GetConfiguredReleaseNotes()
        {
            var pData = _ps.GetConfiguredProjectData();
            return pData;
        }

        [HttpGet("/api/ReleaseNotes/Project/{ProjectName}")]
        public TeamCityProject GetFullProjectReleaseNotes(string ProjectName)
        {
            return _mgr.GetProjectData(ProjectName).GetAwaiter().GetResult();
        }

        [HttpPost]
        public TeamCityBuilds GetBuildReleaseNotes([FromBody]ProjectBuildTypeDto ProjectInfo)
        {
            // Need to write a selector to get a specific build using the project_name, Build_Type_Name, and Build.Number 
            throw new NotImplementedException();
        }

        [HttpGet("/api/ReleaseNotes/BuildType/{BuildType}")]
        public TeamCityBuilds GetBuildTypeReleaseNotes(string BuildType)
        {
            return _mgr.GetBuilds(BuildType).GetAwaiter().GetResult();
        }
    }
}