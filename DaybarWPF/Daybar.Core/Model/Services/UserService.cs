using System;
using System.Threading.Tasks;
using DayBar.Contract.Office;
using DayBar.Contract.Service;

namespace Daybar.Core.Model.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthenticationHelper _authenticationHelperService;

        public UserService(IAuthenticationHelper authenticationHelperService)
        {
            _authenticationHelperService = authenticationHelperService;
        }

        public async Task<bool> EnsureLoggedIn(bool noShow)
        {
            var result = await _authenticationHelperService.EnsureAuthenticationContext(Constants.Authority, noShow);
            return result;
        }

        
    }
}
