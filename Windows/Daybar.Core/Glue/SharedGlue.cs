using System.Reflection;
using Autofac;
using Daybar.Core.Model.Config;
using Daybar.Core.Model.Services.Calendar;
using Daybar.Core.Model.Strip;
using DayBar.Contract.Strip;
using XamlingCore.Portable.Contract.Downloaders;

namespace Daybar.Core.Glue
{
    public static class SharedGlue
    {
        public static void Build(ContainerBuilder builder)
        {
            builder.RegisterType<TransferConfigService>().As<IHttpTransferConfigService>();
            builder.Register(_=>new Strip(StripSize.Size_144, _.Resolve<IStripWriter>())).AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(CalendarService).GetTypeInfo().Assembly)
               .Where((t => t.Name.EndsWith("Repo") || t.Name.EndsWith("Service")))
               .SingleInstance()
               .AsImplementedInterfaces();
        }
    }
}
