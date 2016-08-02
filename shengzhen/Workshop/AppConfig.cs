using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Workshop
{
    /// 
    /// C#�ж�̬��дApp.config�����ļ�
    /// 
    public class AppConfig
    {
        public AppConfig()
        {
            ///
            /// TODO: �ڴ˴���ӹ��캯���߼�
            ///
        }
        /// 
        /// д����
        /// 
        public static void ConfigSetValue(string strExecutablePath, string AppKey, string AppValue)
        {
            XmlDocument xDoc = new XmlDocument();
            //��ȡ��ִ���ļ���·��������
            xDoc.Load(strExecutablePath + ".config");

            XmlNode xNode;
            XmlElement xElem1;
            XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//appSettings");
            // xDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
            xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", AppValue);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", AppKey);
                xElem2.SetAttribute("value", AppValue);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(strExecutablePath + ".config");
        }
        /// 
        /// ������
        /// 
        /// 
        /// 
        /// 
        public static string ConfigGetValue(string strExecutablePath, string appKey)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(strExecutablePath + ".config");

                XmlNode xNode;
                XmlElement xElem;
                xNode = xDoc.SelectSingleNode("//appSettings");
                xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
                if (xElem != null)
                    return xElem.GetAttribute("value");
                else
                    return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}


