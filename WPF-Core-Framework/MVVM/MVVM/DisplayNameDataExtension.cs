﻿using System;
using System.ComponentModel.DataAnnotations;
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
                foreach (object attribute in Attributes)
                {
                    if (attribute is DisplayAttribute)
                    {
                        try
                        {
                            DisplayAttribute vAttribute = attribute as DisplayAttribute;
                            if (vAttribute.ResourceType == typeof(Common.LanguageResource))
                            { 
                                strName = Common.LangHelper.GetValue(vAttribute.ShortName);
                            }

                            if (String.IsNullOrWhiteSpace(strName))
                            {
                                if (vAttribute.ResourceType == typeof(Common.LanguageResource))
                                {
                                    vAttribute.ResourceType = null;
                                }
                                strName = vAttribute.GetName();
                            }
                            break;
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }
                    }
                }
            }
            return strName;
        }
    }
}