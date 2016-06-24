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
        public IntPtr WindowHandle { get; set; }

        public void SetWindowHandle(IntPtr windowHandle)
        {
            this.WindowHandle = windowHandle;
        }
    }
}
