using System.Collections.Generic;
using System.Threading.Tasks;
using DayBar.Entity.Calendars;
using XamlingCore.Portable.Model.Response;

namespace DayBar.Contract.Repo
{
    public interface ICalendarRepo
    {
        Task<XResult<List<CalendarEntry>>> GetToday();
        Task<XResult<List<CalendarEntry>>> GetTomorrow();
    }
}
