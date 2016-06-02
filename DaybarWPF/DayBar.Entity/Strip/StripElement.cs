using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayBar.Entity.Strip
{
    

    public enum StripElementType
    {
        FlashDot,
        SolidDot,
        FlashRange,
        SolidRange
    }
    public class StripElement
    {
        public static bool IsReversed { get; set; }

        private int _start;
        public StripElementType Type { get; set; }

        public int Start
        {
            get
            {
                if (IsReversed)
                {
                    return 143 - (_start + Length);
                }
                return _start;
            }
            set
            {
                _start = value;
            }
        }

        public int Length { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public string Tag { get; set; }
    }
}
