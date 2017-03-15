using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Web;
using Aspose.Cells;

namespace Tmc.Util.Excel
{
    /// <summary>
    /// Excel导入导出辅助方法：使用aspose.cells
    /// </summary>
    public static class ExcelUtil
    {
        public static void ExportToExcel<T>(List<T> list, ExportColumn[] columns, string fileName)
        {
            var book = new Workbook();
            var sheet = book.Worksheets[0];

            //设置标题
            SetTitles(sheet, columns);

            //设置内容
            SetContent(sheet, list, columns);

            //显示表格线并自适应
            sheet.IsGridlinesVisible = true;
            sheet.AutoFitColumns();

            //输出到Http响应流
            book.Save(HttpContext.Current.Response, HttpUtility.UrlEncode(fileName, Encoding.UTF8), ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Xlsx));
        }

        private static void SetTitles(Worksheet sheet, ExportColumn[] columns)
        {
            var titleStyle = GetTitleStyle(sheet.Workbook);

            for (var col = 0; col < columns.Length; col++)
            {
                sheet.Cells[0, col].PutValue(columns[col].Title);
                sheet.Cells[0, col].SetStyle(titleStyle);
            }
            sheet.Cells.SetRowHeightPixel(0, 30);
            //sheet.FreezePanes(1, 1, 1, 0);
        }
                
        private static void SetContent<T>(Worksheet sheet, List<T> list, ExportColumn[] columns)
        {
            var cellStyle = GetCellStyle(sheet.Workbook);
            
            for (var i = 0; i < list.Count; i++)
            {
                int row = i + 1;
                for (var col = 0; col < columns.Length; col++)
                {
                    sheet.Cells.SetRowHeight(row, 20);
                    sheet.Cells[row, col].SetStyle(cellStyle);
                    sheet.Cells[row, col].PutValue(GetPropertyValue(list[i], columns[col].Property));
                }
            }
        }

        private static Style GetTitleStyle(Workbook book)
        {
            var style = book.CreateStyle();

            style.Name = "Title";
            style.Font.Size = 10;
            style.Font.Color = Color.Black;

            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            
            style.ForegroundColor = Color.FromArgb(255, 204, 153);
            style.Pattern = BackgroundType.Solid;
            style.HorizontalAlignment = TextAlignmentType.Center;
            return style;
        }

        private static Style GetCellStyle(Workbook book)
        {
            var style = book.CreateStyle();

            style.Name = "Cell";
            style.Font.Size = 9;

            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;

            style.Pattern = BackgroundType.Solid;
            return style;
        }

        private static object GetPropertyValue(object obj, string propertyName)
        {
            // 约0.8微秒 string最快，int稍慢,Date最慢
            var o = obj.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, obj, null);

            if (o == null) return null;

            if (o is DateTime)
            {
                var dt = (DateTime)o;
                return dt == default(DateTime) ? string.Empty : dt.ToDateTimeString();
            }
            else
            {
                return o;
            }
        }
    }
}
