using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Autofac;
using Daybar.Core.Model.Strip;
using DayBar.Contract.Repo;
using DayBar.Contract.Service;
using DayBar.Contract.Strip;
using DayBar.Entity.Strip;
using DayBar.UWP.Office365;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Data.Glue;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DayBar.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IStrip _strip;

        DateTime _lastUpdate = DateTime.Now;
        int _lastActual = -1;
        public MainPage()
        {
            this.InitializeComponent();
            
            _init();
        }

        async void _init()
        {
            StripElement.IsReversed = true;
            _strip = ContainerHost.Container.Resolve<IStrip>();
            var w = ContainerHost.Container.Resolve<IStripWriter>();
            await w.Init();

            _strip.Clear();
            await Task.Delay(3000);
           // _strip.Rainbow();

            //return;

            _loop();
        }


        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _go();
        }

        async Task _go()
        {
            var c = ContainerHost.Container.Resolve<ICalendarService>();
            
           

            //await Task.Delay(1000);
            //s.Rainbow();

            //_strip.FlashRange(0, 144, 0, 255, 0);

            var events = await c.GetToday();
            if (events == null)
            {
                Debug.WriteLine("Could not get events");
                return;
            }
            if (!events.IsSuccess)
            {
                _strip.SetRange(0, 144, 150, 0, 0);
            }
            else
            {
                //_strip.Clear();

                var tags = events.Object.Select(_ => _.Version).ToList();

                _strip.CheckTags(tags);

                if (events.Object.Any(_ => !_strip.HasTag(_.Version)))
                {
                    _strip.Clear();
                }


                foreach (var i in events.Object)
                {
                    if (_strip.HasTag(i.Version))
                    {
                        continue;
                    }

                    var t7 = DateTime.Today.AddHours(7);

                    var tSince = i.Start.Subtract(t7);

                    var minutes = tSince.TotalMinutes;

                    var actualStart = (minutes / 720) * 144;

                    var runLength = i.End.Subtract(i.Start).TotalMinutes;

                    var actualEnd = (runLength / 720) * 144;

                    int r = 0;
                    int g = 0;
                    int b = 100;

                    if (i.Categories != null)
                    {
                        foreach (var cat in i.Categories)
                        {
                            var cName = cat.ToLower();
                            if (cName.IndexOf("green") != -1)
                            {
                                r = 0;
                                g = 255;
                                b = 0;
                            }
                            if (cName.IndexOf("blue") != -1)
                            {
                                r = 0;
                                g = 0;
                                b = 255;
                            }
                            if (cName.IndexOf("orange") != -1)
                            {
                                r = 255;
                                g = 165;
                                b = 0;
                            }
                            if (cName.IndexOf("purple") != -1)
                            {
                                r = 148;
                                g = 0;
                                b = 211;
                                
                            }
                            if (cName.IndexOf("red") != -1)
                            {
                                r = 255;
                                g = 0;
                                b = 0;
                            }
                            if (cName.IndexOf("yellow") != -1)
                            {
                                r = 255;
                                g = 255;
                                b = 0;
                            }
                        }
                    }

                    _strip.SetRange((int)actualStart, (int)actualEnd, r, g, b, i.Version);

                }
                _updateNowDot();
                //s.SetRange(0, 144, 0, 150, 0);
            }

        }

        async void _loop()
        {

            await _go();
            

           

            while (true)
            {

                if (DateTime.Now.Subtract(_lastUpdate) > TimeSpan.FromMinutes(2))
                {
                    _lastUpdate = DateTime.Now;
                    await _go();
                }

                await Task.Delay(5000);

                _updateNowDot();

            }

        }

        void _updateNowDot()
        {
            var tNow = DateTime.Now;

            var t7 = DateTime.Today.AddHours(7);

            var tSince = tNow.Subtract(t7);

            var minutes = tSince.TotalMinutes;

            var actual = (minutes / 720) * 144;

            Debug.WriteLine(actual);

            var actualAbs = (int)actual;

            if (actualAbs == _lastActual)
            {
                return;
            }

            _lastActual = actualAbs;

            _strip.FlashDot(actualAbs, 0, 255, 0);

            if (actualAbs < 0)
            {
                _strip.FlashDot(0, 255, 0, 10);
            }
            else if (actualAbs >= 144)
            {
                _strip.FlashDot(143, 255, 0, 10);
            }
        }
    }
}
