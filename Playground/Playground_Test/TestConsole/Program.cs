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
        static void Main(string[] args)
        {
            _stripTest();
            //_writer();
            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        static async void _stripTest()
        {
            var p = new StripWriter("COM4");

            var strip = new Strip(StripSize.Size_144, p);

            //strip.Rainbow();

            var rnd = new Random();

            while (true)
            {
                

                strip.Clear();
                //await Task.Delay(500);
                
                strip.SetDot(rnd.Next(0, 120), 255, 0, 0);
                strip.SetDot(rnd.Next(0, 120), 0, 0, 255);
                strip.FlashDot(rnd.Next(0, 120), 0, 255, 0);
                strip.SetRange(rnd.Next(0, 120), 10, 255, 165, 0);
                strip.SetRange(rnd.Next(0, 120), 10, 255, 165, 0);
                strip.FlashRange(rnd.Next(0, 120), 12, 255, 0, 0);

                await Task.Delay(6000);
            }

            
        }

        async static void _writer()
        {
            var port = new SerialPort();

            foreach (var p in SerialPort.GetPortNames())
            {
                Console.WriteLine(p);
            }

            port.BaudRate = 9600;
            port.PortName = "COM4";

            port.Open();
            var open = port.IsOpen;

            //for (var i = 0; i < 144; i++)
            //{
            //    port.Write($">1,{i},0,0,255<");
            //    await Task.Delay(40);
            //}

            while (true)
            {
                var tNow = DateTime.Now;

                var t7 = DateTime.Today.AddHours(7);

                var tSince = tNow.Subtract(t7);

                var minutes = tSince.TotalMinutes;

                var actual = (minutes / 760) * 144;
                Debug.WriteLine(actual);
                var actualAbs = (int)actual;

                port.Write($">1,{actualAbs},0,255,0<");
                await Task.Delay(5000);
            }

            
            port.Close();
        }
    }
}
