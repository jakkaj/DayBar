using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using DayBar.Contract.Strip;

namespace DayBar.UWP.Impl
{
    public class StripWriter : IStripWriter
    {
        private SerialDevice _serialPort;
        DataReader _dataReaderObject = null;
        private CancellationTokenSource _readCancellationTokenSource;
        Queue<string> _commands = new Queue<string>();

        public StripWriter()
        {
            _loop();
        }

        public void Write(string command)
        {
            _commands.Enqueue(command);
        }

        public async Task<bool> Init()
        {
            return true;
            // return await _discoverStrip();
        }



        async void _loop()
        {

            while (true)
            {
                await Task.Delay(50);

                if (_serialPort == null)
                {
                    await _discoverStrip();
                }

                if (_serialPort!=null)
                {
                    if (_commands.Count > 0)
                    {
                        var command = _commands.Dequeue();
                       
                        await _write($"{command}\n");
                        Debug.WriteLine($"Sending: {command}");
                        
                    }
                }
            }
        }

        public async Task _write(string command)
        {
            DataWriter dataWriteObject = null;
            if (_serialPort == null)
            {
                return;
            }
            try
            {
                dataWriteObject = new DataWriter(_serialPort.OutputStream);
                dataWriteObject.WriteString(command);

                // Launch an async task to complete the write operation
                var storeAsyncTask = dataWriteObject.StoreAsync().AsTask();

                UInt32 bytesWritten = await storeAsyncTask;
                if (bytesWritten > 0)
                {
                    Debug.WriteLine($"Bytes: {bytesWritten}");
                }

               

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                CloseDevice();
            }
            finally
            {
                if (dataWriteObject != null)
                {
                    dataWriteObject.DetachStream();
                    dataWriteObject = null;
                }
            }
        }

        
        async Task<bool> _discoverStrip()
        {
            try
            {
                if (_serialPort != null)
                {
                    return true;
                }
                string aqs = SerialDevice.GetDeviceSelector();
                var dis = await DeviceInformation.FindAllAsync(aqs);

                if (dis.Count == 0)
                {
                    return false;
                }

                foreach (var e in dis)
                {
                    var s = await SerialDevice.FromIdAsync(e.Id);

                    if (s == null)
                    {
                        continue;
                    }

                    _serialPort = s;

                    _serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                    _serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                    _serialPort.BaudRate = 115200;
                    _serialPort.Parity = SerialParity.None;
                    _serialPort.StopBits = SerialStopBitCount.One;
                    _serialPort.DataBits = 8;
                    _readCancellationTokenSource = new CancellationTokenSource();
                    _listen();
                    break;
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                CloseDevice();
            }

            return true;

        }

        private async void _listen()
        {
            try
            {
                if (_serialPort != null)
                {
                    _dataReaderObject = new DataReader(_serialPort.InputStream);

                    // keep reading the serial input
                    while (true)
                    {
                        await ReadAsync(_readCancellationTokenSource.Token);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "TaskCanceledException")
                {
                    Debug.WriteLine("Reading task was cancelled, closing device and cleaning up");
                    //status.Text = "Reading task was cancelled, closing device and cleaning up";
                    CloseDevice();
                }
                else
                {
                    Debug.WriteLine(ex.Message);
                    
                }
            }
            finally
            {
                // Cleanup once complete
                if (_dataReaderObject != null)
                {
                    _dataReaderObject.DetachStream();
                    _dataReaderObject = null;
                }
            }
        }

        private async Task ReadAsync(CancellationToken cancellationToken)
        {
            Task<UInt32> loadAsyncTask;

            uint ReadBufferLength = 1024;

            // If task cancellation was requested, comply
            cancellationToken.ThrowIfCancellationRequested();

            // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
            _dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;

            // Create a task object to wait for data on the serialPort.InputStream
            loadAsyncTask = _dataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);

            // Launch the task and wait
            try
            {
                UInt32 bytesRead = await loadAsyncTask;
                if (bytesRead > 0)
                {
                    var text = _dataReaderObject.ReadString(bytesRead);
                    Debug.WriteLine(text);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                CloseDevice();
            }
          
        }

        public void CloseDevice()
        {
            if (_serialPort == null)
            {
                return;
            }
            _readCancellationTokenSource.Cancel();

            _readCancellationTokenSource = null;
            if (_dataReaderObject != null)
            {
                _dataReaderObject.DetachStream();
                _dataReaderObject = null;
            }
            _serialPort = null;
        }
    }
}
