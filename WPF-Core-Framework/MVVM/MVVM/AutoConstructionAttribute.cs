using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM
{
    /// <summary>
    /// 自动构造特性（自动注入）
    /// </summary>
    public class AutoConstructionAttribute : Attribute
    {
        /// <summary>
        /// 获取或设置构造类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 构造使用的参数列表
        /// </summary>
        public object[] args { get; set; }

        /// <summary>
        /// 是否为单例模式
        /// </summary>
        public bool IsSingle { get; set; }
         
        /// <summary>
        /// 属性和属性值
        /// </summary>
        internal static IDictionary<Type, object> ValueDictionary = new Dictionary<Type, object>();

    }
}
