using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace Office365Api.Helpers.Office365
{
    public class ActiveDirectoryHelper : BaseOffice365Helper
    {
        public ActiveDirectoryHelper(AuthenticationHelper authenticationHelper) : 
            base(authenticationHelper)
        {
        }

        public async Task<IEnumerable<IUser>> GetUsers()
        {
            var client = EnsureClientCreated();

            var userResults = await client.DirectoryObjects.OfType<User>().ExecuteAsync();

            List<IUser> allUsers = new List<IUser>();

            do
            {
                allUsers.AddRange(userResults.CurrentPage);
                userResults = await userResults.GetNextPageAsync();
            } while (userResults != null);

            return allUsers;
        }

        public ActiveDirectoryClient EnsureClientCreated()
        {
            Uri serviceRoot = new Uri(
                new Uri(Office365ServicesUris.AADGraphAPIResourceId), 
                this.AuthenticationHelper.AuthenticationResult.TenantId);

            // Create the ActiveDirectoryClient client proxy:
            return new ActiveDirectoryClient(
                serviceRoot,
                async () =>
                {
                    return await this.AuthenticationHelper
                        .GetAccessTokenForServiceAsync(Office365ServicesUris.AADGraphAPIResourceId);
                });
        }
    }
}
