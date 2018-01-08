using System.Threading.Tasks;
using Teamcity.DocumentReleaseNotes.Models;
using Teamcity.DocumentReleaseNotes.Interfaces;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public interface ITeamcityManager
    {
        Task<TeamCityBuilds> GetBuilds(string BuildType);
        Task<TeamCityChange> GetChange(long ChangeId);
        Task<TeamCityChanges> GetChanges(long BuildId);
        Task<TeamCityProject> GetProjectData(string ProjectNameId);
    }
}