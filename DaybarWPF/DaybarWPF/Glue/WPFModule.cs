using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using DaybarWPF.Util;
using DayBar.Contract.Auth;
using DayBar.Contract.Repo;
using DayBar.Contract.UI;
using Office365Api.Helpers;
using Office365Api.Helpers.Impl;
using Office365Api.Helpers.Office365;
using XamlingCore.Portable.Contract.UI;

//using DayBar.UWP.Office365;

namespace DaybarWPF.Glue
{
    public class WPFModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof (WPFModule).Assembly)
                .Where(t => t.Name.EndsWith("View") || t.Name.EndsWith("ViewModel"))
                .AsSelf()
                .PropertiesAutowired();

            builder.RegisterType<UIUtils>().As<IUIUtils>();
            builder.RegisterType<AuthenticationHelper>().AsImplementedInterfaces().AsSelf().SingleInstance();
            builder.RegisterType<CachePersist>().As<ICachePersist>().SingleInstance();
            builder.RegisterType<OutlookCalendarRepo>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CalendarHelper>().AsSelf().SingleInstance();
            builder.RegisterType<DeviceNetworkStatus>().AsImplementedInterfaces();
            builder.RegisterType<TokenGetService>().AsImplementedInterfaces();

            builder.RegisterType<XDispatcher>().As<IDispatcher>().SingleInstance();

            //builder.RegisterType<UserOperations>().SingleInstance();
            //builder.RegisterType<OutlookTokenGetService>().As<ITokenGetService>().SingleInstance();
            //builder.RegisterType<OutlookCalendarRepo>().AsImplementedInterfaces().SingleInstance();
            //builder.RegisterType<StripWriter>().AsImplementedInterfaces().SingleInstance();


            base.Load(builder);
        }
    }
}