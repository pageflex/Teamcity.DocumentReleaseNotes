using Microsoft.Extensions.Configuration;

namespace Teamcity.DocumentReleaseNotes.Interfaces
{
    public interface IConfigurationManager
    {
        IConfigurationRoot Configuration { get; set; }
    }
}