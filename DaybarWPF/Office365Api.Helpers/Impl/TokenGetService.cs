using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayBar.Contract.Auth;

namespace Office365Api.Helpers.Impl
{
    public class TokenGetService : ITokenGetService
    {
        private readonly CalendarHelper _auth;

        public TokenGetService(CalendarHelper auth)
        {
            _auth = auth;
        }

        public async Task<string> GetToken()
        {
            var token = await _auth.GetCalendarToken();

            return token;
        }
    }
}
