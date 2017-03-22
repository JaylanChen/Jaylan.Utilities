using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jaylan.Utilities.String
{
    /// <summary>
    /// 字符串拓展类
    /// </summary>
    public static class StringHelper
    {
        #region 字符串验证
        /// <summary>
        /// 是否是邮箱地址
        /// </summary>
        /// <param name="email">待检测的字符串</param>
        /// <returns></returns>
        public static bool IsMail(this string email)
        {
            return QuickValidate(@"^[\w\-]+?@[\w\-]+\.[\w\-]+$", email) && email.IndexOf('@') == email.LastIndexOf('@');
        }

        /// <summary>
        /// 是否是手机号
        /// </summary>
        /// <param name="mobile">待检测的字符串</param>
        /// <returns></returns>
        public static bool IsMobile(this string mobile)
        {
            return QuickValidate(@"^1[3578]\d{9}$", mobile);
        }

        /// <summary>
        /// 是否是数字字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string str)
        {
            return QuickValidate(@"^\d+$", str);
        }

        /// <summary>
        /// 判断字符串是否是Null 或者 空字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        #endregion

        #region 字符串截取
        /// <summary>
        /// 从指定位置截取字符串，如果起始位置超出了字符串长度则返回空字符串
        /// </summary>
        /// <param name="str">带截取的字符串</param>
        /// <param name="start">截取的开始位置(从0开始)</param>
        /// <returns>返回截取的字符串</returns>
        public static string SubStr(this string str, int start)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            var value = string.Empty;
            if (str.Length > start)
            {
                value = value.Substring(start);
            }
            return value;
        }
        /// <summary>
        /// 从指定的位置截取指定长度的字符串
        /// </summary>
        /// <param name="str">带截取的字符串</param>
        /// <param name="start">截取的开始位置(从0开始)</param>
        /// <param name="len">截取的结束位置(从0开始)</param>
        /// <returns>返回截取的字符串</returns>
        public static string SubStr(this string str, int start, int len)
        {
            var value = str.SubStr(start);
            if (value.Length > len)
                value = value.Substring(0, len);
            return value;
        }
        #endregion

        #region 字符串分割
        /// <summary>
        /// 把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrArray(string str, char speater, bool toLower)
        {
            var list = new List<string>();
            var ss = str.Split(speater);
            foreach (var s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    var strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }
        #endregion

        #region 字符编码转换
        /// <summary>
        /// 将一个字符串从一个编码转换为另一个编码
        /// </summary>
        /// <param name="str">待处理字符串</param>
        /// <param name="lastEncoding">转换前编码</param>
        /// <param name="nextEncoding">转换后编码</param>
        /// <returns></returns>
        public static string ChangeEn(string str, Encoding lastEncoding, Encoding nextEncoding)
        {
            var byteStr = lastEncoding.GetBytes(str);
            byteStr = Encoding.Convert(lastEncoding, nextEncoding, byteStr);
            var newStr = nextEncoding.GetString(byteStr);
            return newStr;
        }
        #endregion

        #region 全角半角转换
        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSBC(string str)
        {
            //半角转全角：
            var c = str.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="str">输入</param>
        /// <returns></returns>
        public static string ToDBC(string str)
        {
            var c = str.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        #endregion

        #region HTML转行成TEXT
        /// <summary>
        /// HTML转行成TEXT
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string HtmlToTxt(string strHtml)
        {
            string[] aryReg ={
            @"<script[^>]*?>.*?</script>",
            @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
            @"([\r\n])[\s]+",
            @"&(quot|#34);",
            @"&(amp|#38);",
            @"&(lt|#60);",
            @"&(gt|#62);",
            @"&(nbsp|#160);",
            @"&(iexcl|#161);",
            @"&(cent|#162);",
            @"&(pound|#163);",
            @"&(copy|#169);",
            @"&#(\d+);",
            @"-->",
            @"<!--.*\n"
            };
            //string strOutput = strHtml;
            //for (int i = 0; i < aryReg.Length; i++)
            //{
            //    Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
            //    strOutput = regex.Replace(strOutput, string.Empty);
            //}
            var strOutput = aryReg.Select(t => new Regex(t, RegexOptions.IgnoreCase)).Aggregate(strHtml, (current, regex) => regex.Replace(current, string.Empty));

            strOutput = strOutput.Replace("<", "");
            strOutput = strOutput.Replace(">", "");
            strOutput = strOutput.Replace("\r\n", "");


            return strOutput;
        }
        #endregion

        #region 得到字符串字节长度
        /// <summary>
        /// 得到字符串字节长度
        /// </summary>
        /// <param name="str">参数字符串</param>
        /// <param name="encoding">以什么编码计算</param>
        /// <returns></returns>
        public static int StrLength(string str, Encoding encoding)
        {
            var tempLen = 0;
            var s = encoding.GetBytes(str);
            foreach (var b in s)
            {
                if ((int)b == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }
        #endregion

        #region SQL语句字符串安全处理
        /// <summary>
        /// SQL语句字符串安全处理，防止非法注入
        /// </summary>
        /// <param name="String">字符串</param>
        /// <param name="isDel">是删除还是替换，true表示删除，false表示转换</param>
        /// <returns></returns>
        public static string SqlSafeString(string String, bool isDel)
        {
            if (isDel)
            {
                String = String.Replace("'", "");
                String = String.Replace("\"", "");
                return String;
            }
            String = String.Replace("'", "&#39;");
            String = String.Replace("\"", "&#34;");
            return String;
        }
        #endregion

        #region 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。
        /// <summary>
        /// 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。(0除外)
        /// </summary>
        /// <param name="value">需验证的字符串。。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsNumberId(string value)
        {
            return QuickValidate("^[1-9]*[0-9]*$", value);
        }
        #endregion

        #region 快速验证一个字符串是否符合指定的正则表达式。
        /// <summary>
        /// 快速验证一个字符串是否符合指定的正则表达式。
        /// </summary>
        /// <param name="express">正则表达式的内容。</param>
        /// <param name="value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool QuickValidate(string express, string value)
        {
            if (value == null)
                return false;
            var myRegex = new Regex(express);
            if (value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(value);
        }
        #endregion
    }
}
