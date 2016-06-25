using System.Collections.Generic;
using System.Threading.Tasks;
using DayBar.Contract.Repo;
using DayBar.Contract.Service;
using DayBar.Entity.Calendars;
using XamlingCore.Portable.Model.Response;

namespace Daybar.Core.Model.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly ICalendarRepo _repo;

        public CalendarService(ICalendarRepo repo)
        {
            _repo = repo;
        }

        public async Task<XResult<List<CalendarEntry>>> GetToday()
        {
            return await _repo.GetToday();
        }

        public async Task<XResult<List<CalendarEntry>>> GetTomorrow()
        {
            return await _repo.GetTomorrow();
        }
    }
}
