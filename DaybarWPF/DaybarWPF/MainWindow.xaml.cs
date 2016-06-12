using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DaybarWPF.Util;
using DaybarWPF.View;
using Office365Api.Helpers;

namespace DaybarWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Activated += MainWindow_Activated;
            _top();
        }

        async void _top()
        {
            while (true)
            {
                await Task.Delay(500);
                this.Topmost = true;
            }
        }



        private void MainWindow_Activated(object sender, EventArgs e)
        {
            //_showLogin();
        }

        async void _showLogin()
        {
            await Task.Delay(1000);
            IntPtr windowHandle = new WindowInteropHelper(Application.Current.MainWindow).Handle;

            AuthenticationHelper authenticationHelper = new AuthenticationHelper();
            await authenticationHelper.EnsureAuthenticationContext("https://login.windows.net/Common/", windowHandle);

            var c = new CalendarHelper(authenticationHelper);

            var events = await c.GetCalendarEvents();
            var t = events;
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _showLogin();
        }
    }
}
