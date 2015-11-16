using System.Threading.Tasks;

namespace DayBar.Contract.Strip
{
    public interface IStripWriter
    {
        void Write(string command);
        Task<bool> Init();
    }
}
