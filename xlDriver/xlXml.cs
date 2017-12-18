using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace xlTools
{
    public class xlXml
    {
        private String m_xmlFile = "";
        public xlXml(String fileXml,String rootNode)
        {
            m_xmlFile = fileXml;
            //追加XML文档
            XmlDoc = new XmlDocument();
            XmlElement books;
            if (File.Exists(fileXml))
            {
                //如果文件存在 加载XML
                XmlDoc.Load(fileXml);
                //获得文件的根节点
                books = XmlDoc.DocumentElement;
            }
            else
            {
                //如果文件不存在
                //创建第一行
                XmlDeclaration dec = XmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                XmlDoc.AppendChild(dec);
                //创建跟节点
                books = XmlDoc.CreateElement(rootNode);
                XmlDoc.AppendChild(books);
            }
        }
        #region Xml操作
        public XmlDocument XmlDoc = new XmlDocument();
        //public XmlNode xmlnode;
        //public XmlElement xmlelem ;
        //XmlDeclaration xmldecl;
        public void CreateXmlDeclaration(string version, string encoding, string standalone)
        {
            XmlDeclaration xmldecl;
            xmldecl = XmlDoc.CreateXmlDeclaration(version, encoding, standalone);
            XmlDoc.AppendChild(xmldecl);
        }

        public void CreateXmlElement(string prefix, string localName, string namespaceURI)
        {
            XmlElement xmlelem;
            xmlelem = XmlDoc.CreateElement(prefix, localName, namespaceURI);
            XmlDoc.AppendChild(xmlelem);
        }

        public void CreateXmlNode(string xpath, string name)
        {
            XmlNode root = XmlDoc.SelectSingleNode(xpath);//查找<Employees>
            XmlElement xe1 = XmlDoc.CreateElement(name);//创建一个<Node>节点 
            root.AppendChild(xe1);
        }

        public void SetXmlNodeValue(String xpath, String name, String value)
        {
            XmlNode root = XmlDoc.SelectSingleNode(xpath);//查找<Employees>
            XmlElement xe1 = XmlDoc.CreateElement(name);
            xe1.InnerText = value;
            root.AppendChild(xe1);    
        }

        public void RemoveNode(String xpath, String name, Boolean isSave)
        {
            XmlNode xn = XmlDoc.SelectSingleNode(xpath + "/" + name);
            xn.RemoveAll();

            if (isSave)
            {
                XmlDoc.Save(m_xmlFile);
            }
        }

        public void SetAttribute(string name)
        {
            XmlElement xe1 = XmlDoc.CreateElement(name);//创建一个<Node>节点
            xe1.SetAttribute("ISBN", "1-1111-1");//设置该节点ISBN属性 
        }

        public void SaveXml(String filePath)
        {
            XmlDoc.Save(filePath); 
        }

        public void XmlLoad(string Path)
        {
            try
            {
                XmlDoc.Load(Path);
            }
            catch
            {

            }
        }

        //public void XmlModify(string xpath, string name, string value)
        //{
        //    try
        //    {
        //        XmlNode xn = XmlDoc.SelectSingleNode("Test").SelectSingleNode(xpath);
        //        XmlElement xe = (XmlElement)xn;
        //        xe.SetAttribute(name, value);
        //    }
        //    catch
        //    {

        //    }
        //}


        #endregion 
    }
}
