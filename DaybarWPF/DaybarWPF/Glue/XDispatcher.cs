using System;
using System.Windows.Threading;
using XamlingCore.Portable.Contract.UI;

namespace DaybarWPF.Glue
{
    public class XDispatcher : IDispatcher
    {
        public static Dispatcher Dispatcher { get; set; }
        public void Invoke(Action action)
        {
            Dispatcher.Invoke(action);
        }
    }
}
