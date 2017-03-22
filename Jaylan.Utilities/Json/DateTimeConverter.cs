using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jaylan.Utilities.Json
{
    /// <summary>
    /// 重写Newtonsoft.Json 的时间转换方法
    /// </summary>
    public class UnixDateTimeConverter : DateTimeConverterBase
    {

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception($"日期格式错误,got {reader.TokenType}.");
            }
            var ticks = (long)reader.Value;
            var date = new DateTime(1970, 1, 1);
            date = date.AddSeconds(ticks);
            return date;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long ticks;
            if (value is DateTime)
            {
                var epoc = new DateTime(1970, 1, 1);
                var delta = ((DateTime)value) - epoc;
                if (delta.TotalSeconds < 0)
                {
                    throw new ArgumentOutOfRangeException("时间格式错误.1");
                }
                ticks = (long)delta.TotalSeconds;
            }
            else
            {
                throw new Exception("时间格式错误.2");
            }
            writer.WriteValue(ticks);
        }
    }


    public class NetDateTimeConverter : DateTimeConverterBase
    {

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception($"日期格式错误,got {reader.TokenType}.");
            }
            var ticks = (long)reader.Value;
            var date = new DateTime(1970, 1, 1);
            date = date.AddSeconds(ticks);
            return date;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string ticks;
            if (value is DateTime)
            {
                ticks = Convert.ToDateTime(value).ToString("yyyy-MM-dd hh:mm:ss");
            }
            else
            {
                throw new Exception("时间格式错误.2");
            }
            writer.WriteValue(ticks);
        }
    }
}
