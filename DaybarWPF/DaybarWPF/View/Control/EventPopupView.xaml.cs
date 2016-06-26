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
using System.Windows.Shapes;
using DayBar.Entity.Calendars;

namespace DaybarWPF.View.Control
{
    /// <summary>
    /// Interaction logic for EventPopupView.xaml
    /// </summary>
    public partial class EventPopupView : Window
    {
        private EventPopupViewModel _vm;
        private CalendarEntry _entry;
        public EventPopupView(EventPopupViewModel vm)
        {
            _vm = vm;
            DataContext = _vm;
            
            InitializeComponent();

            this.Closed += EventPopupView_Closed;
            this.Loaded += EventPopupView_Loaded;
        }

        private void EventPopupView_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void EventPopupView_Closed(object sender, EventArgs e)
        {
            _vm.Dispose();
            _vm = null;
        }

        public void SetEvent(CalendarEntry entry, double offset)
        {
            if (_vm == null)
            {
                return;
            }

            _entry = entry;
            _vm.Entry = _entry;

            var pos = PointFromScreen(new Point(offset, 0));

            this.Left = offset;
            this.Top = 15;

            var sb = this.Resources["Fader"] as Storyboard;

            sb.Begin();
        }
    }
}
