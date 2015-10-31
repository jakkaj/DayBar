using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripLib
{
    public class Strip : IStrip
    {
        private readonly IStripWriter _writer;
        private int _range;
        public Strip(StripSize size, IStripWriter writer)
        {
            _writer = writer;
            if (size == StripSize.Size_144)
            {
                _range = 144;
            }
        }

        public void SendCommand(string command)
        {
            _writer.Write(command);
        }

        public void Rainbow()
        {
            var command = $">3<";
            SendCommand(command);
        }

        public void SetDot(int dot, int red, int green, int blue)
        {
            var command = $">1,{dot},{red},{green},{blue}<";
            SendCommand(command);
        }

        public void FlashDot(int dot, int red, int green, int blue)
        {
            var command = $">4,{dot},{red},{green},{blue}<";
            SendCommand(command);
        }

        public void FlashRange(int start, int length, int red, int green, int blue)
        {
            var command = $">5,{start},{red},{green},{blue},{length}<";
            SendCommand(command);
        }

        public void SetRange(int start, int length, int red, int green, int blue)
        {
            var command = $">2,{start},{red},{green},{blue},{length}<";
            SendCommand(command);
        }

        public void Clear()
        {
            SetRange(0, _range, 0, 0, 0);
        }
    }
}
