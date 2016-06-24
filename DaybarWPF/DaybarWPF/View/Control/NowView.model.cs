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
        public NowViewModel(IDeviceService deviceService)
        {
            _deviceService = deviceService;
            _setOffset();
            var t = new Timer();
            t.Interval = 1000;
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
    }
}
