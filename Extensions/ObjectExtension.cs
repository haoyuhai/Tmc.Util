using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tmc.Util
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 将对象序列化为Xml
        /// </summary>
        public static string ToXmlString(this object @this)
        {
            return XmlUtil.SerializeToXml(@this);
        }

        /// <summary>
        /// 获取对象的ToString()结果，如果为null则返回string.Empty
        /// </summary>
        public static string ToStringSafely(this object @this)
        {
            return @this != null ? @this.ToString() : string.Empty;
        }

        /// <summary>
        /// 获取对象的ToString().Trim()结果，如果为null则返回string.Empty
        /// </summary>
        public static string TrimSafely(this object @this)
        {
            return @this != null ? @this.ToString().Trim() : string.Empty;
        }

        /// <summary>
        /// 克隆一个新的对象(深拷贝,采用二进制格式化的方式)
        /// </summary>
        public static object Clone(this object @this)
        {
            if (@this == null) return null;

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(ms, @this);
                ms.Position = 0;
                return bformatter.Deserialize(ms);
            }
        }
    }
}
