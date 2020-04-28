using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Common
{
    /// <summary>
    /// 
    /// </summary>
    public class LanguageResourceManager : System.Resources.ResourceManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="assembly"></param>
        public LanguageResourceManager(string baseName, Assembly assembly) :
            base(baseName, assembly)
        {
        }

        public override string GetString(string name, System.Globalization.CultureInfo culture)
        {
            LangHelper.SetLang(culture.Name + ".xml");
            //return base.GetString(name, culture);
            return LangHelper.GetValue(name);
        }

        public override string GetString(string name)
        {
            return LangHelper.GetValue(name);
            //return base.GetString(name);
        }

    }
}
