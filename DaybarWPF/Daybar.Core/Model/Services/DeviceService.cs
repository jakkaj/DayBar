using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayBar.Contract.Service;

namespace Daybar.Core.Model.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IBarConfigService _barConfigService;

        public DeviceService(IBarConfigService barConfigService)
        {
            _barConfigService = barConfigService;
        }

        public IntPtr WindowHandle { get; set; }
        public double WindowWidth { get; set; }

        public void SetWindowHandle(IntPtr windowHandle)
        {
            this.WindowHandle = windowHandle;
        }

        public async Task<double> TimeToXPos(DateTime time)
        {
            var config = await _barConfigService.GetCalendarConfig();

            //start hour is 0

            double hoursGap = config.EndHour - config.StartHour;

            double minutePixels = WindowWidth/(hoursGap * 60); 

            var startHour = new DateTime(time.Year, time.Month, time.Day, config.StartHour, 0, 0);

            var offsetMintes = time.Subtract(startHour).TotalMinutes;

            //var endHour = = new DateTime(time.Year, time.Month, time.Day, config.EndHour, 0, 0);

            return offsetMintes*minutePixels;

        }
    }
}
