using System;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Interfaces
{
    public interface IDocumentReleaseNotesApiManager
    {
        List<string> GetConfiguredProjects();
    }
}
