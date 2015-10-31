using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            _writer();
            while (true)
            {
                Thread.Sleep(1000);
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
