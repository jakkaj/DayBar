using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayBar.Contract.Service;
using DayBar.Entity.Calendars;

namespace Daybar.Core.Model.Services
{
    public class BarConfigService : IBarConfigService
    {
        public async Task<CalandarConfig> GetCalendarConfig()
        {
            return new CalandarConfig
            {
                StartHour = 7,
                EndHour = 23
            };
        }
    }
}
