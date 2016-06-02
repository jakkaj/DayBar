//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Autofac;
//using DayBar.Contract.Repo;
//using DayBar.Entity.Calendars;
//using DayBar.UWP.Office365;
//using XamlingCore.Portable.Contract.Downloaders;
//using XamlingCore.Portable.Contract.Serialise;
//using XamlingCore.Portable.Data.Glue;
//using XamlingCore.Portable.Data.Repos.Base;
//using XamlingCore.Portable.Model.Response;

//namespace DayBar.UWP.Impl
//{
//    public class OutlookCalendarRepo : OperationWebRepo<CalendarEntry>, ICalendarRepo
//    {
//        private readonly UserOperations _userOperations;
//        private readonly IEntitySerialiser _entitySerialiser;

//        public OutlookCalendarRepo(UserOperations userOperations, IHttpTransferrer downloader, IEntitySerialiser entitySerialiser)
//            : base(downloader, entitySerialiser, "")
//        {
//            _userOperations = userOperations;
//            _entitySerialiser = entitySerialiser;
//        }

//        public async Task<XResult<List<CalendarEntry>>> GetRange(DateTime start, DateTime end)
//        {
//            var _currentUser = await _userOperations.GetCurrentUserAsync();

//            if (_currentUser == null)
//            {
//                return null;
//            }

//            var _calendarCapability = ServiceCapabilities.Calendar.ToString();
//            var token = await AuthenticationHelper.GetTokenAsync(_calendarCapability);
//            var outlook = await AuthenticationHelper.GetOutlookClientAsync(_calendarCapability);
//            var tStart = start.ToUniversalTime();
//            var tEnd = end.ToUniversalTime();

//            Debug.WriteLine($"Token: {token}");
//            Debug.WriteLine($"Url: {outlook.Context.BaseUri}");
//            Debug.WriteLine($"Start: {start.ToUniversalTime().ToString("u")}");
//            Debug.WriteLine($"End: {end.ToUniversalTime().ToString("u")}");


//            var t = ContainerHost.Container.Resolve<IHttpTransferrer>();

//            var url = outlook.Context.BaseUri.ToString();

//            url =
//                $"{url}/me/CalendarView?$top=30&&startDateTime={tStart.ToString("u")}&endDateTime={tEnd.ToString("u")}";

//            var result = await GetResult(url);


//            if (result == null || !result.IsSuccessCode)
//            {
//                return XResult<List<CalendarEntry>>.GetFailed();
//            }

//            var parsed = _entitySerialiser.Deserialise<OutlookResult>(result.Result);

//            if (parsed == null || parsed.value == null)
//            {
//                return XResult<List<CalendarEntry>>.GetFailed();
//            }

//            var resultList = new List<CalendarEntry>();

//            foreach (var thing in parsed.value)
//            {
//                var version = thing.Id + thing.Start + thing.End;
//                if (thing.Categories != null)
//                {
//                    version += string.Join(",", thing.Categories);
//                }

//                var cal = new CalendarEntry
//                {
//                    Start = DateTime.Parse(thing.Start),
//                    End = DateTime.Parse(thing.End),
//                    Subject = thing.Subject,
//                    Id = thing.Id,
//                    Type = thing.Type,
//                    Categories = thing.Categories,
//                    Version = version
//                };

//                if (cal.Start.Date != cal.End.Date)
//                {
//                    continue;
//                }
//                resultList.Add(cal);
//            }


//            return new XResult<List<CalendarEntry>>(resultList);
//        }


//        public async Task<XResult<List<CalendarEntry>>> GetToday()
//        {
//            return await GetRange(DateTime.Today, DateTime.Today.AddDays(1).AddMilliseconds(-1));
//        }

//        public async Task<XResult<List<CalendarEntry>>> GetTomorrow()
//        {
//            return await GetRange(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2).AddMilliseconds(-1));
//        }
//    }
//}
