using System;

namespace Jaylan.Utilities
{
    public class TimeStampHelper
    {
        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <returns></returns>
        public static long ConvertToTimeStamp()
        {
            return ConvertToTimeStamp(DateTime.Now);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static long ConvertToTimeStamp(DateTime time)
        {
            var result = (time.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return result;
        }
    }
}
