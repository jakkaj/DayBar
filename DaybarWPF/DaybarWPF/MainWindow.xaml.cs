﻿using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autofac;
using DaybarWPF.Util;
using DaybarWPF.View;
using DayBar.Contract.Office;
using DayBar.Contract.Service;
using Office365Api.Helpers;
using XamlingCore.Portable.Data.Glue;

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

            var authResult = await _userService.EnsureLoggedIn(force);

            if (!authResult)
            {
                Debug.WriteLine("Not logged in");
                //show login options
            }
            else
            {
                Debug.WriteLine("Logged in");
                //load the system.
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _showLogin(false);
        }

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            _showLogin(true);
        }
    }
}
