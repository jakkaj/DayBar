using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StripLib;

namespace TestConsole.Impl
{
    public class StripWriter : IStripWriter
    {
        private SerialPort _port;
        private bool _isOpen = false;
        Queue<string> _commands = new Queue<string>(); 
        public StripWriter(string portName)
        {
            _port = new SerialPort();

            foreach (var p in SerialPort.GetPortNames())
            {
                Console.WriteLine(p);
            }

            _port.BaudRate = 9600;
            _port.PortName = portName;

            _port.Open();
            _isOpen = _port.IsOpen;

            _loop();
        }

        async void _loop()
        {

            while (true)
            {
                await Task.Delay(500);
                if (_isOpen)
                {
                    if (_commands.Count > 0)
                    {
                        var command = _commands.Dequeue();
                        _port.Write(command);
                        Console.WriteLine($"Sending: {command}");
                    }
                }
            }
        }
        public void Write(string command)
        {
            
            _commands.Enqueue(command);
        }
    }
}
