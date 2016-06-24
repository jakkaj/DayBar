using System.Threading.Tasks;
using DayBar.Entity.Calendars;

namespace DayBar.Contract.Service
{
    public interface IBarConfigService
    {
        Task<CalandarConfig> GetCalendarConfig();
    }
}