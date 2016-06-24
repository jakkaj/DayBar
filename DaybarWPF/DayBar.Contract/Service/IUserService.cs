using System.Threading.Tasks;

namespace DayBar.Contract.Service
{
    public interface IUserService
    {
        Task<bool> EnsureLoggedIn(bool noShow);
    }
}