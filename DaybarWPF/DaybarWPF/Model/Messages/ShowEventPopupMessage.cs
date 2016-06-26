using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayBar.Entity.Calendars;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace DaybarWPF.Model.Messages
{
    public class ShowEventPopupMessage : XMessage
    {
        public ShowEventPopupMessage(CalendarEntry entry, double offset)
        {
            Entry = entry;
            Offset = offset;
        }

        public CalendarEntry Entry { get; }

        public double Offset { get; }
    }
}
