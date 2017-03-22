using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Jaylan.Utilities.Encrypt
{
    public class EncryptHelper
    {
        /// <summary>
        /// BASE64 加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <param name="encodingType"></param>
        /// <returns></returns>
        public static string EncodeBase64(string str, string encodingType = "utf-8")
        {
            string encode;
            var bytes = Encoding.GetEncoding(encodingType).GetBytes(str);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = str;
            }
            return encode;
        }

        /// <summary>
        /// BASE64 解密
        /// </summary>
        /// <param name="code">待解密字符串</param>
        /// <param name="encodingType"></param>
        /// <returns></returns>
        public static string DecodeBase64(string code, string encodingType = "utf-8")
        {
            string decode;
            var bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding(encodingType).GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }


        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <returns></returns>
        public static string Sha256Encrypt(string str)
        {
            var sha256 = new SHA256Managed();
            var hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
            return hashValue.Aggregate("", (current, x) => current + string.Format("{0:x2}", x));
        }

        /// <summary>
        /// SHA512加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <returns></returns>
        public static string Sha512Encrypt(string str)
        {
            var hashString = new SHA512Managed();
            var hashValue = hashString.ComputeHash(Encoding.UTF8.GetBytes(str));
            return hashValue.Aggregate("", (current, x) => current + string.Format("{0:x2}", x));
        }

        /// <summary>
        /// HMACSHA256 加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <param name="key">加密Key</param>
        /// <returns></returns>
        public static string HMACSHA256Encrypt(string str, string key)
        {
            var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hashValue = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(str));
            return hashValue.Aggregate("", (current, x) => current + string.Format("{0:x2}", x));
        }

        /// <summary>
        /// HMACSHA512 加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <param name="key">加密Key</param>
        /// <returns></returns>
        public static string HMACSHA512Encrypt(string str, string key)
        {
            var hmacsha512 = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hashValue = hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(str));
            return hashValue.Aggregate("", (current, x) => current + string.Format("{0:x2}", x));
        }
    }
}
