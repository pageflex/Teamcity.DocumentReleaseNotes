using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public class ApiConfigManager: ConfigurationManagerBase
    {
        public ApiConfigManager(ILogger<ApiConfigManager> logger = null) : base("TeamcityApi.json", logger)
        { }
    }
}
