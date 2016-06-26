using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayBar.Entity.Calendars
{
    public class CalendarEntry
    {
        public string Subject { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string Type { get; set; }
        public string Id { get; set; }
        public string Version { get; set; }
        public string Location { get; set; }

        public List<string> Categories { get; set; } 
    }
}
