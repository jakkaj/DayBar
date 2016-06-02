using Autofac;

namespace DaybarWPF.Glue
{
    public class WPFModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
           // builder.RegisterType<UserOperations>().SingleInstance();
           // builder.RegisterType<OutlookTokenGetService>().As<ITokenGetService>().SingleInstance();
          //  builder.RegisterType<OutlookCalendarRepo>().AsImplementedInterfaces().SingleInstance();
            //builder.RegisterType<StripWriter>().AsImplementedInterfaces().SingleInstance();


            base.Load(builder);
        }
    }
}