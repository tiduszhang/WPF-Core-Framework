using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Common
{
    /// <summary>
    /// XML工具类
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// 缓存XML文件对象
        /// </summary>
        private static Dictionary<string, XmlDocument> documents = null;

        /// <summary>
        /// 根据关键字获取值
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="keyPath">XML节点路径 直指唯一节点，如："user/person[@name='王五']" 或者 "user/person"</param>
        /// <param name="valuekey">节点指定的属性关键字，若为“”则表示获取节点内的文本</param>
        /// <returns></returns>
        public static string GetValue(string file, string keyPath, string valuekey = "")
        {
            string value = "";

            if (documents == null)
            {
                documents = new Dictionary<string, XmlDocument>();
            }
            try
            {
                if (!System.IO.File.Exists(file))
                {
                    return value;
                }

                //1.XmlDocument类实例化
                XmlDocument xmlDocument = null;

                if (!documents.TryGetValue(file, out xmlDocument))
                {
                    xmlDocument = new XmlDocument();
                    //2.导入指定xml文件
                    xmlDocument.Load(file);
                    documents.Add(file, xmlDocument);
                }

                //3.获取指定节点 
                XmlNode rootNode = xmlDocument.SelectSingleNode(keyPath);

                //获取节点内的文本
                if (String.IsNullOrWhiteSpace(valuekey))
                {
                    value = rootNode.InnerText;
                }

                //根据指定关键字获取属性值
                else if (rootNode != null
                     && rootNode.Attributes != null
                     && rootNode.Attributes.Count > 0
                     && rootNode.Attributes[valuekey] != null)
                {
                    value = rootNode.Attributes[valuekey].Value;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return value;
        }
    }
}
