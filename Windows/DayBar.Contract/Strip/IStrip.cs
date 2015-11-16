using System.Collections.Generic;

namespace DayBar.Contract.Strip
{
    public interface IStrip
    {
        void SendCommand(string command);
        void Rainbow();
        void SolidDot(int dot, int red, int green, int blue, string tag = null);
        void FlashDot(int dot, int red, int green, int blue, string tag = null);
        void FlashRange(int start, int length, int red, int green, int blue, string tag = null);
        void SetRange(int start, int length, int red, int green, int blue, string tag = null);
        void Clear();
        void CheckTags(List<string> tags);
        bool HasTag(string tag);
    }
}