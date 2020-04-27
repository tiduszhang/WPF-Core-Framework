﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace MVVM
{
    /// <summary>
    /// 属性短名称
    /// </summary>
    public class ShortNameData 
    { 
        /// <summary>
        /// 拥有本类类型做属性的类
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual NotifyPropertyBase NotifyProperty { get; set; }
         
        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="propertyName"> </param>
        /// <returns> </returns>
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual string this[string propertyName]
        {
            get
            {
                string propertyNameValue = "";
                if (NotifyProperty != null)
                {
                    propertyNameValue = NotifyProperty.GetPropertyShortName(propertyName);
                }
                return propertyNameValue;
            }
        }
         
    }
}
