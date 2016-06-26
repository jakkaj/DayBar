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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DaybarWPF.View.Control
{
    /// <summary>
    /// Interaction logic for BarItemView.xaml
    /// </summary>
    public partial class BarItemView : UserControl
    {

        public BarItemView()
        {
            InitializeComponent();
        }

       
        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var vm = DataContext as BarItemViewModel;
            vm?.MouseIn();
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var vm = DataContext as BarItemViewModel;
            vm?.MouseOut();
        }
    }
}
