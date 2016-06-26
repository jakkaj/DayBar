using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaybarWPF.Util
{
    public static class TimeUtils
    {
        public static string DoMintesAgo(double minutes)
        {
            var ago = "";
            if (minutes < 1)
            {
                ago = "just now";
            }
            else if (minutes < 2)
            {
                ago = "a minute ago";
            }
            else if (minutes < 60)
            {
                ago = Math.Floor(minutes) + " minutes ago";
            }
            else if (minutes < 120)
            {
                ago = "an hour ago";
            }
            else if (minutes / 60 < 24)
            {
                ago = Math.Floor(minutes / 60) + " hours ago";
            }
            else 
            {
                ago = "a day or more ago";
            }

            return ago;
        }

        public static string DoMintesTo(double minutes)
        {
            var ago = "";
            if (minutes < 1)
            {
                ago = "a minute";
            }
            else if (minutes < 2)
            {
                ago = "two minutes";
            }
            else if (minutes < 120)
            {
                ago = Math.Floor(minutes) + " minutes";
            }
            else if (minutes / 60 < 24)
            {
                ago = Math.Floor(minutes / 60) + " hours";
            }
            else
            {
                ago = "more than a day";
            }

            return ago;
        }

        public static string TimeSpanTo(double minutes)
        {
            var ago = "";
            if (minutes < 1)
            {
                ago = "a minute";
            }
            else if (minutes < 2)
            {
                ago = "two minutes";
            }
            else if (minutes < 120)
            {
                ago = Math.Floor(minutes) + " minutes";
            }
            else if (minutes / 60 < 24)
            {
                ago = Math.Floor(minutes / 60) + " hours";
            }
            else
            {
                ago = "more than a day";
            }

            return ago;
        }
    }
}
