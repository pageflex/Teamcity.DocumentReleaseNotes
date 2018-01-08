using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Models
{
    public partial class TeamCityProject
    {
        public BlockingCollection<BuildType> ParallelBuildTypes { get; set; }
    }
}
