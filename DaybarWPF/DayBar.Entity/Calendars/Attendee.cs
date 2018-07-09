using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayBar.Entity.Calendars
{
    public class Attendee
    {
        public Status Status { get; set; }
        public string Type { get; set; }
        public EmailAddress EmailAddress { get; set; }
    }

    public class Status
    {
        public string Response { get; set; }
        public string Time { get; set; }
    }

    public class EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
