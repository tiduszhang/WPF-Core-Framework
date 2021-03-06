﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// FileName: WorkPath.cs
    /// CLRVersion: 4.0.30319.42000
    /// @author zhangsx
    /// @date 2017/04/12 11:18:19
    /// Corporation:
    /// Description:    
    /// </summary>
    public static class WorkPath
    {
        /// <summary>     
        /// 公共工作目录-该目录根据程序集名称创建-访问时一般不需要管理员权限，若目录不存在需要手动创建。
        /// </summary>
        public static readonly string ApplicationDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\";

        /// <summary>
        /// 公共工作目录-该目录根据程序集名称创建-访问时一般不需要管理员权限，若目录不存在需要手动创建。
        /// </summary>
        public static readonly string ApplicationWorkPath = ApplicationDataPath + @"\" + System.Reflection.Assembly.GetEntryAssembly().GetName().Name + @"\";

        /// <summary>
        /// 应用程序所在目录-应用程序安装目录，可能需要管理员权限，该目录一定存在。
        /// </summary>
        public static readonly string ExecPath = System.AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 当前应用程序集执行程序名称
        /// </summary>
        public static readonly string AssemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;

        /// <summary>
        /// 获取私有目录-只允许程序集安装目录下子目录
        /// </summary>
        /// <returns></returns>
        public static string GetPrivetePath()
        {
            string strPrivatePath = "";
            try
            {
                var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                System.Xml.XmlDocument configFile = new System.Xml.XmlDocument();
                configFile.Load(config.FilePath);
                strPrivatePath = configFile.SelectSingleNode("configuration/runtime").FirstChild.ChildNodes[0].Attributes[0].Value;
            }
            catch
            {
            }
            return strPrivatePath;
        }
    }
}