using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StripLib;
using TestConsole.Impl;

namespace TestConsole
{
    class Program
    {
        private static Strip _strip;
        static void Main(string[] args)
        {
            var p = new StripWriter("COM4");

            _strip = new Strip(StripSize.Size_144, p);

            //_stripTest();
            _writer();
            while (true)
            {
                Thread.Sleep(1000);
            }


        }

        static async void _stripTest()
        {


            //strip.Rainbow();

            var rnd = new Random();

            while (true)
            {


                _strip.Clear();
                //await Task.Delay(500);

                _strip.SetDot(rnd.Next(0, 120), 255, 0, 0);
                _strip.SetDot(rnd.Next(0, 120), 0, 0, 255);
                _strip.FlashDot(rnd.Next(0, 120), 0, 255, 0);
                _strip.SetRange(rnd.Next(0, 120), 10, 255, 165, 0);
                _strip.SetRange(rnd.Next(0, 120), 10, 255, 165, 0);
                _strip.FlashRange(rnd.Next(0, 120), 12, 255, 0, 0);

                await Task.Delay(6000);
            }


        }

        async static void _writer()
        {
            _strip.Clear();
            while (true)
            {
                var tNow = DateTime.Now;

                var t7 = DateTime.Today.AddHours(7);

                var tSince = tNow.Subtract(t7);

                var minutes = tSince.TotalMinutes;

                var actual = (minutes / 720) * 144;

                Debug.WriteLine(actual);

                var actualAbs = (int)actual;
               
                _strip.FlashDot(actualAbs, 0, 255, 0);

                if (actualAbs > 0)
                {
                    _strip.SetDot(actualAbs - 1, 0, 0, 10);
                }
                await Task.Delay(5000);
            }
            
        }
    }
}
