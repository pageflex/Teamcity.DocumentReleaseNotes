using Teamcity.DocumentReleaseNotes.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public static class DocumentFormatterFactory
    {
        public static T Create<T>() where T: IDocumentFormatter, new()
        {
            return new T();
        }
    }
}
