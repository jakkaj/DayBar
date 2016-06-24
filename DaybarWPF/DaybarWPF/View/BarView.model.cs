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

        private List<BarItemViewModel> items;

        public BarViewModel(ICalendarService calendarService, IUserService userService, ILifetimeScope scope)
        {
            _calendarService = calendarService;
            _userService = userService;
            _scope = scope;
        }

        public void Init()
        {
            //find events
            _refreshCalendar();
        }


        async void _refreshCalendar()
        {
            var isLoggedIn = await _userService.EnsureLoggedIn(false);

            if (!isLoggedIn)
            {
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

    }
}
