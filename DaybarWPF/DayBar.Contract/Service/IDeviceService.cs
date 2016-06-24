using System;

namespace DayBar.Contract.Service
{
    public interface IDeviceService
    {
        void SetWindowHandle(IntPtr windowHandle);
        IntPtr WindowHandle { get; set; }
    }
}