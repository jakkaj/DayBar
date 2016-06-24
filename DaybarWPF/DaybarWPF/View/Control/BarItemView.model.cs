using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DayBar.Contract.Service;
using DayBar.Entity.Calendars;
using XamlingCore.Portable.View.ViewModel;

namespace DaybarWPF.View.Control
{
    public class BarItemViewModel : XViewModel
    {
        private readonly IDeviceService _deviceService;
        public CalendarEntry _entry;

        private double _offset;
        private double _width;

        public BarItemViewModel(IDeviceService deviceService)
        {
            _deviceService = deviceService;
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
    }
}
