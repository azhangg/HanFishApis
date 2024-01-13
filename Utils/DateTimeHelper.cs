using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class DateTimeHelper
    {
        public static string DataTimeString => DateTime.Now.ToString("F");

        public static string TimeString(DateTime time) => time.ToString("F");
    }
}
