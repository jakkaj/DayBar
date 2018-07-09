using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autofac;
using DaybarWPF.Glue;
using DaybarWPF.Model.Messages;
using DaybarWPF.Util;
using DaybarWPF.View;
using DayBar.Contract.Office;
using DayBar.Contract.Service;
using Office365Api.Helpers;
using Squirrel;
using XamlingCore.Portable.Contract.UI;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace DaybarWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILifetimeScope _container;
        private IDeviceService _deviceService;

        private IUserService _userService;


        private BarView _barWindow;

        public MainWindow()
        {
            InitializeComponent();
     
            this.Loaded += MainWindow_Loaded;
            _container = ContainerHost.Container;
            _deviceService = _container.Resolve<IDeviceService>();
            _userService = _container.Resolve<IUserService>();
            this.Register<LogoutAndShowMainMessage>(_reshowThis);

            XDispatcher.Dispatcher = Dispatcher;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _init();
        }

        async void _init()
        {

#if DEBUG == false
            try
            {
                using (var mgr = new UpdateManager(@"https://daybar.blob.core.windows.net/app/install"))
                {
                    await mgr.UpdateApp();
                }

            }
            catch  { }
          
#endif
            _deviceService.SetWindowHandle(new WindowInteropHelper(Application.Current.MainWindow).Handle);
            await Task.Delay(1000);
            _showLogin(false);
        }

      

        async void _showLogin(bool force)
        {
            var sb = this.Resources["CalendarPop"] as Storyboard;
            sb.Begin();
            try
            {
                var authResult = await _userService.EnsureLoggedIn(force);

                if (!authResult)
                {
                    Debug.WriteLine("Not logged in");
                    _showLogin();
                    //show login options
                }
                else
                {
                    Debug.WriteLine("Logged in");
                    _showBar();
                    //load the system.
                }
                sb.Pause();
                sb.BeginTime = TimeSpan.Zero;
            }
            catch
            {
                _showLogin();
            }
            
        }

        void _reshowThis()
        {
            Dispatcher.Invoke(() =>
            {
                if (_barWindow != null)
                {
                    _barWindow.Close();
                    _barWindow = null;
                } 

                this.Visibility = Visibility.Visible;
                this.Activate();
                var sb = this.Resources["UnFader"] as Storyboard;
                sb.Begin();
                _showLogin();

            });

        }
        void _showLogin()
        {
            LoginButton.Visibility= Visibility.Visible;
        }

        async void _showBar()
        {
            await Task.Delay(1000);
            var sb = this.Resources["Fader"] as Storyboard;
            sb.Begin();

            sb.Completed += Sb_Completed;

            _barWindow = _container.Resolve<BarView>();

            _barWindow.Show();
        }

        private void Sb_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _showLogin(false);
        }

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            _showLogin(true);
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
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
