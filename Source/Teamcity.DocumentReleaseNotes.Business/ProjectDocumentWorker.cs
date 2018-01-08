using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Teamcity.DocumentReleaseNotes.Business;
using Teamcity.DocumentReleaseNotes.Interfaces;
using Teamcity.DocumentReleaseNotes.Models;
using Microsoft.Extensions.Logging;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public class ProjectDocumentWorker: IProjectDocumentWorker
    {
        IDocumentFormatter _formatter;
        string _documentSubFolder = "Documents";
        string _fileMode = "Overwrite";
        ILogger _logger;

        public ProjectDocumentWorker(IDocumentFormatter Formatter, IConfigurationManager AppMgr = null, ILogger<ProjectDocumentWorker> logger = null)
        {
            _fileMode = AppMgr?.Configuration["FileMode"];
            _formatter = Formatter;
            _logger = logger;
        }

        public void Write(TeamCityProject Project)
        {
            var di = CreateProjectFolder(Path.Combine(_documentSubFolder, FileHelper.GetSafeName(Project.Name)), _fileMode);

            WriteBuildTypes(Project, di, _formatter);
        }

        /// <summary>
        /// Create a New Project Folder or Use Existing Folder
        /// </summary>
        /// <param name="DirectoryName"></param>
        /// <param name="FileMode">Overwrite or CreateNew</param>
        /// <returns></returns>
        private DirectoryInfo CreateProjectFolder(string DirectoryName, string FileMode)
        {
            // Fix for Raneto not handling periods in directory names.
            DirectoryName = FileHelper.GetSafeDirectoryName(DirectoryName);

            DirectoryInfo info;
            if(!Directory.Exists(DirectoryName))
                info = System.IO.Directory.CreateDirectory(DirectoryName);
            else
            {
                if (FileMode.ToLower() != "overwrite")
                {

                    var i = 1;
                    var found = true;
                    string newDirectory = DirectoryName;
                    while (found == true)
                    {
                        newDirectory = String.Format("{0}_{1}", DirectoryName, i);
                        i++;
                        found = Directory.Exists(newDirectory);
                    }

                    info = System.IO.Directory.CreateDirectory(newDirectory);
                }
                else
                {
                    _logger?.LogInformation("Found Directory for Project: {0} and FileMode was {1}. Deleting all files.", DirectoryName, FileMode);
                    info = new DirectoryInfo(DirectoryName);
                    info.Empty();
                }
            }

            return info;
        }

        private void WriteBuildTypes(TeamCityProject Project, DirectoryInfo Directory, IDocumentFormatter Formatter)
        {
            Parallel.ForEach<BuildType>(Project.BuildTypes.BuildType, bt =>
            {
                WriteBuildType(bt, Directory, Formatter);
            });
        }

        private void WriteBuildType(BuildType bt, DirectoryInfo Directory, IDocumentFormatter Formatter)
        {
            // Create a factory for determining the DocumentFormatter;
            var docBuilder = new BuildTypeDocumentBuilder(Directory, bt, Formatter);
            docBuilder.Write();
        }
    }
}
