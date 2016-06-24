using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using DaybarWPF.View.Control;
using DayBar.Contract.Service;
using DayBar.Entity.Calendars;
using XamlingCore.Portable.View.ViewModel;

namespace DaybarWPF.View
{
    public class BarViewModel : XViewModel
    {
        private readonly ICalendarService _calendarService;
        private readonly IUserService _userService;
        private readonly ILifetimeScope _scope;
        private readonly IDeviceService _deviceService;

        private List<BarItemViewModel> items;

        private NowViewModel _nowViewModel;

        private bool _isLoading;

        private double _width = 500;

        public BarViewModel(ICalendarService calendarService, IUserService userService, ILifetimeScope scope, IDeviceService deviceService)
        {
            _calendarService = calendarService;
            _userService = userService;
            _scope = scope;
            _deviceService = deviceService;
            NowViewModel = scope.Resolve<NowViewModel>();
            Width = deviceService.WindowWidth;
        }

        public void Init()
        {
            //find events
            _refreshCalendar();
        }


        async void _refreshCalendar()
        {
            IsLoading = true;
            var isLoggedIn = await _userService.EnsureLoggedIn(false);

            if (!isLoggedIn)
            {
                IsLoading = false;
                //TODO: Add logged out thingo
                return;
            }

            var events = await _calendarService.GetToday();

            if (!events)
            {
                //TODO Error getting events. 
                return;
            }
            var wrapped = _wrap(events.Object);

            Items = wrapped;
            IsLoading = false;
        }

        List<BarItemViewModel> _wrap(List<CalendarEntry> events)
        {
            var l = new List<BarItemViewModel>();

            foreach (var i in events)
            {
                var vm = _scope.Resolve<BarItemViewModel>();
                vm.Entry = i;

                l.Add(vm);
            }

            return l;
        }

        public List<BarItemViewModel> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged();
            }
        }

        public NowViewModel NowViewModel
        {
            get { return _nowViewModel; }
            set
            {
                _nowViewModel = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }
    }
}
