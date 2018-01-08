using System;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Interfaces
{
    public interface IDocumentFormatter
    {
        string StartDocument();
        string Header1(string str);
        string Header2(string str);
        string StartList();
        string EndList();
        string ListItem(string item);
        string OrderedListItem(string item);
        string EndDocument();
        string Url(string url, string description);
        string Url(string url);

        string FileSuffix { get; }
    }
}
