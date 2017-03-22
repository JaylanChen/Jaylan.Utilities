using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Jaylan.Utilities.Excel
{

    #region ExcelHelper辅助类
    /// <summary>
    /// Office文件格式
    /// </summary>
    public enum OfficeType
    {
        /// <summary>
        /// 97-2003格式
        /// </summary>
        [Description("Office2003")]
        Office2003,
        /// <summary>
        /// 2007+格式
        /// </summary>
        [Description("Office2007")]
        Office2007
    }


    /// <summary>
    /// 导出列接口
    /// </summary>
    /// <typeparam name="T">数据行类型</typeparam>
    public interface IExportColumn<in T>
    {
        /// <summary>
        /// 列标题
        /// </summary>
        string Title { get; }
        /// <summary>
        /// 获取该列的值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        Object GetValue(T row, Int32 index);
    }

    /// <summary>
    /// 导出列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExportColumn<T> : IExportColumn<T>
    {
        public ExportColumn(string title, Func<T, Int32, Object> funcGetValue)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentNullException(nameof(title));
            if (funcGetValue == null) throw new ArgumentNullException("funcGetValue");
            Title = title;
            _funcGetValue = funcGetValue;
        }

        private readonly Func<T, int, object> _funcGetValue;
        public string Title { get; }

        public object GetValue(T row, int index)
        {
            return _funcGetValue(row, index);
        }
    }
    #endregion





    /// <summary>
    /// Excel读写帮助类
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// 格式化Excel文件名，根据Excel类型，为Excel增加后缀。
        /// </summary>
        /// <param name="fileName">未格式化的文件名</param>
        /// <param name="officeType">Excel类型</param>
        /// <returns>格式化后的Excel文件名。</returns>
        public static string FormatFileName(string fileName, OfficeType officeType)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            var ext = officeType == OfficeType.Office2007 ? ".xlsx" : ".xls";
            var name = fileName;
            if (!fileName.EndsWith(ext, StringComparison.CurrentCultureIgnoreCase))
            {
                name += ext;
            }
            return name;
        }

        #region 导出

        #region 私有

        private static void SetCellValue(ICell cell, Object value)
        {
            if (value == null || value == DBNull.Value)
            {
                cell.SetCellValue(string.Empty);
                return;
            }
            var str = value as string;
            if (str != null)
            {
                cell.SetCellValue(str);
                cell.SetCellType(CellType.String);
            }
            else if (value is DateTime)
            {
                if ((DateTime)value != default(DateTime))
                {
                    cell.SetCellValue((DateTime)value);
                }
            }
            else if (value is Boolean)
            {
                cell.SetCellValue((Boolean)value);
                cell.SetCellType(CellType.Boolean);
            }
            else if (value is Int16 || value is Int32 || value is Int64 || value is Byte || value is Decimal || value is float || value is Double)
            {
                cell.SetCellValue(Convert.ToDouble(value));
                cell.SetCellType(CellType.Numeric);
            }
            else
            {
                cell.SetCellValue(value.ToString());
            }
        }

        private static IWorkbook CreateWorkbook(OfficeType excelType)
        {
            if (excelType == OfficeType.Office2007)
            {
                return new XSSFWorkbook();
            }
            return new HSSFWorkbook();
        }
        #endregion

        /// <summary>
        /// 导出Excel,如果Excel类型为Office2003，那么数据行数不能超过65535，如果超过，则会被拆分到多个工作区中。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="dataSource">数据源</param>
        /// <param name="excelType">EXCEL格式</param>
        /// <param name="sheetName">工作区名称</param>
        /// <param name="saveStream">保存到的文件流</param>
        /// <param name="columns">导出列</param>
        public static void ExportExcel<T>(IList<T> dataSource, OfficeType excelType, System.String sheetName, Stream saveStream, IList<IExportColumn<T>> columns)
        {
            if (dataSource == null)
                throw new ArgumentNullException("dataSource");
            if (string.IsNullOrEmpty(sheetName))
                throw new ArgumentNullException("sheetName");
            if (saveStream == null)
                throw new ArgumentNullException("saveStream");
            if (columns == null)
                throw new ArgumentNullException("columns");

            //预定义
            var book = CreateWorkbook(excelType);
            var encode = Encoding.GetEncoding(936);
            //样式
            var headerCellStyle = book.CreateCellStyle();
            headerCellStyle.Alignment = HorizontalAlignment.Center;
            var headerFont = book.CreateFont();
            headerFont.FontHeightInPoints = 10;
            headerFont.Boldweight = 700;
            headerCellStyle.SetFont(headerFont);
            var dateStyle = book.CreateCellStyle();
            var format = book.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd HH:mm:ss");

            //Sheet计算
            int sheetDataCount;
            switch (excelType)
            {
                case OfficeType.Office2003:
                    sheetDataCount = 65534;
                    break;
                case OfficeType.Office2007:
                    sheetDataCount = 1048574;
                    break;
                default:
                    sheetDataCount = 65534;
                    break;
            }
            var sheetList = new List<ISheet>();
            if (dataSource.Count > sheetDataCount)
            {
                var sheetCount = dataSource.Count / sheetDataCount;
                if (dataSource.Count % sheetDataCount > 0)
                {
                    sheetCount++;
                }
                for (int i = 0; i < sheetCount; i++)
                {
                    sheetList.Add(book.CreateSheet(System.String.Format("{0}({1})", sheetName, i + 1)));
                }
            }
            else
            {
                sheetList.Add(book.CreateSheet(sheetName));
            }
            //填充数据
            var rowIndex = 0;
            var sheetIndex = 0;
            var arrColumnWidth = new Int32[] { };
            foreach (var row in dataSource)
            {
                var sheet = sheetList[sheetIndex];

                //写列头
                if (rowIndex == 0)
                {
                    arrColumnWidth = new int[columns.Count];
                    var headerRow = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < columns.Count; i++)
                    {
                        var column = columns[i];
                        headerRow.CreateCell(i).SetCellValue(column.Title);
                        arrColumnWidth[i] = encode.GetByteCount(column.Title);
                    }
                }
                //写内容

                var rowData = sheet.CreateRow(rowIndex + 1);
                for (int i = 0; i < columns.Count; i++)
                {
                    var column = columns[i];
                    var cell = rowData.CreateCell(i);
                    var data = column.GetValue(row, i);
                    SetCellValue(cell, data);
                    if (data != null && data != DBNull.Value)
                    {
                        var str = data.ToString();//转换为字符串
                        var cellWidth = encode.GetByteCount(str);
                        if (cellWidth > arrColumnWidth[i])
                        {
                            arrColumnWidth[i] = cellWidth;
                        }
                        if (data is DateTime)
                        {
                            cell.CellStyle = dateStyle;
                        }
                    }
                }

                rowIndex++;
                if (rowIndex > sheetDataCount)
                {
                    //设置列样式
                    for (int i = 0; i < columns.Count; i++)
                    {
                        var cell = sheet.GetRow(0).GetCell(i);
                        cell.CellStyle = headerCellStyle;
                        sheet.SetColumnWidth(i, (arrColumnWidth[i] + 1) * 256);
                    }
                    rowIndex = 0;
                    sheetIndex++;
                }
            }
            if (sheetList.Count > 0)
            {
                //针对最后一个sheet设置样式
                var sheet = sheetList[sheetList.Count - 1];
                for (int i = 0; i < columns.Count; i++)
                {
                    var cell = sheet.GetRow(0).GetCell(i);
                    cell.CellStyle = headerCellStyle;
                    sheet.SetColumnWidth(i, (arrColumnWidth[i] + 1) * 256);
                }
            }
            book.Write(saveStream);
        }
        #endregion

        #region 导入

        private static Object GetCellValue(ICell cell)
        {
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Formula:
                    try
                    {
                        var e = WorkbookFactory.CreateFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch (Exception)
                    {
                        return cell.NumericCellValue;
                    }
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue;
                    }
                    return cell.NumericCellValue;
                default:
                    return cell.ToString();
            }
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DataSet ImportExcel(Stream fileStream)
        {
            if (fileStream == null) throw new ArgumentNullException("fileStream");
            var ds = new DataSet();
            var book = WorkbookFactory.Create(fileStream);
            //遍历工作区
            for (int s = 0; s < book.NumberOfSheets; s++)
            {
                var sheet = book.GetSheetAt(s);
                if (sheet.LastRowNum == 0) continue;//如果一行数据都没有，说明这个Sheet也许根本就不存在啊。。谁给搞得啊。真是的。
                var dt = new DataTable(sheet.SheetName);
                //表定义
                var headerRow = sheet.GetRow(0);
                var rowCount = sheet.LastRowNum;
                var cellCount = headerRow.LastCellNum;
                for (int c = headerRow.FirstCellNum; c < cellCount; c++)
                {
                    var dc = new DataColumn(GetCellValue(headerRow.GetCell(c)).ToString());
                    dt.Columns.Add(dc);
                }
                //表数据
                for (int r = (sheet.FirstRowNum + 1); r <= rowCount; r++)
                {
                    var row = sheet.GetRow(r);
                    if (row == null) continue;
                    var dr = dt.NewRow();
                    for (int i = row.FirstCellNum; i < cellCount; i++)
                    {
                        var cell = row.GetCell(i);
                        if (cell != null)
                        {
                            dr[i] = GetCellValue(cell);
                        }
                    }
                    dt.Rows.Add(dr);
                }
                ds.Tables.Add(dt);
            }
            return ds;
        }
        #endregion
    }
}
