using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVM
{
    /// <summary>
    /// 互通消息对象
    /// </summary>
    public class NotificationMessage : NotifyPropertyBase
    {
        /// <summary>
        /// 消息关键字
        /// </summary>
        public string Key
        {

            get
            {
                return this.GetValue<string>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        /// <summary>
        /// 消息数据内容
        /// </summary>
        public object Data
        {
            get
            {
                return this.GetValue<object>();
            }
            set
            {
                this.SetValue(value);
            }
        }

    }
}
