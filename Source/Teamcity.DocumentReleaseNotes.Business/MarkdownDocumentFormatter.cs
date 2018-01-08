using Teamcity.DocumentReleaseNotes.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Business
{
    class MarkdownDocumentFormatter : IDocumentFormatter
    {
        public string FileSuffix
        {
            get { return "md"; }
        }
        int i = 1;
        public string EndDocument()
        {
            return string.Empty;
        }

        public string EndList()
        {
            i = 1;
            return string.Empty;
        }

        public string Header1(string str)
        {
            return String.Format("# {0}", str);
        }

        public string Header2(string str)
        {
            return String.Format("## {0}", str);
        }

        public string ListItem(string item)
        {
            return String.Format("* {0}", item);
        }

        public string OrderedListItem(string item)
        {
            return String.Format("{0}. {1}", i, item);
        }
        public string StartDocument()
        {
            return String.Empty;
        }

        public string StartList()
        {
            i = 1;
            return String.Empty;
        }

        public string Url(string URL, string Description)
        {
            return String.Format("[{0}]({1})", Description, URL);
        }

        public string Url(string URL)
        {
            return String.Format("<{0}>", URL);
        }


    }
}
