using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.OpenXML
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var helper = new ReportHelper("test.xlsx", null))
            {
                helper.GenerateReport();
            }
        }
    }
}
