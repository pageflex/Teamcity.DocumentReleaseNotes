using System;
using System.Collections.Generic;
using System.Text;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public static class ExtensionHelper
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return String.IsNullOrWhiteSpace(str);
        }
    }
}
