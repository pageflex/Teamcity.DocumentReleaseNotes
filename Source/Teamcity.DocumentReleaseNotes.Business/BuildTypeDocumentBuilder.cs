using Teamcity.DocumentReleaseNotes.Interfaces;
using Teamcity.DocumentReleaseNotes.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public class BuildTypeDocumentBuilder
    {
        BuildType _bType;
        IDocumentFormatter _formatter;
        DirectoryInfo _directory;
        string _filePath;

        public BuildTypeDocumentBuilder(DirectoryInfo Directory, BuildType BType, IDocumentFormatter formatter)
        {
            _bType = BType;
            _directory = Directory;
            _formatter = formatter;
            AddSortOrder(_bType);
            _filePath = Path.Combine(Directory.FullName, String.Format("{0}.{1}", FileHelper.GetSafeName(_bType.Name), _formatter.FileSuffix ));
        }

        public void Write()
        {
            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
            {
                if (!String.IsNullOrWhiteSpace(_formatter.StartDocument()))
                    writer.WriteLine(_formatter.StartDocument());

                var title = String.Format("{0} for Project {1}", _bType.Name, _bType.ProjectName);
                writer.WriteLine(_formatter.Header1(title));

                writer.Flush();

                List<ContentInfo> builds = BuildsWithQuickSort();

                foreach (var si in builds)
                {
                    writer.WriteLine(si.Content);
                }

                if (!String.IsNullOrWhiteSpace(_formatter.EndDocument()))
                    writer.WriteLine(_formatter.EndDocument());

                writer.Flush();

                // Write to file from Stream.
                //stream.Seek(0, SeekOrigin.Begin);
                using (FileStream fs = new FileStream(_filePath, FileMode.OpenOrCreate))
                {
                    stream.WriteTo(fs);
                    fs.Flush();
                    fs.Close();
                }
            }

        }

        private static void AddSortOrder(BuildType bType)
        {
            int i = 1;
            foreach (var build in bType?.Builds)
            {
                build.SortOrder = i;
                i++;
            }
        }


        private List<ContentInfo> BuildsWithQuickSort()
        {
            BlockingCollection<ContentInfo> builds = new BlockingCollection<ContentInfo>();

            Parallel.ForEach<Build>(_bType.Builds, b => {
                builds.Add(CreateBuildContent(b, _formatter));
            });

            List<ContentInfo> listInfo = ContentInfo.QuickSortById(new List<ContentInfo>(builds));

            return listInfo;
        }

        private static ContentInfo CreateBuildContent(Build build, IDocumentFormatter Formatter)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb)) //(stream, Encoding.UTF8))
            {
                writer.WriteLine(Formatter.Header2(Formatter.Url(build.WebUrl, build.Number)));
                writer.WriteLine(Formatter.StartList());

                if (build.Changes == null)
                {
                    writer.WriteLine(Formatter.ListItem("No Changes recorded for build."));
                }
                else
                {
                    foreach (var c in build.Changes)
                    {
                        writer.WriteLine(Formatter.ListItem(String.Format("{0:MM/dd/yyyy H:mm:ss zzz}", c.Date.ConvertToDateTime()) + ": " + c.ChangeDetails?.Comment.TrimEnd(writer.NewLine.ToCharArray()) + " (" + c.ChangeDetails?.Username + ")"));
                    }
                }

                writer.WriteLine(Formatter.EndList());
                writer.Flush();

                var si = new ContentInfo()
                {
                    Content = sb.ToString(),
                    SortOrder = build.SortOrder,
                    Id = build.Id
                };

                return si;
            }
        }
    }


}