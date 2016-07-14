using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac;
using DayBar.Contract.Office;
using DayBar.Contract.Repo;
using DayBar.Entity.Calendars;
using Office365Api.Helpers.Office365;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Serialise;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Portable.Data.Repos.Base;
using XamlingCore.Portable.Model.Response;

namespace Office365Api.Helpers.Impl
{
    public class OutlookCalendarRepo : OperationWebRepo<CalendarEntry>, ICalendarRepo
    {
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly CalendarHelper _calendarHelper;
        private readonly IEntitySerialiser _entitySerialiser;

        public OutlookCalendarRepo(IAuthenticationHelper authenticationHelper, CalendarHelper calendarHelper, IHttpTransferrer downloader, IEntitySerialiser entitySerialiser)
            : base(downloader, entitySerialiser, "")
        {
            _authenticationHelper = authenticationHelper;
            _calendarHelper = calendarHelper;
            _entitySerialiser = entitySerialiser;
        }

        public async Task<XResult<List<CalendarEntry>>> GetRange(DateTime start, DateTime end)
        {
            var client = await _calendarHelper.GetClient();

            //var _currentUser = await _userOperations.GetCurrentUserAsync();

            //if (_currentUser == null)
            //{
            //    return null;
            //}

            //var _calendarCapability = ServiceCapabilities.Calendar.ToString();
            var token = "";

            var uri = client.Context.BaseUri;

            var tStart = start.ToUniversalTime();
            var tEnd = end.ToUniversalTime();

            Debug.WriteLine($"Token: {token}");
            Debug.WriteLine($"Url: {uri}");
            Debug.WriteLine($"Start: {start.ToUniversalTime().ToString("u")}");
            Debug.WriteLine($"End: {end.ToUniversalTime().ToString("u")}");


            var t = ContainerHost.Container.Resolve<IHttpTransferrer>();

            var url = uri.ToString();

            url =
                $"{url}/Me/CalendarView?$top=30&&startDateTime={tStart.ToString("u")}&endDateTime={tEnd.ToString("u")}";

            var result = await GetResult(url);


            if (result == null || !result.IsSuccessCode)
            {
                return XResult<List<CalendarEntry>>.GetFailed();
            }

            var parsed = _entitySerialiser.Deserialise<OutlookResult>(result.Result);

            if (parsed == null || parsed.value == null)
            {
                return XResult<List<CalendarEntry>>.GetFailed();
            }

            var resultList = new List<CalendarEntry>();

            foreach (var thing in parsed.value)
            {
                var version = thing.Id + thing.Start + thing.End;
                if (thing.Categories != null)
                {
                    version += string.Join(",", thing.Categories);
                }

                var cal = new CalendarEntry
                {
                    Start = DateTime.Parse(thing.Start),
                    End = DateTime.Parse(thing.End),
                    Subject = thing.Subject,
                    Id = thing.Id,
                    Type = thing.Type,
                    Categories = thing.Categories,
                    Version = version,
                    Location = thing?.Location?.DisplayName,
                    Attendees = thing.Attendees,
                    Organizer = thing.Organizer,
                    OdataId = thing.OdataId
                   
                    
                };

                if (cal.Start.Date != cal.End.Date)
                {
                    continue;
                }
                resultList.Add(cal);
            }


            return new XResult<List<CalendarEntry>>(resultList);
        }


        public async Task<XResult<List<CalendarEntry>>> GetToday()
        {
            return await GetRange(DateTime.Today, DateTime.Today.AddDays(1).AddMilliseconds(-1));
        }

        public async Task<XResult<List<CalendarEntry>>> GetTomorrow()
        {
            return await GetRange(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2).AddMilliseconds(-1));
        }
    }
}
