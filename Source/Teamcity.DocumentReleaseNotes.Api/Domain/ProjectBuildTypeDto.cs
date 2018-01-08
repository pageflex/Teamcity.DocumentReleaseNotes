using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teamcity.DocumentReleaseNotes.Api.Domain
{
    public class ProjectBuildTypeDto
    {
        public string ProjectName { get; set; }
        public string BuildType { get; set; }
        public int? BuildId { get; set; }
        public string BuildNumber { get; set; }
    }
}
