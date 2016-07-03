using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DaybarWPF.Glue;

namespace DaybarWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ProjectGlue Glue { get; set; }
        public App()
        {
            Glue = new ProjectGlue();
            Glue.Init();



        }
    }

}
