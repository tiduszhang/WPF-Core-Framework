using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 语言对象
    /// </summary>
    public class Language
    {
        /// <summary>
        /// 语言唯一标识，可用代码替代，如：zh_CN
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 语言文件
        /// </summary>
        public string LanguageFile { get; set; }

        /// <summary>
        /// 语言名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 语言编号
        /// </summary>
        public string Code { get; set; }
    }
}
