using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OpenXML.Tools.OpenXMLExcel;

namespace Tools.OpenXML.Helpers.Reports
{
    class ReportHelper<T> : IDisposable
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        private static readonly string _filePath = Path.Combine(Files.FilePath, "Excels");

        /// <summary>
        /// 对象锁
        /// </summary>
        private readonly object _obj = new object();
        /// <summary>
        /// OpenXMLExcel类
        /// </summary>
        protected readonly OpenXMLExcelBase _openXMLExcel;
        /// <summary>
        /// 报告数据
        /// </summary>
        protected readonly T _data;

        /// <summary>
        /// 报告全名（全路径）
        /// </summary>
        public string ReportFullName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">文件名（含后缀）</param>
        /// <param name="data">数据</param>
        public ReportHelper(string fileName, T data)
        {
            ReportFullName = Path.Combine(_filePath, fileName);
            _openXMLExcel = new OpenXMLExcel();
            _data = data;
        }

        /// <summary>
        /// 生成报告
        /// </summary>
        public virtual void Generate()
        {
            InitDocument();
        }

        public void Dispose()
        {
            if (_openXMLExcel.Document != null)
            {
                _openXMLExcel.Document.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 初始化文档
        /// </summary>
        protected void InitDocument()
        {
            if (_openXMLExcel.Document == null)
            {
                lock (_obj)
                {
                    if (_openXMLExcel.Document == null)
                    {
                        _openXMLExcel.CreateDocument(ReportFullName);
                        _openXMLExcel.AddWorkbook();
                    }
                }
            }
        }

        ~ReportHelper()
        {
            Dispose();
        }
    }
}
