﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Models
{
    public partial class BuildType
    {
        public IEnumerable<Build> Builds { get; set; }
    }
}
