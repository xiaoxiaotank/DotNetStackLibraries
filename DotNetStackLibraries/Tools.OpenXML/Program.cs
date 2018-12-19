using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OpenXML.Helpers.Reports.ReportA;
using Tools.OpenXML.Models;

namespace Tools.OpenXML
{
    class Program
    {
        static async Task Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                using (var helper = new ReportAHelper("test.xlsx", new ReportDataModel()))
                {
                    helper.GenerateReportByMemory();
                    await helper.GenerateAsync();
                }
            }
        }
    }
}
