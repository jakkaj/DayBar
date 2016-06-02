//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DayBar.Contract.Auth;
//using DayBar.UWP.Office365;

//namespace DayBar.UWP.Impl
//{
//    public class OutlookTokenGetService : ITokenGetService
//    {
//        private readonly UserOperations _userOperations;

//        public OutlookTokenGetService(UserOperations userOperations)
//        {
//            _userOperations = userOperations;
//        }

//        public async Task<string> GetToken()
//        {
//            var _currentUser = await _userOperations.GetCurrentUserAsync();

//            if (_currentUser == null)
//            {
//                return null;
//            }

//            var _calendarCapability = ServiceCapabilities.Calendar.ToString();
//            var token = await AuthenticationHelper.GetTokenAsync(_calendarCapability);
//            var outlook = await AuthenticationHelper.GetOutlookClientAsync(_calendarCapability);
//            var tStart = DateTime.Today.ToUniversalTime();
//            var tEnd = DateTime.Today.AddHours(24).AddSeconds(-1).ToUniversalTime();

//            Debug.WriteLine($"Token: {token}");
//            Debug.WriteLine($"Url: {outlook.Context.BaseUri}");
//            Debug.WriteLine($"Start: {tStart.ToString("u")}");
//            Debug.WriteLine($"End: {tEnd.ToString("u")}");

//            return token;
//        }
//    }
//}
