using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
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
        public async Task TestAuth()
        {
            var authenticationHelper = ContainerHost.Container.Resolve<AuthenticationHelper>();
            authenticationHelper.EnsureAuthenticationContext("https://login.windows.net/Common/");

            var c = new CalendarHelper(authenticationHelper);

            var events = await c.GetCalendarEvents();
            var t = events;
        }
    }
}
