using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Models
{
    public partial class Build
    {
        public IEnumerable<Change> Changes { get; set; }
        public int SortOrder { get; set; }
    }
}
