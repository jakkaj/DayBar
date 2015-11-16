using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayBar.Contract.Auth
{
    public interface ITokenGetService
    {
        Task<string> GetToken();
    }
}
