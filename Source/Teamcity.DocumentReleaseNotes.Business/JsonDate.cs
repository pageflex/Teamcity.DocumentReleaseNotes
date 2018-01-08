using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Teamcity.DocumentReleaseNotes.Business
{
    public static class JsonDate
    {
        public static DateTime ConvertToDateTime(this string TeamcityDate)
        {
            DateTime dt = DateTime.ParseExact
                        (TeamcityDate,
                         "yyyyMMdd'T'HHmmsszzz",
                         CultureInfo.InvariantCulture);

            return dt;
        }
    }
}
