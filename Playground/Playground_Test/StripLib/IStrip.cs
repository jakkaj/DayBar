namespace StripLib
{
    public interface IStrip
    {
        void SendCommand(string command);
        void Rainbow();
        void SetDot(int dot, int red, int green, int blue);
        void FlashDot(int dot, int red, int green, int blue);
        void FlashRange(int start, int length, int red, int green, int blue);
        void SetRange(int start, int length, int red, int green, int blue);
        void Clear();
    }
}