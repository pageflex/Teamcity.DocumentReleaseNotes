using Teamcity.DocumentReleaseNotes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Interfaces
{
    public interface IProjectDocumentWorker
    {
        void Write(TeamCityProject Project);
    }
}
