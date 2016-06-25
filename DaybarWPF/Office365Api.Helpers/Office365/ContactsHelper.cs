﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Office365.OutlookServices;

namespace Office365Api.Helpers.Office365
{
    public class ContactsHelper : BaseOffice365Helper
    {
        public ContactsHelper(AuthenticationHelper authenticationHelper) : 
            base(authenticationHelper)
        {
        }

        public async Task<IEnumerable<IContact>> GetContacts()
        {
            var client = await this.AuthenticationHelper
                .EnsureOutlookServicesClientCreatedAsync(
                Office365Capabilities.Contacts.ToString());

            // Obtain first page of contacts
            var contactsResults = await (from i in client.Me.Contacts
                                         orderby i.DisplayName
                                         select i).ExecuteAsync();
            if (contactsResults != null)
            {
                return contactsResults.CurrentPage;
            }
            else
            {
                return null;
            }
        }
    }
}
