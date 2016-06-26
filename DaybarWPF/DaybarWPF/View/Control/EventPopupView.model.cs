using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;
using DaybarWPF.Util;
using DayBar.Entity.Calendars;
using XamlingCore.Portable.View.ViewModel;

namespace DaybarWPF.View.Control
{
    public class EventPopupViewModel : XViewModel
    {
        public CalendarEntry _entry;

        private SolidColorBrush _catColor;
        private string _inMinutes;
        private string _atText;
        private Timer _timer;
        public EventPopupViewModel()
        {
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += T_Elapsed;
            _timer.Start();
        }

        public override void Dispose()
        {
            _timer.Stop();
            _timer = null;
        }

        private async void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(_setProps);
        }

        void _setProps()
        {
            foreach (var cat in _entry.Categories)
            {
                CatColor = ColorUtils.CatToColor(cat);
            }

            var now = DateTime.Now;

            var inMin = _entry.Start.Subtract(now);
            var endInMin = now.Subtract(_entry.End);

            var lengthMins = _entry.End.Subtract(_entry.Start);

            if (_entry.End < now)
            {
                InMinutes = $"Ended {TimeUtils.DoMintesAgo(endInMin.TotalMinutes)}";
            }
            else if (_entry.Start < now && _entry.End > now)
            {
                InMinutes = $"On Now - ends {TimeUtils.DoMintesTo(endInMin.TotalMinutes)}";
            }
            else
            {
                InMinutes = $"Starts in {TimeUtils.DoMintesTo(inMin.TotalMinutes)}";
            }

            AtText = $"{_entry.Start.ToShortTimeString()} and runs for {TimeUtils.TimeSpanTo(lengthMins.TotalMinutes)}";


        }

        public CalendarEntry Entry
        {
            get { return _entry; }
            set
            {
                _entry = value;
                OnPropertyChanged();
                _setProps();
            }
        }

        public SolidColorBrush CatColor
        {
            get { return _catColor; }
            set
            {
                _catColor = value;
                OnPropertyChanged();
            }
        }

        public string InMinutes
        {
            get { return _inMinutes; }
            set
            {
                _inMinutes = value;
                OnPropertyChanged();
            }
        }

        public string AtText
        {
            get { return _atText; }
            set
            {
                _atText = value;
                OnPropertyChanged();
            }
        }
    }
}
