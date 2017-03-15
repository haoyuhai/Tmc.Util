using System;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Tmc.Util
{
    /// <summary>
    /// JavaScriptSerializer 帮助类
    /// </summary>
    public static class JsonUtil
    {
        #region 处理Json字符串的时间格式问题
        /// <summary>
        /// 处理JsonString的时间格式问题
        /// <para>eg:ScriptSerializerHelper.ParseJsonTime(@"[{'getTime':'\/Date(1419564257428)\/'}]", "yyyyMMdd hh:mm:ss");==>[{'getTime':'20141226 11:24:17'}]</para>
        /// <para>参考：http://www.cnphp6.com/archives/35773 </para>
        /// </summary>
        /// <param name="jsonString">Json字符串</param>
        /// <param name="format">时间格式化类型</param>
        /// <returns>处理好的Json字符串</returns>
        public static string ParseJsonTime(string jsonString, string format)
        {
            if (!string.IsNullOrEmpty(jsonString))
            {
                jsonString = Regex.Replace(jsonString, @"\\/Date\((\d+)\)\\/", match =>
                {
                    DateTime _dateTime = new DateTime(1970, 1, 1);
                    _dateTime = _dateTime.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    _dateTime = _dateTime.ToLocalTime();
                    return _dateTime.ToString(format);
                });
            }
            return jsonString;
        }

        /// <summary>
        /// 处理JsonString的时间格式问题【时间格式：yyyy-MM-dd HH:mm:ss】
        /// <para>参考：http://www.cnphp6.com/archives/35773 </para>
        /// </summary>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>处理好的Json字符串</returns>
        public static string ParseJsonTime(string jsonString)
        {
            return ParseJsonTime(jsonString, "yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        #region 利用JavaScriptSerializer将对象序列化成JSON
        /// <summary>
        /// 利用JavaScriptSerializer将对象序列化成JSON字符串        
        /// </summary>
        public static string Serialize<T>(T obj) where T : class
        {
            if (obj == null) return string.Empty;

            var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            return serializer.Serialize(obj);
        }
        #endregion

        #region 利用JavaScriptSerializer将json字符串反序列化
        /// <summary>
        ///利用JavaScriptSerializer将json字符串反序列化        
        /// </summary>
        public static T Deserialize<T>(string jsonString) where T : class
        {
            if (string.IsNullOrWhiteSpace(jsonString)) return null;
            
            var serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            return serializer.Deserialize<T>(jsonString);
        }
        #endregion
    }
}
