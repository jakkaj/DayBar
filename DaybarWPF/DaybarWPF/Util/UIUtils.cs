using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DaybarWPF.Model.Messages;
using DayBar.Contract.Office;
using DayBar.Contract.Repo;
using DayBar.Contract.UI;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace DaybarWPF.Util
{
    public class UIUtils : IUIUtils
    {
        private readonly ICachePersist _cachePersist;
        private readonly IAuthenticationHelper _authenticationHelper;

        public UIUtils(ICachePersist cachePersist, IAuthenticationHelper authenticationHelper)
        {
            _cachePersist = cachePersist;
            _authenticationHelper = authenticationHelper;
        }

        public void LogoutAndShowHome()
        {
            new HideEventPopupMessage().Send();
            _authenticationHelper.Clear();
            _cachePersist.Clear();
            
            new LogoutAndShowMainMessage().Send();
        }

        
    }
}
