using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OpenXML.Helpers.Reports.Report1;
using Tools.OpenXML.Models;

namespace Tools.OpenXML
{
    class Program
    {
        static async Task Main(string[] args)
        {
            for (int i = 0; i < 1; i++)
            {
                using (var helper = new Report1Helper("test.xlsx", new ReportDataModel()))
                {
                    helper.GenerateReportByMemory();
                    await helper.GenerateAsync();
                }
            }
        }
    }
}
