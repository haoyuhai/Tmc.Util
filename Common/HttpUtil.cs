using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Tmc.Util
{
    public static class HttpUtil
    {
        /// <summary>
        /// 向指定的URL地址发起一个GET请求，同时可以传递一些数据项。
        /// </summary>
        /// <param name="url">要请求的URL地址</param>
        /// <param name="keyvalues">要传递的数据项</param>
        /// <param name="encoding">发送，接收的字符编码方式</param>
        /// <param name="timeout"></param>
        /// <returns>服务器的返回结果</returns>        
        public static string SendHttpRequestGet(string url, IDictionary<string, string> keyvalues = null, Encoding encoding = null, int timeout = 100000)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            if (encoding == null)
                encoding = Encoding.UTF8;
            
            // 将数据项转变成 name1=value1&name2=value2 的形式, 拼接在url后面
            if (keyvalues != null && keyvalues.Count > 0)
            {
                string data = string.Join("&",
                        (from kvp in keyvalues
                         let item = kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value)
                         select item
                         ).ToArray()
                     );

                if (!url.Contains('?'))
                {
                    url += "?" + data;
                }
                else
                {
                    url += "&" + data;
                }
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = timeout;

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 向指定的URL地址发起一个POST请求，同时可以上传一些数据项。
        /// </summary>
        /// <param name="url">要请求的URL地址</param>
        /// <param name="keyvalues">要上传的数据项</param>
        /// <param name="encoding">发送，接收的字符编码方式</param>
        /// <returns>服务器的返回结果</returns>        
        public static string SendHttpRequestPost(string url, Dictionary<string, string> keyvalues, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            if (encoding == null)
                encoding = Encoding.UTF8;

            string postData = null;
            // 将数据项转变成 name1=value1&name2=value2 的形式
            if (keyvalues != null && keyvalues.Count > 0)
            {
                postData = string.Join("&",
                        (from kvp in keyvalues
                         let item = kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value)
                         select item
                         ).ToArray()
                     );
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=" + encoding.WebName;

            if (postData != null)
            {
                byte[] buffer = encoding.GetBytes(postData);

                Stream stream = request.GetRequestStream();
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetIP()
        {
            //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
            string userHostAddress = null;
            string xff = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(xff))
            {
                userHostAddress = xff.Split(',')[0].Trim();
            }
            //否则直接读取REMOTE_ADDR获取客户端IP地址
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            }
            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}
