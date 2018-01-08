using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Teamcity.DocumentReleaseNotes.Models
{
    public static class FileHelper
    {
        public static string GetSafeName(string name)
        {
            StringBuilder sb = new StringBuilder(name);

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                sb = sb.Replace(c, '-');
            }

            sb = sb.Replace('.', '-');

            return sb.ToString();
        }

        public static string GetSafeDirectoryName(string name)
        {
            StringBuilder sb = new StringBuilder(name);

            sb = sb.Replace('.', '-');

            return sb.ToString();
        }

        public static void Empty(this System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }
    }
}
