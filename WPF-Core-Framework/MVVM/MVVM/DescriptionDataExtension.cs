using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MVVM
{
    /// <summary>
    /// 
    /// </summary>
    public static class DescriptionDataExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="obj"> </param>
        /// <param name="propertyName"> </param>
        /// <returns> </returns>
        public static string GetPropertyDescription<T>(this T obj, string propertyName) where T : NotifyPropertyBase
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return string.Empty;
            }
            Type tp = obj.GetType();
            PropertyInfo pi = tp.GetProperty(propertyName);
            var value = pi.GetValue(obj, null);
            object[] Attributes = pi.GetCustomAttributes(false);
            string strDescription = "";
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
                            strDescription = Common.LangHelper.GetValue(vAttribute.ShortName, vAttribute.Description, "description");
                        }

                        if (String.IsNullOrWhiteSpace(strDescription))
                        {
                            if (vAttribute.ResourceType == typeof(Common.LanguageResource))
                            {
                                vAttribute.ResourceType = null;
                            }
                            strDescription = vAttribute.GetDescription();
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }
            }
            return strDescription;
        }
    }
}