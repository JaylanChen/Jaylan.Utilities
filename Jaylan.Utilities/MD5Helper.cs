using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Jaylan.Utilities
{
    public class MD5Helper
    {
        /// <summary>
        /// 计算传入字节数组的MD5值
        /// </summary>
        /// <param name="bs">字节数组</param>
        /// <returns>返回计算好指定字节数组的MD5值</returns>
        public static string GetBytesMD5(byte[] bs)
        {
            var sb = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                bs = md5.ComputeHash(bs);
                foreach (var b in bs)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 计算传入字节数组的offset偏移量的、count长度的MD5值
        /// </summary>
        /// <param name="bs">字节数组</param>
        /// <param name="offset">偏移量</param>
        /// <param name="count">长度</param>
        /// <returns>返回计算好指定字节数组的MD5值</returns>
        public static string GetBytesMD5(byte[] bs, int offset, int count)
        {
            var sb = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                bs = md5.ComputeHash(bs, offset, count);
                foreach (var b in bs)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 计算传入过来的流的MD5值
        /// </summary>
        /// <param name="st">流文件</param>
        /// <returns>返回计算好流的MD5值</returns>
        public static string GetStreamMD5(Stream st)
        {
            var sb = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                var bs = md5.ComputeHash(st);
                foreach (var b in bs)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 计算一个字符串的MD5值
        /// </summary>
        /// <param name="str">需要计算的字符串</param>
        /// <returns>返回计算好字符串的MD5值</returns>
        public static string GetStringMD5(string str)
        {
            var sb = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                var bs = Encoding.UTF8.GetBytes(str);
                bs = md5.ComputeHash(bs);
                foreach (var b in bs)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="path">传入需要计算MD5值的文件路径</param>
        /// <returns>返回计算好的文件的MD5</returns>
        public static string GetFileMD5(string path)
        {
            var sb = new StringBuilder();
            using (Stream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var md5 = MD5.Create())
                {
                    var bs = md5.ComputeHash(fs);
                    foreach (var b in bs)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 计算一个字符串的16位MD5值
        /// </summary>
        /// <param name="str">需要计算的字符串</param>
        /// <returns></returns>
        public static string GetStringMD5_16(string str)
        {
            var md532 = GetStringMD5(str);
            return md532.Substring(8, 16);
        }
    }
}
