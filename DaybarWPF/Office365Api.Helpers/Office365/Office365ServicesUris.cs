using System;

namespace Office365Api.Helpers.Office365
{
    public static class Office365ServicesUris
    {
        public static readonly Uri DiscoveryServiceEndpointUri = new Uri("https://api.office.com/discovery/v1.0/me/");

        public static readonly string DiscoveryServiceResourceId = "https://api.office.com/discovery/";

        public static readonly string AADGraphAPIResourceId = "https://graph.windows.net";
    }
}
