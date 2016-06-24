using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using DayBar.Contract.Service;
using DayBar.Tests.Glue;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Office365Api.Helpers;
using XamlingCore.Portable.Data.Glue;

namespace DayBar.Tests.Tests
{

    [TestClass]
    public class CalendarTests : TestBase
    {

        [TestMethod]
        public async Task TestServiceMethod()
        {
            Auth();
            
            var calService = Resolve<ICalendarService>();

            var events = await calService.GetToday();

            Assert.IsNotNull(events);

            Debug.WriteLine($"{events.Object.Count()} events found");
        }

        [TestMethod]
        public async Task TestDirect()
        {
            var authenticationHelper = Auth();

            var c = new CalendarHelper(authenticationHelper);
           
            var events = await c.GetCalendarEvents();
            Assert.IsNotNull(events);
            Assert.IsFalse(!events.Any());
            Debug.WriteLine($"{events.Count()} events found");
        }
    }
}
