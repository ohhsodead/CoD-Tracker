using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoD_Tracker
{
    class StringUtilities
    {
        /// <summary>
        /// Return Number with Comma's for Thousands
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetFormattedNumber(string value)
        {
            return string.Format("{0:n0}", Convert.ToInt32(value));
        }

        public static string SecondsToTime(int value)
        {
            int seconds = value % 60;
            int totalMinutes = value / 60;
            int minutes = totalMinutes % 60;
            int totalHours = totalMinutes / 60;
            int hours = totalHours % 24;
            int totalDays = totalHours / 24;
            StringBuilder builder = new StringBuilder();

            if (totalDays > 0)
            {
                builder.Append(totalDays + " D ");
            }
            if (totalHours > 0)
            {
                builder.Append(hours + " H ");
            }
            if (totalMinutes > 0)
            {
                builder.Append(minutes + " M ");
            }
            if (totalMinutes == 0 && totalHours == 0 && totalDays == 0)
            {
                builder.Append(seconds + " S");
            }

            return builder.ToString();
        }
    }
}
