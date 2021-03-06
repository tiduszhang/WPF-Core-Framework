﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MVVM
{
    /// <summary>
    /// 
    /// </summary>
    public static class DisplayNameDataExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="obj"> </param>
        /// <param name="propertyName"> </param>
        /// <returns> </returns>
        public static string GetPropertyDisplayName<T>(this T obj, string propertyName) where T : NotifyPropertyBase
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return string.Empty;
            }
            Type tp = obj.GetType();
            PropertyInfo pi = tp.GetProperty(propertyName);
            var value = pi.GetValue(obj, null);
            object[] Attributes = pi.GetCustomAttributes(false);
            string strName = "";
            if (Attributes != null && Attributes.Length > 0)
            {
                var attribute = Attributes.FirstOrDefault(o => o is DisplayAttribute);

                if (attribute != null)
                {
                    try
                    {
                        DisplayAttribute vAttribute = attribute as DisplayAttribute;
                        if (vAttribute.ResourceType == typeof(Common.LanguageResource))
                        {
                            strName = Common.LangHelper.GetValue(vAttribute.ShortName, vAttribute.Name, "name");
                        }

                        if (String.IsNullOrWhiteSpace(strName))
                        {
                            if (vAttribute.ResourceType == typeof(Common.LanguageResource))
                            {
                                vAttribute.ResourceType = null;
                            }
                            strName = vAttribute.GetName();
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }
            }
            return strName;
        }
    }
}