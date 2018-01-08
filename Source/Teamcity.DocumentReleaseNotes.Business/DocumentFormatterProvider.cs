using Teamcity.DocumentReleaseNotes.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public class DocumentFormatterProvider : IDocumentFormatterProvider
    {
        ILogger _logger;
        AppSettingsManager settings;
        public DocumentFormatterProvider(ILogger<DocumentFormatterProvider> logger = null, AppSettingsManager cMgr = null)
        {
            _logger = logger;
            settings = cMgr;
        }

        /// <summary>
        /// Return back the configured Document Formatter
        /// </summary>
        /// <returns></returns>
        public IDocumentFormatter GetConfigured()
        {
            // TODO: In the future this might be found in the configuration
            return DocumentFormatterFactory.Create<MarkdownDocumentFormatter>();
        }
    }
}
