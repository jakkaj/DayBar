using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Timers;
using DayBar.Contract.Service;
using XamlingCore.Portable.View.ViewModel;

namespace DaybarWPF.View.Control
{
    public class NowViewModel : XViewModel
    {
        private readonly IDeviceService _deviceService;
        private double _offset;

        private bool _isTooLate;
        private bool _isTooEarly;

        public NowViewModel(IDeviceService deviceService)
        {
            _deviceService = deviceService;
            _setOffset();
            var t = new Timer();
            t.Interval = 3000;
            t.Elapsed += T_Elapsed;
            t.Start();
        }

        private async void T_Elapsed(object sender, ElapsedEventArgs e)
        {
           _setOffset();
        }

        async void _setOffset()
        {
            Offset = await _deviceService.TimeToXPos(DateTime.Now);

            if (Offset > _deviceService.WindowWidth)
            {
                Offset = _deviceService.WindowWidth - 15;
                this.IsTooLate = true;
                this.IsTooEarly = false;
            }
            else if (Offset < 0)
            {
                this.IsTooEarly = true;
                this.IsTooLate = false;
            }
            else
            {
                this.IsTooLate = false;
                this.IsTooEarly = false;
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

        public bool IsTooLate
        {
            get { return _isTooLate; }
            set
            {
                _isTooLate = value;
                OnPropertyChanged();
            }
        }

        public bool IsTooEarly
        {
            get { return _isTooEarly; }
            set
            {
                _isTooEarly = value;
                OnPropertyChanged();
            }
        }
    }
}
