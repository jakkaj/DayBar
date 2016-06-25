using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Office365Api.Helpers;
using Office365Api.Helpers.Office365;
using XamlingCore.Portable.Data.Glue;

namespace DayBar.Tests.Glue
{
    public class TestBase
    {
        public ProjectGlue Glue { get; set; }
        protected IContainer Container;
        public TestBase()
        {
            Glue = new ProjectGlue();
            Glue.Init();
            Container = Glue.Container;
        }

        public AuthenticationHelper Auth()
        {
            var authenticationHelper = ContainerHost.Container.Resolve<AuthenticationHelper>();
            authenticationHelper.EnsureAuthenticationContext("https://login.windows.net/Common/");
            var t = authenticationHelper.AuthenticationContext;
            var a = authenticationHelper.AuthenticationResult;
            
            return authenticationHelper;
        }

        public T Resolve<T>() where T : class
        {
            return Container.Resolve<T>();
        }
    }
}
