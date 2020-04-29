﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MVVM
{
    /// <summary>
    /// 
    /// </summary>
    public static class PromptDataExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="obj"> </param>
        /// <param name="propertyName"> </param>
        /// <returns> </returns>
        public static string GetPropertyPromptData<T>(this T obj, string propertyName) where T : NotifyPropertyBase
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return string.Empty;
            }
            Type tp = obj.GetType();
            PropertyInfo pi = tp.GetProperty(propertyName);
            var value = pi.GetValue(obj, null);
            object[] Attributes = pi.GetCustomAttributes(false);
            string strPromptData = "";
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
                                strPromptData = Common.LangHelper.GetValue(vAttribute.ShortName, vAttribute.Prompt, "prompt");
                            }

                            if (String.IsNullOrWhiteSpace(strPromptData))
                            {
                                if (vAttribute.ResourceType == typeof(Common.LanguageResource))
                                {
                                    vAttribute.ResourceType = null;
                                }
                                strPromptData = vAttribute.GetPrompt();
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
            return strPromptData;
        }
    }
}