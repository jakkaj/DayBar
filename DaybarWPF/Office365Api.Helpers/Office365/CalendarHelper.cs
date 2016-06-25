using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Office365.OutlookServices;

namespace Office365Api.Helpers.Office365
{
    public class CalendarHelper : BaseOffice365Helper
    {
        public CalendarHelper(AuthenticationHelper authenticationHelper) : 
            base(authenticationHelper)
        {
        }

        public async Task<string> GetCalendarToken()
        {
            return await AuthenticationHelper.GetOutlookToken();
        }

        public async Task<OutlookServicesClient> GetClient()
        {
            var client = await this.AuthenticationHelper
               .EnsureOutlookServicesClientCreatedAsync(
               Office365Capabilities.Calendar.ToString());

            return client;
        }

        public async Task<IOrderedEnumerable<IEvent>> GetCalendarEvents()
        {
            var client = await this.AuthenticationHelper
                .EnsureOutlookServicesClientCreatedAsync(
                Office365Capabilities.Calendar.ToString());

            // Obtain calendar event data
            var eventsResults = await (from i in client.Me.Events
                                      where i.End >= DateTimeOffset.UtcNow
                                      select i).Take(10).ExecuteAsync();

            if (eventsResults != null)
            {
                var events = eventsResults.CurrentPage.OrderBy(e => e.Start);
                return events;
            }
            else
            {
                return (null);
            }
        }
    }
}
