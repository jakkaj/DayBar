using System;
using System.Threading.Tasks;

namespace DayBar.Contract.Office
{
    public interface IAuthenticationHelper
    {
        Task<bool> EnsureAuthenticationContext(String authority, bool show);
       
       
        Task<string> GetOutlookToken();
    }
}