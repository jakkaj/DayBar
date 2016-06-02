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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DaybarWPF.Util;

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
            _position();
        }

        void _position()
        {
            var tb = WindowsTaskbar.TaskbarPostion;
            this.Left = 0;
            this.Top = 0;
            this.Width = 0;
            this.Height = 0;

            var pLeft = this.PointFromScreen(new Point(tb.Left, tb.Top));
            var pHeight = this.PointFromScreen(new Point(tb.Width, tb.Height));
            
            this.Left = pLeft.X;
            this.Top = pLeft.Y - 5;
            this.Height = 2;
            this.Width = pHeight.X;
            var t = tb;
        }
    }
}
