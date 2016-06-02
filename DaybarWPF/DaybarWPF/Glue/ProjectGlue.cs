using Autofac;
using Daybar.Core.Glue;
using XamlingCore.NET.Glue;
using XamlingCore.Portable.Data.Glue;

namespace DaybarWPF.Glue
{
    public class ProjectGlue : NETGlue
    {
        public override void Init()
        {
            base.Init();

            SharedGlue.Build(Builder);

            Builder.RegisterModule<WPFModule>();

            Container = Builder.Build();
            ContainerHost.Container = Container;
        }
    }
}
