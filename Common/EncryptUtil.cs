using System;
using System.Security.Cryptography;
using System.Text;

namespace Tmc.Util
{
    /// <summary>
    /// 加密解密工具类
    /// </summary>
    public class EncryptUtil
    {
        #region MD5加密

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="text">加密字符串</param>
        /// <returns></returns>
        public static string MD5(string text)
        {
            StringBuilder sb = new StringBuilder();

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        #endregion

        #region 其他加密

        /// <summary>
        /// 加密key:不允许修改
        /// </summary>
        private string encryptKey
        {
            get { return "s6m5v4a1"; }
        }
        /// <summary>
        ///  加密初始化向量:不允许修改
        /// </summary>
        private string encryptIV
        {
            get { return "7y2e3h89"; }
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string Encrypt(string encryptString)
        {
            CryptoStream cStream = null;
            try
            {
                EncryptUtil des = new EncryptUtil();
                byte[] rgbKey = Encoding.UTF8.GetBytes(des.encryptKey.Substring(0, 8));
                byte[] rgbIV = Encoding.UTF8.GetBytes(des.encryptIV.Substring(0, 8));
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                var mStream = new System.IO.MemoryStream();
                cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            finally
            {
                if (cStream != null) { cStream.Close(); }
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string Decrypt(string decryptString)
        {
            CryptoStream cStream = null;
            try
            {
                EncryptUtil des = new EncryptUtil();
                byte[] rgbKey = Encoding.UTF8.GetBytes(des.encryptKey.Substring(0, 8));
                byte[] rgbIV = Encoding.UTF8.GetBytes(des.encryptIV.Substring(0, 8));
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                var mStream = new System.IO.MemoryStream();
                cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            finally
            {
                if (cStream != null) { cStream.Close(); }
            }
        }

        /// <summary>
        /// 生成随机字符码
        /// </summary>
        /// <returns>产生的随机码</returns>
        public static string CreateVerifyCode()
        {
            int codeLen = length;
            string[] arr = codeSerial.Split(',');

            string code = string.Empty;

            //Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            Random rand = new Random(GetRandomSeed());

            for (int i = 0; i < codeLen; i++)
            {
                int randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }

        #endregion

        #region 随机码

        #region 自定义随机码字符串序列(使用逗号分隔)#region 自定义随机码字符串序列(使用逗号分隔)

        private static readonly string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";

        #endregion

        #region 验证码长度(默认8个验证码的长度)#region 验证码长度(默认8个验证码的长度)

        private static readonly int length = 8;

        #endregion


        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        #endregion
    }// 类结束
}// 命名空间结束
