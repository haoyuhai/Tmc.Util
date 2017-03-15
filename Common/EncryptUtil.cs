using System;
using System.Security.Cryptography;
using System.Text;

namespace Tmc.Util
{
    /// <summary>
    /// ���ܽ��ܹ�����
    /// </summary>
    public class EncryptUtil
    {
        #region MD5����

        /// <summary>
        /// MD5����
        /// </summary>
        /// <param name="text">�����ַ���</param>
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

        #region ��������

        /// <summary>
        /// ����key:�������޸�
        /// </summary>
        private string encryptKey
        {
            get { return "s6m5v4a1"; }
        }
        /// <summary>
        ///  ���ܳ�ʼ������:�������޸�
        /// </summary>
        private string encryptIV
        {
            get { return "7y2e3h89"; }
        }

        /// <summary>
        /// �����ַ���
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
        /// ����
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
        /// ��������ַ���
        /// </summary>
        /// <returns>�����������</returns>
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

        #region �����

        #region �Զ���������ַ�������(ʹ�ö��ŷָ�)#region �Զ���������ַ�������(ʹ�ö��ŷָ�)

        private static readonly string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";

        #endregion

        #region ��֤�볤��(Ĭ��8����֤��ĳ���)#region ��֤�볤��(Ĭ��8����֤��ĳ���)

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
    }// �����
}// �����ռ����
