using Teamcity.DocumentReleaseNotes.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Teamcity.DocumentReleaseNotes.Interfaces
{
    public interface IProjectWorker
    {
        List<TeamCityProject> LoadProjects(CancellationToken token);
        List<TeamCityProject> LoadProjectsInParallel(CancellationToken token);
        TeamCityProject LoadProject(string ProjectName, CancellationToken token);
    }
}
