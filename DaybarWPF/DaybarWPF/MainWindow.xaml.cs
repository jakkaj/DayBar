using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autofac;
using DaybarWPF.Model.Messages;
using DaybarWPF.Util;
using DaybarWPF.View;
using DayBar.Contract.Office;
using DayBar.Contract.Service;
using Office365Api.Helpers;
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
        public MainWindow()
        {
            InitializeComponent();
     
            this.Loaded += MainWindow_Loaded;
            _container = ContainerHost.Container;
            _deviceService = _container.Resolve<IDeviceService>();
            _userService = _container.Resolve<IUserService>();
            this.Register<LogoutAndShowMainMessage>(_reshowThis);
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _init();
        }

        async void _init()
        {
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
                this.Visibility = Visibility.Visible;
                this.Activate();
                var sb = this.Resources["UnFader"] as Storyboard;
                sb.Begin();
                
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

            var barWindow = _container.Resolve<BarView>();

            barWindow.Show();
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
    }
}
