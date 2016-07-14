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

        private string _attendeesText;
        private string _createdText;

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
            var startedButEndsInMin = _entry.End.Subtract(now);
            var lengthMins = _entry.End.Subtract(_entry.Start);

            if (_entry.End < now)
            {
                InMinutes = $"Ended {TimeUtils.DoMintesAgo(endInMin.TotalMinutes)}";
            }
            else if (_entry.Start < now && _entry.End > now)
            {
                InMinutes = $"On Now - ends in {TimeUtils.DoMintesTo(startedButEndsInMin.TotalMinutes)}";
            }
            else
            {
                InMinutes = $"Starts in {TimeUtils.DoMintesTo(inMin.TotalMinutes)}";
            }

            AtText = $"{_entry.Start.ToShortTimeString()} and runs for {TimeUtils.TimeSpanTo(lengthMins.TotalMinutes)}";


            var attList = new List<string>();

            CreatedText =
                $"{_entry.Organizer.EmailAddress.Name} ({_entry.Organizer.EmailAddress.Address})";

            if (_entry.Attendees != null)
            {
                foreach (var attendee in _entry.Attendees)
                {
                    //is this person me?
                    if (_entry.OdataId.ToLower().Contains(attendee.EmailAddress.Address.ToLower()))
                    {
                        continue;
                    }
                    attList.Add($"{attendee?.EmailAddress.Name} ({attendee.EmailAddress.Address})");
                }
            }

            AttendeesText = String.Join(", ", attList);
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

        public string AttendeesText
        {
            get { return _attendeesText; }
            set
            {
                _attendeesText = value;
                OnPropertyChanged();
            }
        }

        public string CreatedText
        {
            get { return _createdText; }
            set
            {
                _createdText = value; 
                OnPropertyChanged();
            }
        }
    }
}
