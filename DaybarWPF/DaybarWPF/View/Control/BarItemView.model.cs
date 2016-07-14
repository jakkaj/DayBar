using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DaybarWPF.Model.Messages;
using DaybarWPF.Util;
using DayBar.Contract.Service;
using DayBar.Entity.Calendars;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.View.ViewModel;

namespace DaybarWPF.View.Control
{
    public class BarItemViewModel : XViewModel
    {
        private readonly IDeviceService _deviceService;
        public CalendarEntry _entry;

        private double _offset;
        private double _width;

        private SolidColorBrush _brush;

        public BarItemViewModel(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        public void MouseIn()
        {
            new ShowEventPopupMessage(_entry, _offset).Send();
            Debug.WriteLine($"Mouse in: {_offset}");
        }

        public void MouseOut()
        {
            new HideEventPopupMessage().Send();
            Debug.WriteLine($"Mouse out: {_offset}");
        }

        void _setColor()
        {
            if (_entry.Categories == null || _entry.Categories.Count == 0)
            {
                Brush = new SolidColorBrush(Colors.DodgerBlue);
                return;
            }

            foreach (var cat in _entry.Categories)
            {
                Brush = ColorUtils.CatToColor(cat);
            }

           
        }

        async void _setWidth()
        {
            if (_entry == null)
            {
                return;
            }

            var s = await _deviceService.TimeToXPos(_entry.Start);
            var e = await _deviceService.TimeToXPos(_entry.End);

            Offset = s;
            Width = e - s;

            _setColor();
        }

        public CalendarEntry Entry
        {
            get { return _entry; }
            set
            {
                _entry = value;
                _setWidth();
            }
        }

        public double Offset
        {
            get { return _offset; }
            set
            {
                _offset = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush Brush
        {
            get { return _brush; }
            set
            {
                _brush = value;
                OnPropertyChanged();

            }
        }
    }
}
