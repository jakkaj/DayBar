using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Autofac;
using DaybarWPF.Model.Messages;
using DaybarWPF.Util;
using DayBar.Contract.Service;
using DayBar.Contract.UI;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace DaybarWPF.View
{
    /// <summary>
    /// Interaction logic for BarView.xaml
    /// </summary>
    public partial class BarView : Window
    {
        private BarViewModel _vm;
        private readonly IDeviceService _deviceService;
        private readonly IUIUtils _uiUtils;
        private Storyboard _timeEnter;
        private Storyboard _timeLeave;

        public BarView(BarViewModel vm,
            IDeviceService deviceService, IUIUtils uiUtils)
        {
            InitializeComponent();


            _timeEnter = this.Resources["TimeLengedFadeIn"] as Storyboard;
            _timeLeave = this.Resources["TimeLegendFadeOut"] as Storyboard;

            this._vm = vm;
            _deviceService = deviceService;
            _uiUtils = uiUtils;
            this.DataContext = vm;
            vm.MyDispatcher = Dispatcher.CurrentDispatcher;
            vm.PropertyChanged += Vm_PropertyChanged;

            this.MouseDoubleClick += BarView_MouseDoubleClick;
            this.MouseRightButtonUp += BarView_MouseRightButtonUp;
            this.MouseUp += BarView_MouseUp;
            this.Closed += BarView_Closed;
            this.Loaded += BarView_Loaded;
            this.MouseEnter += BarView_MouseEnter;
            this.MouseLeave += BarView_MouseLeave;
        }

        private void BarView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _logoutAndShowMain();
        }

        private void BarView_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _timeEnter.Stop();
            _timeLeave.BeginTime = TimeSpan.Zero;
            _timeLeave.Begin();
            _vm.MouseLeave();
        }

        private void BarView_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _timeLeave.Stop();
            _timeEnter.BeginTime = TimeSpan.Zero;
            _timeEnter.Begin();
        }

        private void BarView_Loaded(object sender, RoutedEventArgs e)
        {
            _position();
        }

        private void BarView_Closed(object sender, EventArgs e)
        {
            _vm.Dispose();
            _vm = null;
        }

        private void BarView_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        void _logoutAndShowMain()
        {
            _uiUtils.LogoutAndShowHome();
            this.Close();

        }

        private void BarView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _vm.RefreshCalendar(true);
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoading")
            {
                _loadCheck();
            }
            if (e.PropertyName == "IsTomorrow")
            {
                Dispatcher.Invoke(_tomorrowCheck);
            }
        }

        void _tomorrowCheck()
        {
            if (_vm.IsTomorrow)
            {
                border.Visibility = Visibility.Visible;
            }
            else
            {
                border.Visibility = Visibility.Collapsed;
            }
        }

        void _loadCheck()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(_doLoad));
        }

        void _doLoad()
        {
            if (_vm == null)
            {
                return;
            }
            var sb = this.Resources["LoadChaser"] as Storyboard;

            if (_vm.IsLoading)
            {
                sb.BeginTime = TimeSpan.Zero;
                rectangle.Visibility = Visibility.Visible;
                sb.Begin();
            }
            else
            {
                sb.Stop();
                rectangle.Visibility = Visibility.Collapsed;
            }
        }



        void _position()
        {
            foreach (Screen s in Screen.AllScreens)
            {
                if (!s.Primary)
                {
                    continue;
                }

                var tb = s.Bounds;
                this.Left = 0;
                this.Top = 0;
                this.Width = 0;
                this.Height = 0;

                var pLeft = this.PointFromScreen(new Point(tb.Left, tb.Top));
                var pHeight = this.PointFromScreen(new Point(tb.Width, tb.Height));

                this.Left = 0;
                this.Top = 0;
                this.Height = 30;
                this.Width = pHeight.X;

                ((EasingDoubleKeyFrame)Resources["myEasingKey"]).Value = this.Width - 20;
                _deviceService.WindowWidth = this.Width;
                _vm.Init();

            }

            //var tb = WindowsTaskbar.TaskbarPostion;
            //this.Left = 0;
            //this.Top = 0;
            //this.Width = 0;
            //this.Height = 0;

            //var pLeft = this.PointFromScreen(new Point(tb.Left, tb.Top));
            //var pHeight = this.PointFromScreen(new Point(tb.Width, tb.Height));

            //this.Left = pLeft.X;
            //this.Top = pLeft.Y - 5;
            //this.Height = 2;
            //this.Width = pHeight.X;
            //var t = tb;
        }

        private void BarView_OnLoaded(object sender, RoutedEventArgs e)
        {

            WindowInteropHelper wndHelper = new WindowInteropHelper(this);

            int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

            exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);

        }

        #region Window styles
        [Flags]
        public enum ExtendedWindowStyles
        {
            // ...
            WS_EX_TOOLWINDOW = 0x00000080,
            // ...
        }

        public enum GetWindowLongFields
        {
            // ...
            GWL_EXSTYLE = (-20),
            // ...
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);
        #endregion
    }
}
