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

namespace DaybarWPF.View.Control
{
    /// <summary>
    /// Interaction logic for NowView.xaml
    /// </summary>
    public partial class NowView : UserControl
    {
        private NowViewModel _vm;

        public NowView()
        {
            InitializeComponent();
            this.DataContextChanged += NowView_DataContextChanged;

            VisualStateManager.GoToState(this, "JustRight", false);
        }

        private void NowView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _vm = DataContext as NowViewModel;

            _vm.PropertyChanged += _vm_PropertyChanged;
        }

        private void _vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsTooLate" || e.PropertyName == "IsTooEarly")
            {
                Dispatcher.Invoke(_tooLateEarlyToggle);

            }
        }

        void _tooLateEarlyToggle()
        {
            if (_vm.IsTooLate)
            {
                VisualStateManager.GoToState(this, "TooLate", true);
            }
            else if (_vm.IsTooEarly)
            {
                VisualStateManager.GoToState(this, "TooEarly", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "JustRight", true);
            }
        }
    }
}
