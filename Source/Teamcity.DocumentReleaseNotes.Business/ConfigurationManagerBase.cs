using Teamcity.DocumentReleaseNotes.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public abstract class ConfigurationManagerBase : IConfigurationManager
    {
        public IConfigurationRoot Configuration { get; set; }
        protected readonly ILogger _logger;

        public ConfigurationManagerBase(string fileName, ILogger logger = null)
        {
            _logger = logger;
            SetConfiguration(fileName);
        }

        protected void SetConfiguration(string fileName)
        {
            try
            {
                var builder = new ConfigurationBuilder().AddJsonFile(fileName, optional: false, reloadOnChange: true);
                var _configuration = builder.Build();
                Configuration = _configuration;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error retrieving {0} Configuration.", fileName);
            }
        }
    }
}
