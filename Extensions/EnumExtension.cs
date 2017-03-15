using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Tmc.Util
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class AttachDataAttribute : Attribute
    {
        public AttachDataAttribute(object key, object value)
        {
            Key = key;
            Value = value;
        }

        public object Key { get; private set; }

        public object Value { get; private set; }
    }

    /// <summary>
    /// 表示枚举项的中文显示文本
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class CnTextAttribute : AttachDataAttribute
    {
        public CnTextAttribute(string cnText)
            : base("CnText", cnText)
        {
        }
    }

    /// <summary>
    /// 表示枚举项的英文显示文本
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnTextAttribute : AttachDataAttribute
    {
        public EnTextAttribute(string enText)
            : base("EnText", enText)
        {
        }
    }

    /// <summary>
    /// 表示枚举项的值
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumValueAttribute : AttachDataAttribute
    {
        public EnumValueAttribute(string enumValue)
            : base("EnumValue", enumValue)
        {
        }
    }

    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举项的中文显示文本
        /// </summary>
        public static string GetCnText(this Enum value)
        {
            return value == null ? string.Empty : GetEnumAttachedText(value, "CnText");
        }

        /// <summary>
        /// 获取枚举项的英文显示文本
        /// </summary>
        public static string GetEnText(this Enum value)
        {
            return value == null ? string.Empty : GetEnumAttachedText(value, "EnText");
        }


        /// <summary>
        /// 获取枚举项的值(如果定义了EnumValue标记，则返回EnumValue定义的值，否则返回枚举项的字符串定义)
        /// </summary>
        public static string GetEnumValue(this Enum value)
        {
            string result = GetEnumAttachedText(value, "EnumValue");
            return string.IsNullOrEmpty(result) ? value.ToString() : result;
        }

        private static string GetEnumAttachedText(Enum value, string key)
        {
            FieldInfo enumField = value.GetType().GetField(value.ToString());
            if (enumField == null)
                throw new ArgumentException(string.Format("参数{0}不是枚举类型{1}的有效值", value, value.GetType()), "value");

            var attributes = (AttachDataAttribute[])enumField.GetCustomAttributes(typeof(AttachDataAttribute), false);
            foreach (AttachDataAttribute item in attributes)
            {
                if (item.Key.Equals(key))
                    return (string)item.Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取枚举项的界面上的显示文本
        /// </summary>
        public static string GetDisplayName(this Enum value)
        {
            if (value == null) return string.Empty;
            
            FieldInfo enumField = value.GetType().GetField(value.ToString());
            if (enumField == null)
                throw new ArgumentException(string.Format("参数{0}不是枚举类型{1}的有效值", value, value.GetType()), "value");

            var attribute = enumField.GetCustomAttribute<DisplayAttribute>(false);
            if (attribute == null) return string.Empty;

            return attribute.Name;
        }
    }

    public static class EnumUtil
    {
        /// <summary>
        /// 通过枚举的值("EnumValue标记的值")来获取枚举对象,如果给定的字符串无效，则抛出异常
        /// </summary>
        public static T ParseByEnumValue<T>(string enumValue) where T : struct
        {
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                object enumObj = Enum.ToObject(typeof(T), i);

                if (((Enum)enumObj).GetEnumValue() == enumValue)
                {
                    return (T)enumObj;
                }
            }
            throw new ArgumentOutOfRangeException("enumValue", "无效的enumValue:" + enumValue);
        }

        /// <summary>
        /// 通过枚举CnText标记的值来获取枚举对象,如果给定的字符串无效，则抛出异常
        /// </summary>
        public static T ParseByCnText<T>(string enumCnText) where T : struct
        {
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                object enumObj = Enum.ToObject(typeof(T), i);

                if (((Enum)enumObj).GetCnText() == enumCnText)
                {
                    return (T)enumObj;
                }
            }
            throw new ArgumentOutOfRangeException("enumCnText", "无效的enumCnText:" + enumCnText);
        }

        /// <summary>
        /// 通过枚举的名称("Enum的名称或数字值的字符串")来获取枚举对象,如果给定的字符串无效，则抛出异常
        /// </summary>
        public static T Parse<T>(string enumNameOrNumber) where T : struct
        {
            return (T)Enum.Parse(typeof(T), enumNameOrNumber);
        }

        /// <summary>
        /// 通过枚举的名称("Enum的名称或数字值的字符串")来获取枚举对象,如果给定的字符串无效，则返回default(T)
        /// </summary>
        public static T ParseOrDefault<T>(string enumNameOrNumber) where T : struct
        {
            if (string.IsNullOrWhiteSpace(enumNameOrNumber)) return default(T);

            T result;
            return Enum.TryParse(enumNameOrNumber, true, out result) ? result : default(T);
        }

        public static List<string> GetDropdownItemsByEnumCnText<T>(bool addAll = false, string allText = "所有") where T : struct
        {
            List<string> result = new List<string>();
            if (addAll) result.Add(allText);

            var items = Enum.GetValues(typeof(T));
            foreach (var item in items)
            {
                result.Add(((Enum)item).GetCnText());
            }
            return result;
        }
    }
}
