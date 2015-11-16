using Autofac;
using Daybar.Core.Glue;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Windows8.Glue;

namespace DayBar.UWP.Glue
{
    public class ProjectGlue : Windows8Glue
    {
        public override void Init()
        {
            base.Init();

            SharedGlue.Build(Builder);

            Builder.RegisterModule<UWPModule>();

            Container = Builder.Build();
            ContainerHost.Container = Container;
        }
    }
}
