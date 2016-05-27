using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DaybarWPF.Util
{


    //this is from https://winsharp93.wordpress.com/2009/06/29/find-out-size-and-position-of-the-taskbar/
    
        public sealed class WindowsTaskbar
        {
            private const string ClassName = "Shell_TrayWnd";

            public static Rectangle TaskbarPostion
            {
                get
                {
                    IntPtr taskbarHandle = User32.FindWindow(WindowsTaskbar.ClassName, null);

                    RECT r = new RECT();
                    User32.GetWindowRect(taskbarHandle, ref r);

                    return Rectangle.FromLTRB(r.left, r.top, r.right, r.bottom);
                }
            }
        }

        public static class User32
        {
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
    }

