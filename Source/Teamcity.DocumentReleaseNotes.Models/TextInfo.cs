using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Models
{
    public class ContentInfo
    {
        public string Content { get; set; }
        public int SortOrder { get; set; }
        public long Id { get; set; }

        public static List<ContentInfo> QuickSortById(List<ContentInfo> b)
        {
            List<ContentInfo> l;
            List<ContentInfo> h;

            if (b.Count == 0)
                return b;
            else
            {
                l = new List<ContentInfo>();
                h = new List<ContentInfo>();
                for (int i = 1; i < b.Count; i++)
                {
                    if (b[0].Id <= b[i].Id)
                        h.Add(b[i]);
                    else
                        l.Add(b[i]);
                }
            }

            l = QuickSortById(l);
            h = QuickSortById(h);

            l.Add(b[0]);
            return l.Concat(h).ToList();
        }

        public static List<ContentInfo> QuickSort(List<ContentInfo> b)
        {
            List<ContentInfo> l;
            List<ContentInfo> h;

            if (b.Count == 0)
                return b;
            else
            {
                l = new List<ContentInfo>();
                h = new List<ContentInfo>();
                for (int i = 1; i < b.Count; i++)
                {
                    if (b[0].SortOrder <= b[i].SortOrder)
                        h.Add(b[i]);
                    else
                        l.Add(b[i]);
                }
            }

            l = QuickSort(l);
            h = QuickSort(h);

            l.Add(b[0]);
            return l.Concat(h).ToList();

        }
    }
}
