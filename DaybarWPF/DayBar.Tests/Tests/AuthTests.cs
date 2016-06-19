using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayBar.Contract.Auth;
using DayBar.Tests.Glue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DayBar.Tests.Tests
{
    [TestClass]
    public class AuthTests : TestBase
    {
        [TestMethod]
        public async Task GetToken()
        {
            Auth();

            var tokenGet = Resolve<ITokenGetService>();

            var token = await tokenGet.GetToken();

            Assert.IsNotNull(token);
        }
    }
}
