using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using DaybarWPF.Impl;
using DayBar.Contract.Auth;
using DayBar.Contract.Repo;
using DayBar.UWP.Impl;
using Office365Api.Helpers;

//using DayBar.UWP.Office365;

namespace DaybarWPF.Glue
{
    public class WPFModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticationHelper>().AsSelf().SingleInstance();
            builder.RegisterType<CachePersist>().As<ICachePersist>().SingleInstance();

            //builder.RegisterType<UserOperations>().SingleInstance();
            //builder.RegisterType<OutlookTokenGetService>().As<ITokenGetService>().SingleInstance();
            //builder.RegisterType<OutlookCalendarRepo>().AsImplementedInterfaces().SingleInstance();
            //builder.RegisterType<StripWriter>().AsImplementedInterfaces().SingleInstance();


            base.Load(builder);
        }
    }
}