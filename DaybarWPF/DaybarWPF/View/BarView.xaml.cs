using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Autofac;
using DaybarWPF.Util;
using DayBar.Contract.Service;

namespace DaybarWPF.View
{
    /// <summary>
    /// Interaction logic for BarView.xaml
    /// </summary>
    public partial class BarView : Window
    {
        private BarViewModel _vm;
        private readonly IDeviceService _deviceService;

        public BarView(BarViewModel vm, IDeviceService deviceService)
        {
            InitializeComponent();

            this.Activated += BarView_Activated;
            this._vm = vm;
            _deviceService = deviceService;
            this.DataContext = vm;

        }

        private void BarView_Activated(object sender, EventArgs e)
        {
            _position();
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
                this.Height = 3;
                this.Width = pHeight.X;

                
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
    }
}
