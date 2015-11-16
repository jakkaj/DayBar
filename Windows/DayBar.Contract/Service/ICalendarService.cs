using System.Collections.Generic;
using System.Threading.Tasks;
using DayBar.Entity.Calendars;
using XamlingCore.Portable.Model.Response;

namespace DayBar.Contract.Service
{
    public interface ICalendarService
    {
        Task<XResult<List<CalendarEntry>>> GetToday();
        Task<XResult<List<CalendarEntry>>> GetTomorrow();
    }
}