using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jaylan.Utilities.Enum
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumTextAttribute : Attribute
    {
        private int _order = -1;
        public EnumTextAttribute(string text)
        {
            Text = text;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// 排序 最小为0 
        /// </summary>
        public int Order
        {
            get
            { return _order; }
            set
            {
                if (value > -1)
                    _order = value;
            }
        }
    }

    public class EnumData
    {
        /// <summary>
        /// 枚举名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 枚举值
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 枚举说明
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }

    public static class EnumHelper
    {
        public static string GetEnumText(
            this ICustomAttributeProvider provider)
        {
            var attributes = (EnumTextAttribute[])provider.GetCustomAttributes(
                typeof(EnumTextAttribute), false);
            return attributes.First().Text;
        }

        public static EnumData GetEnumData(this System.Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var textAttr = Attribute.GetCustomAttribute(fi, typeof(EnumTextAttribute)) as EnumTextAttribute;

            var enumData = new EnumData
            {
                Name = value.ToString(),
                Value = (int)fi.GetValue(value),
                Text = textAttr == null ? value.ToString() : textAttr.Text,
                Order = textAttr?.Order ?? -1
            };

            return enumData;
        }

        public static System.Enum GetEnum(this Type enumType, object value)
        {
            if (!enumType.IsEnum)
                throw new InvalidOperationException();

            return (System.Enum)System.Enum.Parse(enumType, value.ToString(), true);
        }

        public static T GetEnum<T>(this Type enumType, object value)
        {
            if (!enumType.IsEnum)
                throw new InvalidOperationException();

            return (T)Convert.ChangeType(GetEnum(enumType, value), typeof(T));
        }

        public static IEnumerable<EnumData> GetEnumList(this Type enumType)
        {
            if (!enumType.IsEnum)
                throw new InvalidOperationException();
            var enumDataList = new List<EnumData>();
            foreach (int enu in System.Enum.GetValues(enumType))
            {
                var e = GetEnum(enumType, enu);
                //yield return GetEnumData(e);
                enumDataList.Add(GetEnumData(e));
            }
            return enumDataList.Where(q => q.Order != -1).OrderBy(e => e.Order).Concat(
             enumDataList.Where(q => q.Order == -1).OrderBy(e => e.Value)).AsEnumerable();
        }

        public static string GetEnumText(this Type enumType, int value)
        {
            if (!enumType.IsEnum)
                throw new InvalidOperationException();
            var name = System.Enum.GetName(enumType, value);
            if (name == null)
                return null;

            return enumType.GetField(name).GetEnumText();
        }

        public static string GetEnumText(this System.Enum obj, int value)
        {
            var enumType = obj.GetType();
            if (!enumType.IsEnum)
                throw new InvalidOperationException();
            var name = System.Enum.GetName(enumType, value);
            if (name == null)
                return null;

            return enumType.GetField(name).GetEnumText();
        }
    }

    /// <summary>
    /// 逆代成枚举类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumHelper<T> : IEnumerable<T>
    {
        public static IEnumerable<T> AsEnumerable()
        {
            return new EnumHelper<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in System.Enum.GetValues(typeof(T)))
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}