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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DaybarWPF.Model.Messages;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace DaybarWPF.View.Control
{
    /// <summary>
    /// Interaction logic for NowView.xaml
    /// </summary>
    public partial class NowView : UserControl
    {
        private NowViewModel _vm;
        private Storyboard _timeAttractor;
        private Storyboard _settingsOut;
        private Storyboard _settingsIn;
        public NowView()
        {
            InitializeComponent();
            this.DataContextChanged += NowView_DataContextChanged;
            this.MouseEnter += NowView_MouseEnter;
            this.MouseLeave += NowView_MouseLeave;
            _timeAttractor = this.Resources["EventActiveAttractor"] as Storyboard;
            _settingsOut = this.Resources["SettingsOut"] as Storyboard;
            _settingsIn = this.Resources["SettingsIn"] as Storyboard;
            VisualStateManager.GoToState(this, "JustRight", false);

            this.Register<EventAttractorMessage>(_onAttractor);
        }

        private void NowView_MouseLeave(object sender, MouseEventArgs e)
        {
            _settingsIn.BeginTime = TimeSpan.Zero;
            _settingsIn.Begin();

        }

        private void NowView_MouseEnter(object sender, MouseEventArgs e)
        {
            _settingsOut.BeginTime = TimeSpan.Zero;
            _settingsOut.Begin();
        }

        void _onAttractor()
        {
            Dispatcher.Invoke(() =>
            {
                _timeAttractor.BeginTime = TimeSpan.Zero;
                _timeAttractor.Begin();

            });
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

        private void TextBlockRefresh_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            new RefreshCalendarMessage().Send();
        }

        private void TextBlockSettings_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            new ShowSettingsMessage().Send();
        }

        private void TextBlockTomorrow_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            new ShowTomorrowMessage().Send();
        }
    }
}
