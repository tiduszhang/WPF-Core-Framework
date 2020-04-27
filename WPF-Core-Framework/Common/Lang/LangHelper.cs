using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 自定义多国语言帮助类
    /// <![CDATA[
    /// 多国语言文件配置方法，一个语言一个配置文件，默认存放位置为程序目录下“lang”文件夹内，使用xml文件结构，编码格式使用UTF-8。
    /// <langs>
    ///     <config Id="【语言唯一标识，可用代码替代，如：zh_CN】" name="【语言名称】" code="【语言编码】"/>
    ///     <lang key="【关键字1】" value="【值】"/>
    ///     <lang key="【关键字2】" value="【值】"/>
    /// </langs>
    /// ]]>
    /// </summary>
    public static class LangHelper
    {
        /// <summary>
        /// 默认语言文件
        /// </summary>
        private static readonly string langDefaultFile = "default.xml";

        /// <summary>
        /// 默认语言文件
        /// </summary>
        private static string langFile = "default.xml";

        /// <summary>
        /// 语言文件所在文件夹
        /// </summary>
        private static string langPath = WorkPath.ExecPath + @"\lang\";

        /// <summary>
        /// 预言缓存
        /// </summary>
        private static System.Collections.Specialized.HybridDictionary dictionaryLangs = null;

        /// <summary>
        /// 所有语言对应字典
        /// </summary>
        private static List<Language> languages = null;

        /// <summary>
        /// 根据关键字获取对应语言的文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            // 当前语言
            string currentLangFile = langPath + langFile;

            // 默认语言 
            string defaultLangFile = langPath + langDefaultFile;

            //文本
            string value = "";

            //没有key
            if (String.IsNullOrWhiteSpace(key))
            {
                return value;
            }

            //缓存中获取
            if (dictionaryLangs == null)
            {
                dictionaryLangs = new System.Collections.Specialized.HybridDictionary();
            }

            lock (dictionaryLangs)
            {
                if (dictionaryLangs.Contains(key))
                {
                    value = dictionaryLangs[key].ToString();
                }
            }

            //优先获取指定的语言
            if (String.IsNullOrWhiteSpace(value)
               && System.IO.File.Exists(currentLangFile))
            {
                value = XmlHelper.GetValue(currentLangFile, @"langs/lang[@key='" + key + "']", "value");
            }

            //若指定的语言没有值则获取默认语言
            if (String.IsNullOrWhiteSpace(value)
                && System.IO.File.Exists(defaultLangFile))
            {
                value = XmlHelper.GetValue(defaultLangFile, @"langs/lang[@key='" + key + "']", "value");
            }

            if (!String.IsNullOrWhiteSpace(value))
            {
                lock (dictionaryLangs)
                {
                    if (!dictionaryLangs.Contains(key))
                    {
                        dictionaryLangs.Add(key, value);
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// 设置语言文件
        /// </summary>
        /// <param name="langFile">文件名不含路径，但包含后缀名。</param>
        public static void SetLang(string langFile = "")
        {
            if (!String.IsNullOrWhiteSpace(langFile))
            {
                LangHelper.langFile = langFile;
            }
            else
            {
                LangHelper.langFile = LangHelper.langDefaultFile;
            }

            if (dictionaryLangs == null)
            {
                dictionaryLangs = new System.Collections.Specialized.HybridDictionary();
            }

            dictionaryLangs.Clear();

            if (languages != null && languages.Count > 0)
            {
                var language = languages.FirstOrDefault(o => o.LanguageFile == langFile);
                if (language != null)
                {
                    var culture = new System.Globalization.CultureInfo(language.ID);
                    System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                }
            }

        }

        /// <summary>
        /// 获取所有语言
        /// </summary>
        /// <returns></returns>
        public static List<Language> GetALLLanguages()
        {
            if (languages == null)
            {
                languages = new List<Language>();
            }

            if (languages.Count > 0)
            {
                return languages;
            }

            if (System.IO.Directory.Exists(langPath))
            {
                var files = System.IO.Directory.GetFiles(langPath);

                if (files != null && files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        var id = XmlHelper.GetValue(file, @"langs/config", "Id");
                        var name = XmlHelper.GetValue(file, @"langs/config", "name");
                        var code = XmlHelper.GetValue(file, @"langs/config", "code");

                        if (String.IsNullOrWhiteSpace(id)  //没有id
                            || String.IsNullOrWhiteSpace(name) //没有name
                            || String.IsNullOrWhiteSpace(code) //没有code
                            || languages.Exists(o => o.ID == id)) //重复的
                        {
                            continue;//跳过获取下一个
                        }

                        languages.Add(new Language()
                        {
                            LanguageFile = new System.IO.FileInfo(file).Name,//文件名
                            ID = id,//id
                            Name = name,//name
                            Code = code//code
                        });
                    }
                }
            }

            return languages;
        }
    }
}
