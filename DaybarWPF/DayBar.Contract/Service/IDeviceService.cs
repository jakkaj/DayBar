using System;
using System.Threading.Tasks;

namespace DayBar.Contract.Service
{
    public interface IDeviceService
    {
        void SetWindowHandle(IntPtr windowHandle);
        IntPtr WindowHandle { get; set; }
        double WindowWidth { get; set; }
        Task<double> TimeToXPos(DateTime time);
    }
}