using System;
using System.Collections.Generic;

namespace Jaylan.Utilities.Random
{
    /// <summary>
    /// 使用Random类生成伪随机数
    /// </summary>
    public class RandomHelper
    {
        //随机数对象
        private static readonly System.Random HelperRandom = new System.Random(~unchecked((int)DateTime.Now.Ticks));

        #region 对一个数组进行随机排序
        /// <summary>
        /// 对一个数组进行随机排序
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="arr">需要随机排序的数组</param>
        public static void GetRandomArray<T>(T[] arr)
        {
            //对数组进行随机排序的算法:随机选择两个位置，将两个位置上的值交换

            //交换的次数,这里使用数组的长度作为交换次数
            var count = arr.Length;

            //开始交换
            for (var i = 0; i < count; i++)
            {
                //生成两个随机数位置
                var randomNum1 = GetRandomInt(0, arr.Length);
                var randomNum2 = GetRandomInt(0, arr.Length);

                //定义临时变量

                //交换两个随机数位置的值
                var temp = arr[randomNum1];
                arr[randomNum1] = arr[randomNum2];
                arr[randomNum2] = temp;
            }
        }

        /// <summary>
        /// 随机生成不重复数字字符串
        /// </summary>
        /// <param name="codeCount">字符串长度</param>
        /// <returns></returns>
        public static string GenerateCheckCodeNum(int codeCount)
        {
            var rep = 0;
            var str = string.Empty;
            var num2 = DateTime.Now.Ticks + rep;
            rep++;
            var random = new System.Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (var i = 0; i < codeCount; i++)
            {
                var num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }

        /// <summary>
        /// 随机生成字符串（数字和字母混和）
        /// </summary>
        /// <param name="codeCount">字符串长度</param>
        /// <returns></returns>
        public static string GenerateCheckCode(int codeCount)
        {
            var rep = 0;
            var str = string.Empty;
            var num2 = DateTime.Now.Ticks + rep;
            rep++;
            var random = new System.Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (var i = 0; i < codeCount; i++)
            {
                char ch;
                var num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        #endregion

        #region 产生随机整数
        /// <summary>
        /// 产生随机数，100000~999999之间
        /// 包含100000不包含999999
        /// </summary>
        /// <returns>随机数</returns>
        public static int GetRandomInt()
        {
            const int minimum = 100000;
            const int maximal = 999999;
            return GetRandomInt(minimum, maximal);
        }

        /// <summary>
        /// 生成一个指定范围的随机整数，该随机数范围包括最小值，但不包括最大值
        /// </summary>
        /// <param name="minNum">最小值</param>
        /// <param name="maxNum">最大值</param>
        public static int GetRandomInt(int minNum, int maxNum)
        {
            return HelperRandom.Next(minNum, maxNum);
        }

        /// <summary>
        /// 生成一个指定范围的随机整数List，该随机数范围包括最小值，但不包括最大值
        /// </summary>
        /// <param name="count">List 大小</param>
        /// <param name="minNum">最小值</param>
        /// <param name="maxNum">最大值</param>
        public static IEnumerable<int> GetRandomIntList(int count, int minNum, int maxNum)
        {
            var numList = new List<int>();
            while (numList.Count < count)
            {
                var num = GetRandomInt(minNum, maxNum);
                if (!numList.Contains(num))
                {
                    numList.Add(num);
                }
            }
            return numList;
        }

        #endregion

        #region 生成一个0.0到1.0的随机小数
        /// <summary>
        /// 生成一个0.0到1.0的随机小数
        /// </summary>
        public static double GetRandomDouble()
        {
            return HelperRandom.NextDouble();
        }
        #endregion

        #region 生成数字的随机字符串
        /// <summary>
        /// 生成数字的随机字符串
        /// </summary>
        /// <param name="length">生成长度</param>
        public static string GetNumberString(int length)
        {
            return GetNumberString(length, false);
        }

        /// <summary>
        /// 生成数字的随机字符串
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string GetNumberString(int length, bool sleep)
        {
            if (sleep) System.Threading.Thread.Sleep(5);
            var result = "";
            for (var i = 0; i < length; i++)
            {
                result += HelperRandom.Next(10).ToString();
            }
            return result;
        }
        #endregion

        #region 生成字母与数字的随机字符串
        /// <summary>
        /// 生成字母与数字的随机字符串
        /// </summary>
        /// <param name="length">生成长度</param>
        public static string GetString(int length)
        {
            return GetString(length, false);
        }

        /// <summary>
        /// 生成字母与数字的随机字符串
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string GetString(int length, bool sleep)
        {
            if (sleep) System.Threading.Thread.Sleep(3);
            var pattern = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            var result = "";
            var n = pattern.Length;
            for (var i = 0; i < length; i++)
            {
                var rnd = HelperRandom.Next(0, n);
                result += pattern[rnd];
            }
            return result;
        }
        #endregion

        #region 生成纯字母的随机字符串
        /// <summary>
        /// 生成纯字母的随机字符串
        /// </summary>
        /// <param name="length">生成长度</param>
        public static string GetCharString(int length)
        {
            return GetCharString(length, false);
        }

        /// <summary>
        /// 生成纯字母的随机字符串
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string GetCharString(int length, bool sleep)
        {
            if (sleep) System.Threading.Thread.Sleep(3);
            var pattern = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            var result = "";
            var n = pattern.Length;
            for (var i = 0; i < length; i++)
            {
                var rnd = HelperRandom.Next(0, n);
                result += pattern[rnd];
            }
            return result;
        }
        #endregion
    }
}
