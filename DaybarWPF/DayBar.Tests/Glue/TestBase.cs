using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayBar.Tests.Glue
{
    public class TestBase
    {
        public ProjectGlue Glue { get; set; }
        public TestBase()
        {
            Glue = new ProjectGlue();
            Glue.Init();
        }
    }
}
