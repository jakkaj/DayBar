using System.Collections.Generic;
using DayBar.Entity.Calendars;
using Newtonsoft.Json;

namespace Office365Api.Helpers.Impl
{
    public class OutlookValue
    {
        public string Id { get; set; }
        [JsonProperty(PropertyName = "@odata.id")]
        public string OdataId { get; set; }
        public string Subject { get; set; }
        public string Start { get; set; }
        public string End { get; set; }

      
        public string Type { get; set; }
       
        public string ChangeKey { get; set; }

        public List<string> Categories { get; set; }
        public Location Location { get; set; }

        public List<Attendee> Attendees { get; set; }

        public Attendee Organizer { get; set; }

        
    }

    public class Location
    {
        public string DisplayName { get; set; }
    }

    public class OutlookResult
    {
        public List<OutlookValue> value { get; set; }
    }

}
