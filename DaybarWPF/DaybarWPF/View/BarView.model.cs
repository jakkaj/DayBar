using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using Autofac;
using DaybarWPF.Model.Messages;
using DaybarWPF.View.Control;
using DayBar.Contract.Service;
using DayBar.Entity.Calendars;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.View.ViewModel;

namespace DaybarWPF.View
{
    public class BarViewModel : XViewModel
    {
        private readonly ICalendarService _calendarService;
        private readonly IUserService _userService;
        private readonly ILifetimeScope _scope;
        private readonly IDeviceService _deviceService;
        private readonly IBarConfigService _barConfig;

        private List<BarItemViewModel> items;
        private List<TimeLegendViewModel> _timeItems;

        private NowViewModel _nowViewModel;

        private bool _isLoading;

        private double _width = 500;

        private Timer _timer;

        private DateTime? _lastRefresh = null;

        private EventPopupView _popupView;

        public BarViewModel(ICalendarService calendarService, 
            IUserService userService, ILifetimeScope scope, 
            IDeviceService deviceService, IBarConfigService barConfig)
        {
            _calendarService = calendarService;
            _userService = userService;
            _scope = scope;
            _deviceService = deviceService;
            _barConfig = barConfig;
            NowViewModel = scope.Resolve<NowViewModel>();
            Width = deviceService.WindowWidth;

            this.Register<ShowEventPopupMessage>(_showEventPopup);
            this.Register<HideEventPopupMessage>(_hideEventPopup);


            _timer = new Timer();
            _timer.Interval = 5000;
            _timer.Elapsed += T_Elapsed;
            _timer.Start();
        }

        public override void Dispose()
        {
            //base.Dispose();
            
            _timer.Stop();
            
        }

        async void _showEventPopup(object message)
        {
            Dispatcher.Invoke(() =>
            {
                var m = message as ShowEventPopupMessage;

                if (m?.Entry == null)
                {
                    return;
                }

                if (_popupView != null)
                {
                    _popupView.Close();
                }

                _popupView = _scope.Resolve<EventPopupView>();
                

                _popupView.Show();

                _popupView.SetEvent(m.Entry, m.Offset);
            });
            
        }

        void _hideEventPopup()
        {
            Dispatcher.Invoke(() =>
            {
                if (_popupView != null)
                {
                    _popupView.Close();
                }
                _popupView = null;

            });
        }


        private async void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            _scanEvents();
            if(_lastRefresh == null || DateTime.Now.Subtract(_lastRefresh.Value) > TimeSpan.FromSeconds(60000))
            RefreshCalendar();
        }

        void _scanEvents()
        {
            if (_timeItems == null || Items.Count == 0)
            {
                return;
            }

            var now = DateTime.Now.AddMinutes(-5);
            var nowten = DateTime.Now.AddMinutes(10);
          
            foreach (var item in items)
            {
                if (item.Entry.Start > now && item.Entry.Start < nowten)
                {
                    new EventAttractorMessage().Send();
                    return;
                }
            }
        }

        public void Init()
        {
            //find events
            RefreshCalendar();
        }


        public void RefreshCalendar(bool showLoad = false)
        {
            MyDispatcher.Invoke(async () =>
            {
                _lastRefresh = DateTime.Now;
                if (Items == null || Items?.Count == 0 || showLoad)
                {
                    IsLoading = true;
                }
                
                Items?.Clear();

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


                var ti = new List<TimeLegendViewModel>();


                var config = await _barConfig.GetCalendarConfig();

                var hours = config.EndHour - config.StartHour;

                for (var i = 0; i <= hours; i++)
                {
                    var vm = _scope.Resolve<TimeLegendViewModel>();
                    vm.Offset = await _deviceService.TimeToXPos(DateTime.Today.AddHours(config.StartHour).AddHours(i));
                    if (i == hours)
                    {
                        vm.Offset -= 15;
                    }
                    vm.Time = (config.StartHour + i).ToString();
                    ti.Add(vm);
                }

                TimeItems = ti;

                IsLoading = false;
            });
            
            
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

        public Dispatcher MyDispatcher { get; set; }

        public List<TimeLegendViewModel> TimeItems
        {
            get { return _timeItems; }
            set
            {
                _timeItems = value; 
                OnPropertyChanged();
            }
        }
    }
}
