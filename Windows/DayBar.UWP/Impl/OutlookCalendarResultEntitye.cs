using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayBar.UWP.Impl
{
    public class OutlookValue
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Start { get; set; }
        public string End { get; set; }

      
        public string Type { get; set; }
       
        public string ChangeKey { get; set; }

        public List<string> Categories { get; set; }
    }
    public class OutlookResult
    {
        public List<OutlookValue> value { get; set; }
    }

}
