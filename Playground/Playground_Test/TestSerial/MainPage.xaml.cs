using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestSerial
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            //uint32_t Adafruit_NeoPixel::Color(uint8_t r, uint8_t g, uint8_t b) {
           // return ((uint32_t)r << 16) | ((uint32_t)g << 8) | b;
        //}

            UInt32 color = 255 << 16 | 0 << 8 | 0;
            uint r = (color & 0x00FF0000) >> 16;
            uint g = (color & 0x0000FF00) >> 8;
            uint b = (color & 0x000000FF);
            Debug.WriteLine(color);
        }
    }
}
