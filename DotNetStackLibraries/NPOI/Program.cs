using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPOI
{
    class Program
    {
        /// <summary>
        /// 字符单位
        /// </summary>
        public const short CharUnit = 256;
        /// <summary>
        /// 表头数
        /// </summary>
        public const short HeaderCount = 6;

        public static readonly string[] Headers = new[] { "安装地址", "IP", "下行设备", "设备型号", "所属机构", "在线状态" };

        static void Main(string[] args)
        {
            var workbook = CreateWorkbook("TestExcel");
            using(var fs = new FileStream("Test.xls", FileMode.Create))
            {
                workbook.Write(fs);
            }

            Console.WriteLine("Excel文件已生成！");
        }

        /// <summary>
        /// 创建工作簿
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static HSSFWorkbook CreateWorkbook(string sheetName)
        {
            #region 创建DocumentSummaryInformation和SummaryInformation
            var docSummary = PropertySetFactory.CreateDocumentSummaryInformation();
            docSummary.Company = "贾建军公司";
            var summary = PropertySetFactory.CreateSummaryInformation();
            summary.Subject = "我是主题";
            #endregion

            //创建工作簿
            var workbook = new HSSFWorkbook()
            {
                DocumentSummaryInformation = docSummary,
                SummaryInformation = summary
            };

            //创建工作表
            var sheet = workbook.CreateSheet(sheetName);
            //冻结表头行
            sheet.CreateFreezePane(0, 1);
            //创建表头行
            var headerRow = sheet.CreateRow(0);
            //设置表头行高
            headerRow.HeightInPoints = 19.5f;

            #region 设置列宽
            sheet.SetColumnWidth(0, 50 * CharUnit);
            sheet.SetColumnWidth(1, 24 * CharUnit);
            sheet.SetColumnWidth(2, 37 * CharUnit);
            sheet.SetColumnWidth(3, 22 * CharUnit);
            sheet.SetColumnWidth(4, 20 * CharUnit);
            sheet.SetColumnWidth(5, 20 * CharUnit);
            #endregion

            #region 生成表头
            for (int i = 0; i < HeaderCount; i++)
            {
                //创建单元格
                var headerCell = headerRow.CreateCell(i);
                headerCell.CellStyle = GetCellStyleByType(workbook, CellStyleType.Header);
                headerCell.SetCellValue(Headers[i]);
            }
            #endregion

            #region 合并单元格并填充数据
            sheet.AddMergedRegion(new CellRangeAddress(1, 2, 0, 1));
            var row1 = sheet.CreateRow(1);
            var cell0 = row1.CreateCell(0);
            cell0.CellStyle = GetCellStyleByType(workbook,CellStyleType.Common);
            cell0.SetCellValue("世纪德辰");
            var cell2 = row1.CreateCell(2);
            cell2.CellStyle = GetCellStyleByType(workbook, CellStyleType.Common);
            cell2.SetCellValue("无");
            #endregion

            #region 添加注释
            var patriarch = sheet.CreateDrawingPatriarch();
            var comment1 = patriarch.CreateCellComment(new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 2, 4));
#warning 注释作者不显示，且作者显示区域显示的确是内容
            comment1.String = new HSSFRichTextString("这是公司名称");
            comment1.Author = "贾建军";
            cell0.CellComment = comment1;
            #endregion

            #region 页眉页脚 分为左中右三部分
            sheet.Header.Center = "我是中间内容的页眉";
            sheet.Footer.Left = "我是左边的页脚";
            sheet.Footer.Right = "我是右边的页脚";
            #endregion

            #region 单元格格式
            var cell1_3 = row1.CreateCell(3);
            cell1_3.SetCellValue(DateTime.Now);
            cell1_3.CellStyle = GetCellStyleByType(workbook, CellStyleType.DateTime);
            var cell1_4 = row1.CreateCell(4);
            cell1_4.SetCellValue(1.2);
            cell1_4.CellStyle = GetCellStyleByType(workbook, CellStyleType.Float);
            var cell1_5 = row1.CreateCell(5);
            cell1_5.SetCellValue(20000);
            cell1_5.CellStyle = GetCellStyleByType(workbook, CellStyleType.Money);
            #endregion

            #region 单元格边框
            var row2 = sheet.CreateRow(2);
            var cell2_3 = row2.CreateCell(3);
            cell2_3.SetCellValue("我有边框");
            cell2_3.CellStyle = GetCellStyleByType(workbook, CellStyleType.Border);
            #endregion

            return workbook;
        }

        /// <summary>
        /// 根据类型获取单元格样式
        /// </summary>
        /// <param name="cellStyleType"></param>
        /// <returns></returns>
        public static ICellStyle GetCellStyleByType(HSSFWorkbook workbook, CellStyleType cellStyleType)
        {
#warning 检测是否已创建样式
            var cellstyle = workbook.CreateCellStyle();
            switch (cellStyleType)
            {
                case CellStyleType.Header:
                    #region 设置列头单元格样式
                    //创建单元格样式
                    cellstyle.Alignment = HorizontalAlignment.Center;
                    cellstyle.VerticalAlignment = VerticalAlignment.Center;
                    //创建字体
                    var headerFont = workbook.CreateFont();
                    headerFont.IsBold = true;
                    headerFont.FontHeightInPoints = 12;
                    cellstyle.SetFont(headerFont);
                    #endregion
                    break;
                case CellStyleType.Common:
                    cellstyle.Alignment = HorizontalAlignment.Center;
                    cellstyle.VerticalAlignment = VerticalAlignment.Center;
                    cellstyle.WrapText = true;
                    //文本缩进
                    cellstyle.Indention = 3;
                    //文本方向  逆时针旋转 -90~90
                    cellstyle.Rotation = (short)90;
                    break;
                case CellStyleType.DateTime:
                    cellstyle.DataFormat = workbook.CreateDataFormat().GetFormat("yyyy年mm月dd日");
                    break;
                case CellStyleType.Float:
                    //百分比:0.00%   
                    //数字转中文(限整数):[DbNum2][$-804]0
                    //科学计数法：0.00E+00
                    cellstyle.DataFormat = workbook.CreateDataFormat().GetFormat("0.00");
                    break;
                case CellStyleType.Money:
                    cellstyle.DataFormat = workbook.CreateDataFormat().GetFormat("￥#,##0");
                    break;
                case CellStyleType.Border:
                    cellstyle.BorderTop = BorderStyle.Dashed;
                    cellstyle.BorderRight = BorderStyle.Dotted;
                    cellstyle.BorderBottom = BorderStyle.Thin;
                    cellstyle.BorderLeft = BorderStyle.Double;
                    cellstyle.BorderDiagonal = BorderDiagonal.Forward;
                    cellstyle.LeftBorderColor = HSSFColor.Red.Index;
                    break;
            }

            return cellstyle;
        }
    }
}
