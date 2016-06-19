using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Network;
using XamlingCore.Portable.Model.Network;

namespace Office365Api.Helpers.Impl
{
    public class DeviceNetworkStatus : IDeviceNetworkStatus
    {
        public bool QuickNetworkCheck()
        {
            return true;
        }

        public XNetworkType NetworkCheck()
        {
            return XNetworkType.WiFi;
        }

        public event EventHandler NetworkChanged;
    }
}
