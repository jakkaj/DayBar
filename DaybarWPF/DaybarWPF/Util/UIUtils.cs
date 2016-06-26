using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DaybarWPF.Model.Messages;
using DayBar.Contract.Repo;
using DayBar.Contract.UI;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace DaybarWPF.Util
{
    public class UIUtils : IUIUtils
    {
        private readonly ICachePersist _cachePersist;

        public UIUtils(ICachePersist cachePersist)
        {
            _cachePersist = cachePersist;
        }

        public void LogoutAndShowHome()
        {
            _cachePersist.Clear();
            new LogoutAndShowMainMessage().Send();
        }

        
    }
}
