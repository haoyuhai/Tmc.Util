using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Tmc.Util
{
    public static class StringExtension
    {
        /// <summary>
        /// 当前的字符串是否为null或空？ eg: string s = null; if(s.IsNullOrEmpty()) { ...}
        /// </summary>
        public static bool IsNullOrEmpty(this String s)
        {
            return String.IsNullOrEmpty(s);
        }

        public static bool ParseBoolOrDefault(this String s)
        {
            if (s == null) return false;
            s = s.Trim();

            return string.Equals(s, "True", StringComparison.CurrentCultureIgnoreCase)
                || string.Equals(s, "Y", StringComparison.CurrentCultureIgnoreCase)
                || s == "1";
        }

        public static int ParseIntOrDefault(this String s)
        {
            int d;
            if (int.TryParse(s, out d))
            {
                return d;
            }
            else
            {
                return 0;
            }
        }        

        public static double ParseDoubleOrDefault(this String s)
        {
            double d;
            if (double.TryParse(s, out d))
            {
                return d;
            }
            else
            {
                return 0;
            }
        }

        public static decimal ParseDecimalOrDefault(this String s)
        {
            decimal d;
            if (decimal.TryParse(s, out d))
            {
                return d;
            }
            else
            {
                return 0;
            }
        }
                
        public static DateTime ParseDateTimeOrDefault(this String s)
        {
            DateTime dt;
            if (DateTime.TryParse(s, out dt))
            {
                return dt;
            }
            else
            {
                return default(DateTime);
            }
        }

        public static DateTime? ParseDateTimeOrNull(this String s)
        {
            DateTime dt;
            if (DateTime.TryParse(s, out dt))
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        public static string TrimStartString(this string target, string trimString)
        {
            if (string.IsNullOrEmpty(target)) return target;
            if (string.IsNullOrEmpty(trimString)) return target;

            string result = target;
            while (result.StartsWith(trimString))
            {
                result = result.Substring(trimString.Length);
            }

            return result;
        }

        public static string TrimEndString(this string target, string trimString)
        {
            if (string.IsNullOrEmpty(target)) return target;
            if (string.IsNullOrEmpty(trimString)) return target;

            string result = target;
            while (result.EndsWith(trimString))
            {
                result = result.Substring(0, result.Length - trimString.Length);
            }

            return result;
        }

        public static List<T> ToList<T>(this string str, char split, Converter<string, T> convertHandler)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new List<T>();
            }
            else
            {
                string[] arr = str.Split(split);
                T[] tArr = Array.ConvertAll(arr, convertHandler);
                return new List<T>(tArr);
            }
        }

        /// <summary>
        /// 用于web页面上，显示前n个字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetFirstNString(this string str, int count)
        {
            if(string.IsNullOrWhiteSpace(str)) return string.Empty;
            if(count < 1) throw new ArgumentOutOfRangeException("count");

            if (str.Length > count)
            {
                return str.Substring(0, count) + "...";
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// 获取手机号码的掩码表示（13288886666变为132****6666，即将中间四位数字变为*）
        /// </summary>
        public static string GetMaskStringForMobile(this string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile)) return mobile;
            if (mobile.Length <= 4) return mobile;

            string result = "";
            for (int i = 0; i < mobile.Length; i++)
            {
                if (i >= 3 && i <= 6)
                {
                    result += '*';
                }    
                else
                {
                    result += mobile[i];
                }
            }
            return result;
        }

        /// <summary>
        /// 获取证件号码的掩码表示（420603197811174026变为**************4026，即保留最后四位，其它变为*）
        /// </summary>
        public static string GetMaskStringForCertNo(this string certNo)
        {
            if (string.IsNullOrWhiteSpace(certNo)) return certNo;
            if (certNo.Length <= 4) return certNo;

            string result = "";
            for (int i = 0; i < certNo.Length; i++)
            {
                if (i < certNo.Length - 4)
                {
                    result += '*';
                }
                else
                {
                    result += certNo[i];
                }
            }
            return result;
        }

        /// <summary>
        /// 如果字符串不为空，返回自身，否则返回替代值
        /// </summary>
        public static string NotNullable(this string str, string valueIfNullabled)
        {
            return string.IsNullOrWhiteSpace(str) ? valueIfNullabled : str;
        }

        public static bool IsValidEmail(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;

            string regEmail = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            return Regex.IsMatch(str, regEmail);
        }

        public static bool IsValidMobile(this string str)
        {
            long val;
            return !string.IsNullOrWhiteSpace(str) && str.Length == 11 && str.StartsWith("1") && long.TryParse(str, out val);
        }

        public static bool IsValidIdCardNo(this string str)
        {
            return !string.IsNullOrWhiteSpace(str) && (str.Length == 15 || str.Length == 18);            
        }
    }
}
