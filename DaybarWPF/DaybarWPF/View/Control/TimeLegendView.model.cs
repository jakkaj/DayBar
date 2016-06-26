using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;

namespace DaybarWPF.View.Control
{
    public class TimeLegendViewModel : XViewModel
    {
        private double _offset;

        private string _time;

        public double Offset
        {
            get { return _offset; }
            set
            {
                _offset = value;
                OnPropertyChanged();
            }
        }

        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }
    }
}
