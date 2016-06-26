using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DaybarWPF.Util
{
    public static class ColorUtils
    {
        public static SolidColorBrush CatToColor(string cName)
        {
            SolidColorBrush brush = null;
            cName = cName.ToLower();
            if (cName.IndexOf("green") != -1)
            {
                brush = new SolidColorBrush(Colors.ForestGreen);
            }
            if (cName.IndexOf("blue") != -1)
            {
                brush = new SolidColorBrush(Colors.DodgerBlue);
            }
            if (cName.IndexOf("orange") != -1)
            {
                brush = new SolidColorBrush(Colors.Orange);
            }
            if (cName.IndexOf("purple") != -1)
            {
                brush = new SolidColorBrush(Colors.Purple);

            }
            if (cName.IndexOf("red") != -1)
            {
                brush = new SolidColorBrush(Colors.Red);
            }
            if (cName.IndexOf("yellow") != -1)
            {
                brush = new SolidColorBrush(Colors.Yellow);

            }

            if (brush == null)
            {
                brush = new SolidColorBrush(Colors.DodgerBlue);
            }

            return brush;
        }
    }
}
