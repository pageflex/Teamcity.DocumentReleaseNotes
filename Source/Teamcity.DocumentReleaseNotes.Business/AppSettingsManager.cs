using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public class AppSettingsManager: ConfigurationManagerBase
    {
        public AppSettingsManager(ILogger<AppSettingsManager> logger = null): base("appsettings.json", logger)
        {
        }
    }
}
