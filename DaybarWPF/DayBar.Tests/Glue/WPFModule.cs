using Autofac;
using DayBar.Contract.Repo;
using Office365Api.Helpers;
using Office365Api.Helpers.Impl;
using Office365Api.Helpers.Office365;

//using DayBar.UWP.Office365;

namespace DayBar.Tests.Glue
{
    public class WPFModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticationHelper>().AsImplementedInterfaces().AsSelf().SingleInstance();
            builder.RegisterType<CachePersist>().As<ICachePersist>().SingleInstance();
            builder.RegisterType<OutlookCalendarRepo>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<CalendarHelper>().AsSelf().SingleInstance();
            builder.RegisterType<TokenGetService>().AsImplementedInterfaces();
            builder.RegisterType<DeviceNetworkStatus>().AsImplementedInterfaces();
            

            //builder.RegisterType<UserOperations>().SingleInstance();
            //builder.RegisterType<OutlookTokenGetService>().As<ITokenGetService>().SingleInstance();
            //builder.RegisterType<OutlookCalendarRepo>().AsImplementedInterfaces().SingleInstance();
            //builder.RegisterType<StripWriter>().AsImplementedInterfaces().SingleInstance();


            base.Load(builder);
        }
    }
}