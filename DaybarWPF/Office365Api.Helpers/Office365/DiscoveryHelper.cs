using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Office365.Discovery;

namespace Office365Api.Helpers.Office365
{
    public class DiscoveryHelper : BaseOffice365Helper
    {
        public DiscoveryHelper(AuthenticationHelper authenticationHelper) : 
            base(authenticationHelper)
        {
            DiscoveryClient discoveryClient =
                new DiscoveryClient(
                    Office365ServicesUris.DiscoveryServiceEndpointUri,
                                    async () =>
                                    {
                                        var discoveryAuthResult =
                                            await this.AuthenticationHelper.AuthenticationContext.AcquireTokenSilentAsync(
                                                Office365ServicesUris.DiscoveryServiceResourceId,
                                                AuthenticationHelper.ClientId,
                                                new UserIdentifier(
                                                    this.AuthenticationHelper.AuthenticationResult.UserInfo.UniqueId, 
                                                    UserIdentifierType.UniqueId));

                                        return discoveryAuthResult.AccessToken;
                                    });

            this.DiscoveryClient = discoveryClient;
        }

        public DiscoveryClient DiscoveryClient
        {
            get;
            private set;
        }
        
        private async Task<CapabilityDiscoveryResult> DiscoverCapabilityInternalAsync(String capabilityName)
        {
            if (DiscoveryClient == null)
            {
                throw new ApplicationException("Missing the DiscoveryClient object!");
            }

            var dcr = await DiscoveryClient.DiscoverCapabilityAsync(capabilityName);
            return dcr;
        }

        // Discovery service supports MyFiles, Mail, Contacts and Calendar
        public async Task<CapabilityDiscoveryResult> DiscoverMail()
        {
            return (await DiscoverCapabilityInternalAsync(Office365Capabilities.Mail.ToString()));
        }

        public async Task<CapabilityDiscoveryResult> DiscoverContacts()
        {
            return (await DiscoverCapabilityInternalAsync(Office365Capabilities.Contacts.ToString()));
        }

        public async Task<CapabilityDiscoveryResult> DiscoverCalendar()
        {
            return (await DiscoverCapabilityInternalAsync(Office365Capabilities.Calendar.ToString()));
        }

        public async Task<CapabilityDiscoveryResult> DiscoverMyFiles()
        {
            return (await DiscoverCapabilityInternalAsync(Office365Capabilities.MyFiles.ToString()));
        }
    }
}
