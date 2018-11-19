using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.OpenXML.Models
{
    class ReportDataModel
    {
        public object Sheet1Data { get; set; }

        public object Sheet2Data { get; set; }

        public IReadOnlyList<Sheet3Data> Sheet3DataList { get; set; }

        public ReportDataModel()
        {
            var sheet3DataList = new List<Sheet3Data>();
            for (int i = 0; i < 8; i++)
            {
                sheet3DataList.Add(new Sheet3Data()
                {
                    ProductName = $"P{i + 1}",
                    DataDic = new Dictionary<string, int>()
                    {
                        ["Month1"] = (i + 1) * 10,
                        ["Month2"] = (i + 1) * 20
                    }
                });
            }
            Sheet3DataList = sheet3DataList;
        }
    }

    class Sheet3Data
    {
        public string ProductName { get; set; }

        public Dictionary<string, int> DataDic { get; set; }
    }
}
