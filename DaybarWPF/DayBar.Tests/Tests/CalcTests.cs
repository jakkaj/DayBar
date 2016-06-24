using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayBar.Contract.Service;
using DayBar.Tests.Glue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DayBar.Tests.Tests
{
    [TestClass]
    public class CalcTests : TestBase
    {
        [TestMethod]
        public async Task TestDateTimeOffsetCalcs()
        {
            var deviceService = Resolve<IDeviceService>();
            deviceService.WindowWidth = 3200;

            var eventStart = DateTime.Today.AddHours(8);

            var x = await deviceService.TimeToXPos(eventStart);

            Assert.IsTrue(x > 0);
        }
    }
}
