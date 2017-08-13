using RandomUtils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Parameterization
{
    internal class DefaultValues
    {
        internal const int MAXINTVALUE = 2147483647;
        internal const int DEFAULTMAXIMUM = 255;
        internal const int DEFAULTMINIMUM = 1;
        internal const int DEFAULTMAXIMUMLENGTH = 10;
        internal const int DEFAULTMINIMUMLENGTH = 1;
        internal const double DEFAULTSEEDCLEARINGTHRESHOLD = 0.5;
        internal const StringType DEFAULTSTRINGTYPE = StringType.AlphaNumeric;
        internal static DateTime DEFAULTEARLIESTDATETIME = new DateTime(DateTime.Today.Year, 1, 1, 0, 0, 0);
        internal static DateTime DEFAULTLATESTDATETIME = new DateTime(DateTime.Today.Year + 1, 1, 1, 0, 0, 0);
    }
}
