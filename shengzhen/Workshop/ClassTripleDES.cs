using System;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Net;
namespace Workshop
{
    #region TripleDES�㷨
    public class ClassTripleDES
    {
        [DllImport("ws2_32.dll")]
        private static extern int inet_addr(string cp);
        [DllImport("IPHLPAPI.dll")]
        private static extern int SendARP(Int32 DestIP, Int32 SrcIP, ref Int64 pMacAddr, ref Int32 PhyAddrLen);
        public ClassTripleDES()
        {
        }

        /// <summary>
        /// �����㷨�Ĺ�Կ
        /// </summary>
        public static string passswordkey = "VansCard";

        /// <summary>
        /// ���ܣ�ʹ��������������㷨�Ĺ�Կ����ʹ��TripleDES��������м��ܡ�
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string Encrypt(string pass)
        {
            try
            {
                byte[] bt = (new System.Text.UnicodeEncoding()).GetBytes(pass);
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(pass, null);
                byte[] key = pdb.GetBytes(24);
                byte[] iv = pdb.GetBytes(8);
                MemoryStream ms = new MemoryStream();
                TripleDESCryptoServiceProvider tdesc = new TripleDESCryptoServiceProvider();
                CryptoStream cs = new CryptoStream(ms, tdesc.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                cs.Write(bt, 0, bt.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// ���ܣ�ʹ��������������㷨�Ĺ�Կ����ʹ��TripleDES�Լ������ݽ��н��ܡ�
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string Decrypt(string str, string pass)
        {
            try
            {
                byte[] bt = Convert.FromBase64String(str);
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(pass, null);
                byte[] key = pdb.GetBytes(24);
                byte[] iv = pdb.GetBytes(8);
                MemoryStream ms = new MemoryStream();
                TripleDESCryptoServiceProvider tdesc = new TripleDESCryptoServiceProvider();
                CryptoStream cs = new CryptoStream(ms, tdesc.CreateDecryptor(key, iv), CryptoStreamMode.Write);
                cs.Write(bt, 0, bt.Length);
                cs.FlushFinalBlock();
                return (new System.Text.UnicodeEncoding()).GetString(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }

        //ʹ�ã�
        //string str = Encrypt("bbb");
        //Console.WriteLine(Decrypt(str, "bbb"));

        /// <summary>
        /// ���ܣ�ʹ��������������㷨�Ĺ�Կ����ʹ��TripleDES��������м��ܡ�
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="p_key"></param>
        /// <returns></returns>
        public static string EncryptWithKey(string pass, string p_key)
        {
            try
            {
                byte[] bt = (new System.Text.UnicodeEncoding()).GetBytes(pass);
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(p_key, null);
                byte[] key = pdb.GetBytes(24);
                byte[] iv = pdb.GetBytes(8);
                MemoryStream ms = new MemoryStream();
                TripleDESCryptoServiceProvider tdesc = new TripleDESCryptoServiceProvider();
                CryptoStream cs = new CryptoStream(ms, tdesc.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                cs.Write(bt, 0, bt.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// ���ܣ�ʹ��������������㷨�Ĺ�Կ����ʹ��TripleDES�Լ������ݽ��н��ܡ�
        /// </summary>
        /// <param name="str"></param>
        /// <param name="p_key"></param>
        /// <returns></returns>
        public static string DecryptWithKey(string str, string p_key)
        {
            try
            {
                byte[] bt = Convert.FromBase64String(str);
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(p_key, null);
                byte[] key = pdb.GetBytes(24);
                byte[] iv = pdb.GetBytes(8);
                MemoryStream ms = new MemoryStream();
                TripleDESCryptoServiceProvider tdesc = new TripleDESCryptoServiceProvider();
                CryptoStream cs = new CryptoStream(ms, tdesc.CreateDecryptor(key, iv), CryptoStreamMode.Write);
                cs.Write(bt, 0, bt.Length);
                cs.FlushFinalBlock();
                return (new System.Text.UnicodeEncoding()).GetString(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }
        public static string GetMacAddress(string hostip)//��ȡԶ��IP�����ܿ����Σ���MAC��ַ
        {
            string Mac = "";
            try
            {
                Int32 ldest = inet_addr(hostip); //��IP��ַ�� ������ʽת�����޷��ų�����
                Int64 macinfo = new Int64();
                Int32 len = 6;
                SendARP(ldest, 0, ref macinfo, ref len);
                string TmpMac = Convert.ToString(macinfo, 16).PadLeft(12, '0');//ת����16���ơ���ע����Щû��ʮ��λ
                Mac = TmpMac.Substring(0, 2).ToUpper();//
                for (int i = 2; i < TmpMac.Length; i = i + 2)
                {
                    Mac = TmpMac.Substring(i, 2).ToUpper() + "-" + Mac;
                }
            }
            catch (Exception Mye)
            {
                Mac = "��ȡԶ��������MAC����" + Mye.Message;
            }
            return Mac;
        }
    }

    #endregion

}
