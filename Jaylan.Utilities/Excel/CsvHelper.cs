using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Jaylan.Utilities.Excel
{

    #region 辅助类
    ///// <summary>
    ///// 导出列
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public class ExportColumn<T> : IExportColumn<T>
    //{
    //    public ExportColumn(string title, Func<T, Int32, Object> funcGetValue)
    //    {
    //        if (String.IsNullOrEmpty(title)) throw new ArgumentNullException("title");
    //        if (funcGetValue == null) throw new ArgumentNullException("funcGetValue");
    //        this.Title = title;
    //        this._funcGetValue = funcGetValue;
    //    }

    //    private readonly Func<T, Int32, Object> _funcGetValue;
    //    public string Title { get; private set; }

    //    public object GetValue(T row, int index)
    //    {
    //        return this._funcGetValue(row, index);
    //    }
    //}

    internal interface IStringWriter
    {
        void WriteLine();
        void Write(System.String str);
        void Write(char c);
    }


    internal class StringWriterWrapper : IStringWriter
    {
        public StringWriterWrapper(StringWriter stringWriter)
        {
            StringWriter = stringWriter;
        }

        public StringWriter StringWriter { get; set; }

        public void WriteLine()
        {
            StringWriter.WriteLine();
        }

        public void Write(string str)
        {
            StringWriter.Write(str);
        }

        public void Write(char c)
        {
            StringWriter.Write(c);
        }
    }

    internal class StringBuilderWrapper : IStringWriter
    {
        public StringBuilderWrapper(StringBuilder stringBuilder)
        {
            StringBuilder = stringBuilder;
        }

        public StringBuilder StringBuilder { get; set; }

        public void WriteLine()
        {
            StringBuilder.AppendLine();
        }

        public void Write(string str)
        {
            StringBuilder.Append(str);
        }

        public void Write(char c)
        {
            StringBuilder.Append(c);
        }
    }

    internal class StreamWriterWrapper : IStringWriter
    {
        public StreamWriterWrapper(StreamWriter streamWriter)
        {
            StreamWriter = streamWriter;
        }

        public StreamWriter StreamWriter { get; set; }
        public void WriteLine()
        {
            StreamWriter.WriteLine();
        }

        public void Write(string str)
        {
            StreamWriter.Write(str);
        }

        public void Write(char c)
        {
            StreamWriter.Write(c);
        }
    }
    #endregion


    /// <summary>
    /// 提供CSV格式文件的帮助
    /// </summary>
    public static class CsvHelper
    {
        /// <summary>
        /// 格式化字段
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string FormatField(Object field)
        {
            if (field is string)
            {
                var str = field as string;
                var wapper = false;
                if (str.IndexOf('\"') != -1)
                {
                    str = str.Replace("\"", "\"\"");
                    wapper = true;
                }
                if (str.IndexOf(',') != -1)
                {
                    wapper = true;
                }
                if (str.IndexOf('\n') != -1)
                {
                    wapper = true;
                }
                return wapper ? $"\"{str}\"" : str;
            }
            return Convert.ToString(field);
        }

        private static void ExportCsv<T>(IEnumerable<T> dataSource, IStringWriter writer, IEnumerable<IExportColumn<T>> columns)
        {
            //检测
            if (dataSource == null) throw new ArgumentNullException("dataSource");
            if (writer == null) throw new ArgumentNullException("writer");
            if (columns == null) throw new ArgumentNullException("columns");

            //变量
            var arrColumn = columns.ToArray();
            //写标题
            for (var i = 0; i < arrColumn.Length; i++)
            {
                writer.Write(FormatField(arrColumn[i].Title));
                if (i < arrColumn.Length - 1)
                {
                    writer.Write(',');
                }
            }
            writer.WriteLine();
            //写内容
            var index = 0;
            foreach (var item in dataSource)
            {
                for (var i = 0; i < arrColumn.Length; i++)
                {
                    var val = arrColumn[i].GetValue(item, index);
                    writer.Write(FormatField(val));
                    if (i < arrColumn.Length - 1)
                    {
                        writer.Write(',');
                    }
                }
                writer.WriteLine();
                index++;
            }
        }

        /// <summary>
        /// 导出CSV字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static string ExportToCsvString<T>(IEnumerable<T> dataSource, IEnumerable<IExportColumn<T>> columns)
        {
            var sb = new StringBuilder();
            var wrapper = new StringBuilderWrapper(sb);
            ExportCsv(dataSource, wrapper, columns);
            return sb.ToString();
        }

        /// <summary>
        /// 导出CSV到数据流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="saveStream"></param>
        /// <param name="encoding"></param>
        /// <param name="columns"></param>
        public static void ExportCsv<T>(IEnumerable<T> dataSource, Stream saveStream, Encoding encoding, IEnumerable<IExportColumn<T>> columns)
        {
            var sw = new StreamWriter(saveStream, encoding);
            var wrapper = new StreamWriterWrapper(sw);
            ExportCsv(dataSource, wrapper, columns);
        }


        /// <summary>
        /// 导出报表为Csv
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="strFilePath">物理路径</param>
        /// <param name="tableheader">表头</param>
        /// <param name="columname">字段标题,逗号分隔</param>
        public static bool DtToCsv(DataTable dt, string strFilePath, string tableheader, string columname)
        {
            try
            {
                var strmWriterObj = new StreamWriter(strFilePath, false, Encoding.UTF8);
                strmWriterObj.WriteLine(tableheader);
                strmWriterObj.WriteLine(columname);
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var strBufferLine = "";
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j > 0)
                            strBufferLine += ",";
                        strBufferLine += dt.Rows[i][j].ToString();
                    }
                    strmWriterObj.WriteLine(strBufferLine);
                }
                strmWriterObj.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将Csv读入DataTable
        /// </summary>
        /// <param name="filePath">csv文件路径</param>
        /// <param name="n">表示第n行是字段title,第n+1行是记录开始</param>
        /// <param name="dt"></param>
        public static DataTable CsvToDt(string filePath, int n, DataTable dt)
        {
            var reader = new StreamReader(filePath, Encoding.UTF8, false);
            var m = 0;
            reader.Peek();
            while (reader.Peek() > 0)
            {
                m = m + 1;
                var str = reader.ReadLine();
                if (m >= n + 1)
                {
                    var split = str.Split(',');

                    var dr = dt.NewRow();
                    int i;
                    for (i = 0; i < split.Length; i++)
                    {
                        dr[i] = split[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

    }
}

