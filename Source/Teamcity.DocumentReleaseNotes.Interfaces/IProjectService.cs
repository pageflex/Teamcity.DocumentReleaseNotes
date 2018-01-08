using Autofac;
using Teamcity.DocumentReleaseNotes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Interfaces
{
    public interface IProjectService
    {
        List<TeamCityProject> DoProjectWork();
        List<TeamCityProject> GetConfiguredProjectData();
    }
}
